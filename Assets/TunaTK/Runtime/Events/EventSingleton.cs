// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:35 AM

using UnityEngine;

namespace TunaTK.Events
{
	/// <summary>The base class for any singleton. This allows easy automated generation of singletons. Inherits from EventMonoBehaviour making it automatically register into the event system</summary>
	/// <typeparam name="T">The type of the singleton. The type specified will automatically make <see cref="Instance"/> the same type.</typeparam>
	public class EventSingleton<T> : EventBehaviour where T : EventSingleton<T>, IEventHandler
	{
		/// <summary>The static reference that allows anywhere and anything can be used to access the singleton.</summary>
		public static T Instance
		{
			get
			{
				// If the instance itself hasn't already been set, try and find it.
				if(instance == null)
				{
					instance = FindObjectOfType<T>();

					// If the instance is still invalid, we will create one and log out the error
					if(instance == null)
					{
						Debug.LogError($"Internal '{typeof(T).Name}' singleton instance was null, auto creating...");
						instance = new GameObject($"{typeof(T).Name} Singleton").AddComponent<T>();
					}
				}

				return instance;
			}
		}

		/// <summary>Checks that the internal instance variable is valid without generating the instance itself.</summary>
		public static bool IsInstanceValid() => instance != null;

		/// <summary>The actual instance reference that will store the instance reference.</summary>
		protected static T instance;

		/// <summary>This will make the singleton that has been generated not be destroyed between scene changes.</summary>
		protected void FlagAsPersistant() => DontDestroyOnLoad(this.gameObject);

		/// <summary>Generates the singleton instance by accessing the <see cref="Instance"/> variable.</summary>
		protected virtual T CreateSingletonInstance() => Instance;
	}
}