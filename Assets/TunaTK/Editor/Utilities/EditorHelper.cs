#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 17/04/2019 06:51 PM
// Created On: CHRONOS

using Sirenix.OdinInspector.Editor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace TunaTK
{
	public static class EditorHelper
	{
		public static Texture GetModuleLogo(string _moduleName) => Resources.Load<Texture>(_moduleName != "Core" ? $"TunaTK/{_moduleName}/Logo" : "TunaTK/Logo");

		public static int GetPropertyCountExcluding(PropertyTree _tree, params string[] _propertyToExclude)
		{
			// Loop through properties and create one field (including children) for each top level property.
			InspectorProperty property = _tree.GetRootProperty(0);
			int count = 0;
			while(property != null)
			{
				if(!_propertyToExclude.Contains(property.Name))
					count++;
				
				property = property.NextProperty(false);
			}

			return count;
		}

		public static void DrawBanner(string _title, int _fontSize, Texture _texture)
		{
			GUI.DrawTexture(new Rect(0, 0, 400, 100), _texture);
			GUI.Label(new Rect(75, 25, 350, 50), _title, GetHeaderStyle(_fontSize));
		}

		public static GUIStyle GetHeaderStyle(int _fontSize)
		{
			GUIStyle newStyle = new GUIStyle(GUI.skin.label)
			{
				fontStyle = FontStyle.Bold,
				fontSize = _fontSize,
				alignment = TextAnchor.MiddleCenter
			};

			return newStyle;
		}

		public static bool LayoutToggleButton(GUIContent _content, bool _buttonToggle)
		{
			GUIStyles.Rebuild();

			GUIStyle normalStyle = new GUIStyle("Button");
			GUIStyle activeStyle = new GUIStyle(normalStyle)
			{
				onNormal =
				{
					background = normalStyle.onActive.background
				}
			};

			return GUILayout.Button(_content, _buttonToggle ? activeStyle : normalStyle, GUILayout.ExpandHeight(true));
		}

		public static void Spacer(float _space, bool _vertical = false) => GUILayout.Box("", GUIStyles.Spacer, _vertical ? GUILayout.Height(_space) : GUILayout.ExpandHeight(true), _vertical ? GUILayout.ExpandWidth(true) : GUILayout.Width(_space));

		public static void HeaderField(string _headerText) => EditorGUILayout.LabelField(_headerText, EditorStyles.boldLabel);

		public static Vector2 MinMaxField(string _title, Vector2 _value)
		{
			Vector2 value = _value;

			EditorGUILayout.LabelField(_title);
			EditorGUI.indentLevel++;
			{
				value.x = EditorGUILayout.FloatField("Min", value.x);
				value.y = EditorGUILayout.FloatField("Max", value.y);
			}
			EditorGUI.indentLevel--;

			return value;
		}

		public static void CenterOnMainWindow(this EditorWindow _window, float _width, float _height)
		{
			Rect main = GetEditorMainWindowPos();
			Rect pos = _window.position;
			pos.width = _width;
			pos.height = _height;

			float w = (main.width - pos.width) * 0.5f;
			float h = (main.height - pos.height) * 0.5f;
			pos.x = main.x + w;
			pos.y = main.y + h;
			_window.position = pos;
		}

		public static Rect GetEditorMainWindowPos()
		{
			Type containerWinType = AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(ScriptableObject)).FirstOrDefault(_type => _type.Name == "ContainerWindow");
			if(containerWinType == null)
				throw new MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");

			FieldInfo showModeField = containerWinType.GetField("m_ShowMode", BindingFlags.NonPublic | BindingFlags.Instance);
			PropertyInfo positionProperty = containerWinType.GetProperty("position", BindingFlags.Public | BindingFlags.Instance);

			if(showModeField == null || positionProperty == null)
				throw new MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");

			Object[] windows = Resources.FindObjectsOfTypeAll(containerWinType);
			foreach(Object win in windows)
			{
				int showmode = (int)showModeField.GetValue(win);
				if(showmode == 4) // main window
				{
					Rect pos = (Rect)positionProperty.GetValue(win, null);
					return pos;
				}
			}

			throw new NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
		}

		private static Type[] GetAllDerivedTypes(this AppDomain _appDomain, Type _type)
		{
			List<Type> result = new List<Type>();
			Assembly[] assemblies = _appDomain.GetAssemblies();
			foreach(Assembly assembly in assemblies)
			{
				Type[] types = assembly.GetTypes();
				foreach(Type type in types)
				{
					if(type.IsSubclassOf(_type))
						result.Add(type);
				}
			}

			return result.ToArray();
		}
	}
}
#endif