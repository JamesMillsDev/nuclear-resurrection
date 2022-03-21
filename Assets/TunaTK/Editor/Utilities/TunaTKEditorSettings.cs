#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/22/2018 10:09:03 PM
// Created On: CHRONOS

using UnityEditor;

using UnityEngine;

namespace TunaTK
{
	public static class TunaTkEditorSettings
	{
		private static string GetKeyWithProjectID(string _key)
		{
			string[] splitPath = Application.dataPath.Split('/');
			return splitPath[splitPath.Length - 2] + "." + _key;
		}

	#region Setting Keys

		private const string LIBRARY_KEY = "TunaTK";

		// ------------- TUNATK SETTINGS MANAGER ------------- //
		private const string SETTINGS_MENU_MODULE_KEY = "SettingsMenu";
		private const string SELECTED_TAB_KEY = LIBRARY_KEY + "." + SETTINGS_MENU_MODULE_KEY + ".SelectedTab";

		private const string PLAY_MODE_AUTO_SAVE_KEY = LIBRARY_KEY + "." + SETTINGS_MENU_MODULE_KEY + ".PlayModeAutoSave";
		// ------------- TUNATK SETTINGS MANAGER ------------- //

		// ------------- SCRIPT TEMPLATE SYSTEM ------------- //
		private const string SCRIPT_TEMPLATE_SETTINGS_MODULE_KEY = "ScriptTemplates";
		private const string DOCUMENT_HEADER_KEY = LIBRARY_KEY + "." + SCRIPT_TEMPLATE_SETTINGS_MODULE_KEY + ".DocumentHeader";
		private const string OVERRIDE_USERNAME_KEY = LIBRARY_KEY + "." + SCRIPT_TEMPLATE_SETTINGS_MODULE_KEY + ".OverrideUsername";
		private const string USERNAME_KEY = LIBRARY_KEY + "." + SCRIPT_TEMPLATE_SETTINGS_MODULE_KEY + ".Username";
		private const string HIDE_MODULE_NAMESPACE_KEY = LIBRARY_KEY + "." + SCRIPT_TEMPLATE_SETTINGS_MODULE_KEY + ".HideModuleNamespace";
		private const string HIDE_EDITOR_NAMESPACE_KEY = LIBRARY_KEY + "." + SCRIPT_TEMPLATE_SETTINGS_MODULE_KEY + ".HideEditorNamespace";
		private const string HIDE_PLUGINS_NAMESPACE_KEY = LIBRARY_KEY + "." + SCRIPT_TEMPLATE_SETTINGS_MODULE_KEY + ".HidePluginsNamespace";

		private const string KEYWORDS_TO_REMOVE_KEY = LIBRARY_KEY + "." + SCRIPT_TEMPLATE_SETTINGS_MODULE_KEY + ".KeywordsToRemove";
		// ------------- SCRIPT TEMPLATE SYSTEM ------------- //

		// ------------- MODULE MANAGER ------------- //
		private const string MODULE_MANAGER_MODULE_KEY = "ModuleManager";
		private const string RELEASE_VERSION_DIRECTORY_KEY = LIBRARY_KEY + "." + MODULE_MANAGER_MODULE_KEY + ".ReleaseDirectory";

		private const string AUTO_CHECK_UPDATES_KEY = LIBRARY_KEY + "." + MODULE_MANAGER_MODULE_KEY + ".AutoCheckUpdates";
		// ------------- MODULE MANAGER ------------- //

	#endregion

	#region Settings

		// ------------- TUNATK SETTINGS MANAGER ------------- //
		public static int SelectedTab
		{
			get => EditorPrefs.GetInt(GetKeyWithProjectID(SELECTED_TAB_KEY), 0);

			set => EditorPrefs.SetInt(GetKeyWithProjectID(SELECTED_TAB_KEY), value);
		}

		public static bool PlayModeAutoSave
		{
			get => EditorPrefs.GetBool(GetKeyWithProjectID(PLAY_MODE_AUTO_SAVE_KEY), true);

			set => EditorPrefs.SetBool(GetKeyWithProjectID(PLAY_MODE_AUTO_SAVE_KEY), value);
		}
		// ------------- TUNATK SETTINGS MANAGER ------------- //

		// ------------- SCRIPT TEMPLATE SYSTEM ------------- //
		public static string DocumentHeader
		{
			get => EditorPrefs.GetString(GetKeyWithProjectID(DOCUMENT_HEADER_KEY), "// Property of TUNACORN STUDIOS PTY LTD 2018\n//\n// Creator: #CREATE_NAME#\n// Creation Time: #CREATE_DATE# #CREATE_TIME#\n// Created On: #CREATE_MACHINE#");

			set => EditorPrefs.SetString(GetKeyWithProjectID(DOCUMENT_HEADER_KEY), value);
		}

		public static bool OverrideUsername
		{
			get => EditorPrefs.GetBool(GetKeyWithProjectID(OVERRIDE_USERNAME_KEY), false);

			set => EditorPrefs.SetBool(GetKeyWithProjectID(OVERRIDE_USERNAME_KEY), value);
		}

		public static string Username
		{
			get => EditorPrefs.GetString(GetKeyWithProjectID(USERNAME_KEY), "");

			set => EditorPrefs.SetString(GetKeyWithProjectID(USERNAME_KEY), value);
		}

		public static bool HideModuleNamespace
		{
			get => EditorPrefs.GetBool(GetKeyWithProjectID(HIDE_MODULE_NAMESPACE_KEY), true);

			set => EditorPrefs.SetBool(GetKeyWithProjectID(HIDE_MODULE_NAMESPACE_KEY), value);
		}

		public static bool HideEditorNamespace
		{
			get => EditorPrefs.GetBool(GetKeyWithProjectID(HIDE_EDITOR_NAMESPACE_KEY), true);

			set => EditorPrefs.SetBool(GetKeyWithProjectID(HIDE_EDITOR_NAMESPACE_KEY), value);
		}

		public static bool HidePluginsNamespace
		{
			get => EditorPrefs.GetBool(GetKeyWithProjectID(HIDE_PLUGINS_NAMESPACE_KEY), true);

			set => EditorPrefs.SetBool(GetKeyWithProjectID(HIDE_PLUGINS_NAMESPACE_KEY), value);
		}

		public static string KeywordsToRemove
		{
			get => EditorPrefs.GetString(GetKeyWithProjectID(KEYWORDS_TO_REMOVE_KEY), "");

			set => EditorPrefs.SetString(GetKeyWithProjectID(KEYWORDS_TO_REMOVE_KEY), value);
		}
		// ------------- SCRIPT TEMPLATE SYSTEM ------------- //

		// ------------- MODULE MANAGER ------------- //
		public static string ReleaseVersionDirectory
		{
			get => EditorPrefs.GetString(GetKeyWithProjectID(RELEASE_VERSION_DIRECTORY_KEY), "");

			set => EditorPrefs.SetString(GetKeyWithProjectID(RELEASE_VERSION_DIRECTORY_KEY), value);
		}

		public static bool AutoCheckUpdates
		{
			get => EditorPrefs.GetBool(GetKeyWithProjectID(AUTO_CHECK_UPDATES_KEY), false);

			set => EditorPrefs.SetBool(GetKeyWithProjectID(AUTO_CHECK_UPDATES_KEY), value);
		}
		// ------------- MODULE MANAGER ------------- //

	#endregion
	}
}
#endif