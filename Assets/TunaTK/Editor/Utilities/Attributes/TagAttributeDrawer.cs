#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 13/11/2018 10:00 PM
// Created On: CHRONOS

using UnityEditor;

using UnityEngine;

namespace TunaTK
{
	[CustomPropertyDrawer(typeof(TagAttribute))]
	public class TagAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			EditorGUI.BeginProperty(_position, _label, _property);

			bool isNotSet = string.IsNullOrEmpty(_property.stringValue);

			_property.stringValue = EditorGUI.TagField(_position, _label, isNotSet ? (_property.serializedObject.targetObject as Component)?.gameObject.tag : _property.stringValue);

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) => EditorGUIUtility.singleLineHeight;
	}
}
#endif