// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/14/2018 10:04:40 PM
// Created On: CHRONOS

using UnityEngine;

namespace TunaTK
{
	/// <summary>The base class for any Initialisable singleton. This allows easy automated generation of singletons.</summary>
	/// <typeparam name="T">The type of the Initialisable singleton. The type specified will automatically make <see cref="Instance"/> the same type.</typeparam>
	public abstract class InitialiseBehaviourSingleton<T> : InitialiseBehaviour where T : InitialiseBehaviourSingleton<T>
	{
		/// <summary>The static reference that allows anywhere and anything can be used to access the singleton.</summary>
		// ReSharper disable once MemberCanBeProtected.Global
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
		protected static T instance = null;

		/// <summary>This will make the singleton that has been generated not be destroyed between scene changes.</summary>
		protected void FlagAsPersistant() => DontDestroyOnLoad(this.gameObject);

		/// <summary>Generates the singleton instance by accessing the <see cref="Instance"/> variable.</summary>
		protected virtual T CreateSingletonInstance() => Instance;
	}
}