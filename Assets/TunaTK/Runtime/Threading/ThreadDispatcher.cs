// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 13/03/2022 12:19 PM

using Sirenix.OdinInspector;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunaTK.Threading
{
	public class ThreadDispatcher : Singleton<ThreadDispatcher>
	{
		[SerializeField, ReadOnly] private List<ThreadRunner> runners = new List<ThreadRunner>();
		protected readonly Queue<Action> pending = new Queue<Action>();

		// ReSharper disable once FieldCanBeMadeReadOnly.Local
		[SerializeField, ReadOnly] private List<Thread> threads = new List<Thread>();

		private void Awake()
		{
			FlagAsPersistant();

			SceneManager.sceneLoaded += OnSceneLoaded;

			CreateThreads();
		}

		private void OnDestroy() => StopThreads();

		private void Update()
		{
			lock(pending)
			{
				while(pending.Count != 0)
					pending.Dequeue().Invoke();
			}

			foreach(Thread thread in threads.Where(_thread => !_thread.IsAlive))
			{
				threads.Remove(thread);
				break;
			}
		}

		public void Invoke(Action _function)
		{
			lock(pending)
			{
				pending.Enqueue(_function);
			}
		}

		public void StartThread(ThreadRunner _runner)
		{
			_runner.Prepare(this);
			Thread runnerThread = new Thread(_runner.WorkerThread);
			runnerThread.Start();
			threads.Add(runnerThread);
		}

		private void OnSceneLoaded(Scene _scene, LoadSceneMode _loadSceneMode)
		{
			StopThreads();
			CreateThreads();
		}

		private void LocateRunners()
		{
			runners.Clear();
			foreach(ThreadRunner runner in FindObjectsOfType<ThreadRunner>())
				runners.Add(runner);
		}

		private void CreateThreads()
		{
			LocateRunners();
			
			runners.ForEach(StartThread);
		}

		private void StopThreads()
		{
			threads.ForEach(_thread => _thread.Abort());
			threads.Clear();
		}
	}
}