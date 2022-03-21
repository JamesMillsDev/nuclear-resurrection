// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 25/05/2021 05:51 PM
// Created On: MODI

using System;
using System.Linq;
using System.Reflection;

using UnityEngine;

using Object = UnityEngine.Object;

namespace TunaTK.Utility
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class RequireRuntimeAttribute : PropertyAttribute
	{
		/// <summary>
		/// This function runs just before the scene is fully loaded in order
		/// to add the required components onto the gameObject requiring them.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void LoadEvent()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			// Loop through all assemblies attached in this game
			foreach(Assembly assembly in assemblies)
			{
				Type[] types = assembly.GetTypes().Where(_type => _type.GetCustomAttributes(typeof(RequireRuntimeAttribute)).ToArray().Length > 0).ToArray();

				// Find all types using the RequireRuntimeAttribute Attribute on them
				foreach(Type type in types)
				{
					RequireRuntimeAttribute[] attributes = type.GetCustomAttributes(typeof(RequireRuntimeAttribute)).Cast<RequireRuntimeAttribute>().ToArray();
					Object[] foundObjects = Object.FindObjectsOfType(type);

					// Loop through all found instances of the component found
					foreach(Object obj in foundObjects)
					{
						// Loop through all attributes on each found component references
						foreach(RequireRuntimeAttribute attribute in attributes)
						{
							// Get the gameObject reference from the type
							GameObject gameObject = type.GetProperty("gameObject")?.GetValue(obj) as GameObject;

							// ReSharper disable once PossibleNullReferenceException
							if(gameObject.GetComponent(attribute.requiredType) == null)
								gameObject.AddComponent(attribute.requiredType);

							if(attribute.requiredType2 != null)
							{
								// ReSharper disable once PossibleNullReferenceException
								if(gameObject.GetComponent(attribute.requiredType2) == null)
									gameObject.AddComponent(attribute.requiredType2);
							}

							if(attribute.requiredType3 != null)
							{
								// ReSharper disable once PossibleNullReferenceException
								if(gameObject.GetComponent(attribute.requiredType3) == null)
									gameObject.AddComponent(attribute.requiredType3);
							}
						}
					}
				}
			}
		}

		/// <summary> The first type that will be added to the object. This is the only mandatory one. </summary>
		private readonly Type requiredType;
		/// <summary> The second possible type that can be added to the object. </summary>
		private readonly Type requiredType2;
		/// <summary> The third possible type that can be added to the object. </summary>
		private readonly Type requiredType3;

		/// <summary>  </summary>
		/// <param name="_required"></param>
		public RequireRuntimeAttribute(Type _required)
		{
			requiredType = _required;
			requiredType2 = null;
			requiredType3 = null;
		}

		/// <summary>  </summary>
		/// <param name="_required"></param>
		/// <param name="_required2"></param>
		public RequireRuntimeAttribute(Type _required, Type _required2)
		{
			requiredType = _required;
			requiredType2 = _required2;
			requiredType3 = null;
		}

		/// <summary>  </summary>
		/// <param name="_required"></param>
		/// <param name="_required2"></param>
		/// <param name="_required3"></param>
		public RequireRuntimeAttribute(Type _required, Type _required2, Type _required3)
		{
			requiredType = _required;
			requiredType2 = _required2;
			requiredType3 = _required3;
		}
	}
}