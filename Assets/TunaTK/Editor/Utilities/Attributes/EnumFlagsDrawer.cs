#if UNITY_EDITOR
// Property of PYROCRACY STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 15/01/2019 09:55 AM
// Created On: DESKTOP-JN21C3R

using System;
using System.Reflection;

using UnityEditor;

using UnityEngine;
#pragma warning disable 618

namespace TunaTK
{
	[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
	public class EnumFlagsDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			Enum targetEnum = (Enum) Enum.ToObject(fieldInfo.FieldType, _property.intValue);

			EditorGUI.BeginChangeCheck();
			EditorGUI.BeginProperty(_position, _label, _property);
			
			Enum enumNew = EditorGUI.EnumMaskField(_position, _property.displayName, targetEnum);
			
			if(!_property.hasMultipleDifferentValues || EditorGUI.EndChangeCheck())
				_property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
			
			EditorGUI.EndProperty();
		}
	}
}
#endif