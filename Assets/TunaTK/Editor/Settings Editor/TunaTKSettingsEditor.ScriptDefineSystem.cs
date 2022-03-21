#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/27/2018 10:12:15 AM
// Created On: CHRONOS

using UnityEditor;
using UnityEditor.AnimatedValues;

using UnityEngine;

namespace TunaTK
{
	public partial class TunaTkSettingsEditor
	{
		private readonly AnimBool overrideUsername = new AnimBool();

		protected void OnScriptTemplatesInit()
		{
			overrideUsername.value = TunaTkEditorSettings.OverrideUsername;
			overrideUsername.valueChanged.AddListener(Repaint);
		}

		public void OnScriptTemplatesTab()
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				EditorGUILayout.HelpBox("This is what is placed at the top of any file created through the custom script define system.\n\nThere are a couple of replacement values you can use to customise the header:\n#CREATE_NAME# - The name of the user who created the file\n#CREATE_DATE#- The date the file you are creating was made.\n#CREATE_TIME# - The time the file you are creating was made.\n#CREATE_MACHINE# - The name of the machine the file was made on.\n#SCRIPTNAME# - The name of the script you are making (Unity Built in).\n#SCRIPTNAME_NOEDITOR# - The name of the script you are making minus the term 'Editor'.\n#SCRIPTNAME_NODRAWER# - The name of the script you are making minus the term 'Drawer'.\n#NAMESPACE# - The namespace of the script being made... this is usually the path of the file.", MessageType.Info);
				EditorGUILayout.LabelField("Document Header");
				TunaTkEditorSettings.DocumentHeader = EditorGUILayout.TextArea(TunaTkEditorSettings.DocumentHeader);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				EditorGUILayout.HelpBox("Tick this box to add a custom username to replace the #CREATE_NAME# field of the header... by default (this checkbox is off), the script creation system will use the username of the person logged into the Unity editor.", MessageType.Info);
				TunaTkEditorSettings.OverrideUsername = EditorGUILayout.Toggle("Override Username", TunaTkEditorSettings.OverrideUsername);
				overrideUsername.target = TunaTkEditorSettings.OverrideUsername;

				if(EditorGUILayout.BeginFadeGroup(overrideUsername.faded))
				{
					TunaTkEditorSettings.Username = EditorGUILayout.TextField("Username", TunaTkEditorSettings.Username);
				}

				EditorGUILayout.EndFadeGroup();
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				EditorGUILayout.HelpBox("Use these toggle buttons to disable/enable the words in the namespace path when creating a new file.\n\nDark means disabling, light means enabling.\ni.e. If the 'Module(s)' button is dark, creating a new script in a module(s) folder will automatically remove the word 'Module(s)' from the namespace path.", MessageType.Info);

				EditorGUILayout.BeginHorizontal();
				{
					if(GUILayout.Button("Module(s)", TunaTkEditorSettings.HideModuleNamespace ? GUIStyles.ButtonLeftActive : GUIStyles.ButtonLeftInactive))
					{
						TunaTkEditorSettings.HideModuleNamespace = !TunaTkEditorSettings.HideModuleNamespace;
					}

					if(GUILayout.Button("Editor", TunaTkEditorSettings.HideEditorNamespace ? GUIStyles.ButtonMidActive : GUIStyles.ButtonMidInactive))
					{
						TunaTkEditorSettings.HideEditorNamespace = !TunaTkEditorSettings.HideEditorNamespace;
					}

					if(GUILayout.Button("Plugin(s)", TunaTkEditorSettings.HidePluginsNamespace ? GUIStyles.ButtonRightActive : GUIStyles.ButtonRightInactive))
					{
						TunaTkEditorSettings.HidePluginsNamespace = !TunaTkEditorSettings.HidePluginsNamespace;
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				EditorGUILayout.HelpBox("Add keywords that you would like to remove from namespace paths separated by a comma (No Spaces).\n\nFor Example: 'The,Cow'.", MessageType.Info);
				TunaTkEditorSettings.KeywordsToRemove = EditorGUILayout.TextArea(TunaTkEditorSettings.KeywordsToRemove);
			}
			EditorGUILayout.EndVertical();
		}
	}
}
#endif