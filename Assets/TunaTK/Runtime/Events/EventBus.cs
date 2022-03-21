// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:24 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using UnityEngine;

namespace TunaTK.Events
{
	public static class EventBus
	{
		private static readonly Dictionary<Type, List<MethodInfo>> events;

		private static readonly List<object> registeredListeners;

		static EventBus()
		{
			events = new Dictionary<Type, List<MethodInfo>>();
			registeredListeners = new List<object>();
			new Thread(LoadEventsThread).Start();
		}

		private static void LoadEventsThread()
		{
			List<Type> eventListeners = new List<Type>();

			List<Assembly> assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies().Where(_assembly => _assembly.FullName.Contains("CSharp") || _assembly.FullName.Contains("tunatk")));

			foreach(Assembly assembly in assemblies)
			{
				foreach(Type type in assembly.GetTypes())
				{
					if(type.GetInterfaces().Contains(typeof(IEventHandler)))
						eventListeners.Add(type);
				}
			}

			foreach(Type listener in eventListeners)
			{
				foreach(MethodInfo method in listener.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly))
				{
					if(method.GetCustomAttributes(typeof(SubscribeEventAttribute), true).Length > 0)
					{
						ParameterInfo[] parameters = method.GetParameters();
						if(parameters.Length == 1)
						{
							Type paramType = parameters[0].ParameterType;
							if(!events.ContainsKey(paramType))
								events.Add(paramType, new List<MethodInfo>(new[] { method }));
							else
								events[paramType].Add(method);
						}
						else
						{
							Debug.LogError($"Event function {method.Name} in {listener.Name} class has more than one parameter, only one allowed.");
						}
					}
				}
			}
		}

		public static void RegisterObject(object _eventObject)
		{
			if(!registeredListeners.Contains(_eventObject))
				registeredListeners.Add(_eventObject);
		}

		public static void RemoveObject(object _eventObject)
		{
			if(registeredListeners.Contains(_eventObject))
				registeredListeners.Remove(_eventObject);
		}

		public static void Raise<EVENT>(EVENT _event) where EVENT : BaseEvent
		{
			foreach(List<MethodInfo> methods in events.Values.Where(_eventType => _eventType.Select(_method => _method.GetParameters()[0].ParameterType).First() == typeof(EVENT)))
			{
				foreach(MethodInfo method in methods)
				{
					foreach(object listener in registeredListeners.Where(_listener => method.DeclaringType == _listener.GetType()))
					{
						method.Invoke(listener, new BaseEvent[] { _event });
					}
				}
			}
		}
	}
}