#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 08/04/2019 03:19 PM
// Created On: DESKTOP-JN21C3R

using UnityEditor;

using UnityEngine;

namespace TunaTK.Utilities
{
	[CustomPropertyDrawer(typeof(TimeInput))]
	public class TimeInputDrawer : PropertyDrawer
	{
		private const float PADDING = 2f;

		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			SerializedProperty minutesProp = _property.FindPropertyRelative("minutes");
			SerializedProperty secondsProp = _property.FindPropertyRelative("seconds");

			minutesProp.floatValue = Mathf.Clamp(minutesProp.floatValue, 0, 59.9f);
			secondsProp.floatValue = Mathf.Clamp(secondsProp.floatValue, 0, 59.9f);

			GUI.Box(_position, "");
			{
				TimeInput timeInput = new TimeInput(minutesProp.floatValue, secondsProp.floatValue);
				GUIContent content = new GUIContent($"{_label.text} ({timeInput.Value} seconds)");

				Rect position = GetPaddedRect(new Rect(_position.x, _position.y, _position.width, EditorGUIUtility.singleLineHeight), PADDING);
				EditorGUI.LabelField(position, content, EditorStyles.boldLabel);

				position.y += EditorGUIUtility.singleLineHeight;
				position.width /= 2;

				EditorGUI.PropertyField(position, minutesProp);

				position.x += position.width;
				EditorGUI.PropertyField(position, secondsProp);
			}
		}

		public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label) => (EditorGUIUtility.singleLineHeight * 2f) + (PADDING * 2f);

		private Rect GetPaddedRect(Rect _baseRect, float _padding) => new Rect(_baseRect.x + _padding, _baseRect.y + _padding, _baseRect.width - (_padding * 2f), _baseRect.height);
	}
}
#endif