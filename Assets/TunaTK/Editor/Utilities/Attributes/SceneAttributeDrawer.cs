#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/14/2018 10:13:23 PM
// Created On: CHRONOS

using TunaTK.Utility;

using UnityEditor;

using UnityEngine;

namespace TunaTK
{
	[CustomPropertyDrawer(typeof(SceneAttribute))]
	public class SceneAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			SceneAsset oldPath = AssetDatabase.LoadAssetAtPath<SceneAsset>(_property.stringValue);

			_position = EditorGUI.PrefixLabel(_position, new GUIContent(_property.displayName));

			EditorGUI.BeginChangeCheck();
			SceneAsset newScene = EditorGUI.ObjectField(_position, oldPath, typeof(SceneAsset), false) as SceneAsset;

			if(EditorGUI.EndChangeCheck())
			{
				string newPath = AssetDatabase.GetAssetPath(newScene);
				_property.stringValue = newPath;
			}
		}
	}
}
#endif