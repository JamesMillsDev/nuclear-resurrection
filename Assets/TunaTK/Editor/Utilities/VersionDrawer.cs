#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 28/10/2019 06:37 PM
// Created On: CHRONOS

using TunaTK.Utility;

using UnityEditor;

using UnityEngine;

namespace TunaTK
{
	[CustomPropertyDrawer(typeof(Version))]
	public class VersionDrawer : PropertyDrawer
	{
		private const float PADDING = 2f;
		private const int LINE_COUNT = 3;

		// Draw the custom property
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			SerializedProperty majorProp = _property.FindPropertyRelative("Major");
			SerializedProperty minorProp = _property.FindPropertyRelative("Minor");
			SerializedProperty patchProp = _property.FindPropertyRelative("Patch");
			SerializedProperty releaseProp = _property.FindPropertyRelative("Release");

			majorProp.intValue = Mathf.RoundToInt(Mathf.Clamp(majorProp.intValue, 0, 100f));
			minorProp.intValue = Mathf.RoundToInt(Mathf.Clamp(minorProp.intValue, 0, 100f));
			patchProp.intValue = Mathf.RoundToInt(Mathf.Clamp(patchProp.intValue, 0, 100f));

			GUI.Box(_position, "");
			{
				Version version = new Version(majorProp.intValue, minorProp.intValue, patchProp.intValue, (ReleaseType)releaseProp.enumValueIndex);
				GUIContent content = new GUIContent($"Version: {version}");

				Rect position = GetPaddedRect(new Rect(_position.x, _position.y, _position.width, EditorGUIUtility.singleLineHeight), PADDING);
				EditorGUI.LabelField(position, content, EditorStyles.boldLabel);

				position.y += EditorGUIUtility.singleLineHeight + PADDING;
				int[] versionArray = new int[] { majorProp.intValue, minorProp.intValue, patchProp.intValue };
				EditorGUI.MultiIntField(position, new[] { new GUIContent(""), new GUIContent(""), new GUIContent("") }, versionArray);

				majorProp.intValue = versionArray[0];
				minorProp.intValue = versionArray[1];
				patchProp.intValue = versionArray[2];

				position.y += EditorGUIUtility.singleLineHeight + PADDING;
				EditorGUI.PropertyField(position, releaseProp);
			}
		}

		public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) => (EditorGUIUtility.singleLineHeight * LINE_COUNT) + (PADDING * (LINE_COUNT + 1));

		private Rect GetPaddedRect(Rect _baseRect, float _padding) => new Rect(_baseRect.x + _padding, _baseRect.y + _padding, _baseRect.width - (_padding * 2f), _baseRect.height);
	}
}
#endif