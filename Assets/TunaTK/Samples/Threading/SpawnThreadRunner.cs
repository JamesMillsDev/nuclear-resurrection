// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 13/03/2022 12:22 PM

using System.Threading;

using UnityEngine;

namespace TunaTK.Threading.Samples
{
	public class SpawnThreadRunner : ThreadRunner 
	{
		[SerializeField] private int spawnCount;
		[SerializeField, Range(50, 1000)] private int interval = 50;
		
		public override void WorkerThread()
		{
			for(int i = 0; i < spawnCount; i++)
			{
				Thread.Sleep(interval);

				int index = i;
				
				dispatcher.Invoke(() => CreateObject(index));
			}
		}

		private void CreateObject(int _index)
		{
			GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			newObject.transform.parent = transform;
			newObject.name = $"Enemy {_index + 1}";
			newObject.transform.localPosition = Vector3.forward * _index;
		}
	}
}