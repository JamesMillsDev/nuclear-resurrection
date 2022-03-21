#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/15/2018 8:17:31 PM
// Created On: CHRONOS

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace TunaTK
{
	public static class FindTools
	{
	#region ScriptableObject

		[MenuItem("CONTEXT/ScriptableObject/Ping", false, 400)]
		private static void PingScriptableObject(MenuCommand _data)
		{
			Object context = _data.context;
			if(context)
			{
				EditorGUIUtility.PingObject(context);
			}
		}

	#endregion

	#region Component

		[MenuItem("CONTEXT/Component/Ping", false, 400)]
		private static void PingObject(MenuCommand _data)
		{
			Object context = _data.context;
			if(context)
			{
				if(context is Component component)
				{
					GameObject go = component.gameObject;
					EditorGUIUtility.PingObject(go);
				}
				else
				{
					EditorGUIUtility.PingObject(context);
				}
			}
		}

		[MenuItem("CONTEXT/Component/Find references to this", false, 410)]
		private static void FindReferences(MenuCommand _data)
		{
			Object context = _data.context;
			if(context)
			{
				Component component = context as Component;
				if(component)
				{
					FindReferencesTo(component);
				}
			}
		}

		private static void FindReferencesTo(Component _to)
		{
			List<GameObject> referenceBy = new List<GameObject>();
			GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
			for(int j = 0; j < allObjects.Length; j++)
			{
				GameObject go = allObjects[j];

				if(PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular)
				{
					// ReSharper disable once SuspiciousTypeConversion.Global
					if(PrefabUtility.GetCorrespondingObjectFromSource(go) == _to)
					{
						Debug.Log($"Referenced by {go.name}, {go.GetType()}", go);
						referenceBy.Add(go);
					}
				}

				Component[] components = go.GetComponents<Component>();
				for(int i = 0; i < components.Length; i++)
				{
					Component component = components[i];
					if(!component)
					{
						continue;
					}

					SerializedObject serializedObject = new SerializedObject(component);
					SerializedProperty serializedProp = serializedObject.GetIterator();

					while(serializedProp.NextVisible(true))
					{
						if(serializedProp.propertyType == SerializedPropertyType.ObjectReference)
						{
							if(serializedProp.objectReferenceValue == _to)
							{
								Debug.Log($"Referenced by {component.name}, {component.GetType()}", component);
								referenceBy.Add(component.gameObject);
							}
						}
					}
				}
			}

			if(referenceBy.Count > 0)
			{
				// ReSharper disable once CoVariantArrayConversion
				Selection.objects = referenceBy.ToArray();
			}
			else
			{
				Debug.Log("No references in scene.");
			}
		}

	#endregion
	}
}
#endif