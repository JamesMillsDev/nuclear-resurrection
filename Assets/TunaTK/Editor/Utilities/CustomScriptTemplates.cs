#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 13/11/2018 10:00 PM
// Created On: CHRONOS

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using Unity.EditorCoroutines.Editor;

using UnityEditor;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#pragma warning disable CS0618

namespace TunaTK
{
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
	public sealed class CustomScriptTemplates : AssetModificationProcessor
	{
		[MenuItem("Assets/Create/TunaTK/Behaviours/MonoBehaviour Script", priority = 0)]
		[MenuItem("TunaTK/Scripts/Behaviours/MonoBehaviour Script", priority = 0)]
		public static void CreateBehaviourScript() => CreateScriptAsset("TunaTK/Script Templates/Behaviours/ScriptTemplate", "NewBehaviourScript");

		[MenuItem("Assets/Create/TunaTK/Behaviours/Singleton Script", priority = 1)]
		[MenuItem("TunaTK/Scripts/Behaviours/Singleton Script", priority = 1)]
		public static void CreateBehaviourSingleton() => CreateScriptAsset("TunaTK/Script Templates/Behaviours/SingletonTemplate", "NewBehaviourSingleton");

		[MenuItem("Assets/Create/TunaTK/Behaviours/Scriptable Object", priority = 2)]
		[MenuItem("TunaTK/Scripts/Behaviours/Scriptable Object", priority = 2)]
		public static void CreateScriptableObject() => CreateScriptAsset("TunaTK/Script Templates/Behaviours/ScriptableObjectTemplate", "NewScriptableObject");

		[MenuItem("Assets/Create/TunaTK/C# Standard/Class", priority = 0)]
		[MenuItem("TunaTK/Scripts/C# Standard/Class", priority = 0)]
		public static void CreateClass() => CreateScriptAsset("TunaTK/Script Templates/Standard/ClassTemplate", "NewClass");

		[MenuItem("Assets/Create/TunaTK/C# Standard/Serialized Class", priority = 1)]
		[MenuItem("TunaTK/Scripts/C# Standard/Serialized Class", priority = 1)]
		public static void CreateSerializedClass() => CreateScriptAsset("TunaTK/Script Templates/Standard/SerializedClassTemplate", "NewSerializedClass");

		[MenuItem("Assets/Create/TunaTK/C# Standard/Struct", priority = 2)]
		[MenuItem("TunaTK/Scripts/C# Standard/Struct", priority = 2)]
		public static void CreateStruct() => CreateScriptAsset("TunaTK/Script Templates/Standard/StructTemplate", "NewStruct");

		[MenuItem("Assets/Create/TunaTK/C# Standard/Serialized Struct", priority = 3)]
		[MenuItem("TunaTK/Scripts/C# Standard/Serialized Struct", priority = 3)]
		public static void CreateSerializedStruct() => CreateScriptAsset("TunaTK/Script Templates/Standard/SerializedStructTemplate", "NewSerializedStruct");

		[MenuItem("Assets/Create/TunaTK/C# Standard/Enum", priority = 4)]
		[MenuItem("TunaTK/Scripts/C# Standard/Enum", priority = 4)]
		public static void CreateEnum() => CreateScriptAsset("TunaTK/Script Templates/Standard/EnumTemplate", "NewEnum");

		[MenuItem("Assets/Create/TunaTK/C# Standard/Interface", priority = 5)]
		[MenuItem("TunaTK/Scripts/C# Standard/Interface", priority = 5)]
		public static void CreateInterface() => CreateScriptAsset("TunaTK/Script Templates/Standard/InterfaceTemplate", "INewInterface");

		[MenuItem("Assets/Create/TunaTK/C# Standard/Attribute", priority = 6)]
		[MenuItem("TunaTK/Scripts/C# Standard/Attribute", priority = 6)]
		public static void CreateAttribute() => CreateScriptAsset("TunaTK/Script Templates/Standard/AttributeTemplate", "NewAttribute");

		[MenuItem("Assets/Create/TunaTK/C# Standard/Exception", priority = 7)]
		[MenuItem("TunaTK/Scripts/C# Standard/Exception", priority = 7)]
		public static void CreateException() => CreateScriptAsset("TunaTK/Script Templates/Standard/ExceptionTemplate", "NewException");

		[MenuItem("Assets/Create/TunaTK/Initialisables/Initialisable", priority = 0)]
		[MenuItem("TunaTK/Scripts/Initialisables/Initialisable", priority = 0)]
		public static void CreateInitialisableScript() => CreateScriptAsset("TunaTK/Script Templates/Initialisables/ScriptTemplate", "NewInitialisableScript");

		[MenuItem("Assets/Create/TunaTK/Initialisables/Singleton", priority = 1)]
		[MenuItem("TunaTK/Scripts/Initialisables/Singleton", priority = 1)]
		public static void CreateInitialisableSingleton() => CreateScriptAsset("TunaTK/Script Templates/Initialisables/SingletonTemplate", "NewInitialisableSingleton");

		[MenuItem("Assets/Create/TunaTK/Initialisables/State Machine Behaviour", priority = 2)]
		[MenuItem("TunaTK/Scripts/Initialisables/State Machine Behaviour", priority = 2)]
		public static void CreateStateMachineBehaviourScript() => CreateScriptAsset("TunaTK/Script Templates/Initialisables/StateMachineBehaviourTemplate", "NewStateMachineBehaviour");

		[MenuItem("Assets/Create/TunaTK/Editor/Custom Inspector", priority = 0)]
		[MenuItem("TunaTK/Scripts/Editor/Custom Inspector", priority = 0)]
		public static void CreateEditorScript() => CreateScriptAsset("TunaTK/Script Templates/Editors/CustomInspectorTemplate", "NewCustomInspector");

		[MenuItem("Assets/Create/TunaTK/Editor/Property Drawer", priority = 1)]
		[MenuItem("TunaTK/Scripts/Editor/Property Drawer", priority = 1)]
		public static void CreatePropertyDrawer() => CreateScriptAsset("TunaTK/Script Templates/Editors/PropertyDrawerTemplate", "NewPropertyDrawer");

		[MenuItem("Assets/Create/TunaTK/Editor/Editor Window", priority = 2)]
		[MenuItem("TunaTK/Scripts/Editor/Editor Window", priority = 2)]
		public static void CreateEditorWindow() => CreateScriptAsset("TunaTK/Script Templates/Editors/EditorWindowTemplate", "NewEditorWindow");

		public static void CreateScriptAsset(string _asset, string _fileName) => EditorCoroutineUtility.StartCoroutine(CreateScriptAsset_CR(_asset, _fileName), EditorWindow.GetWindow<TunaTkSettingsEditor>());

		private static IEnumerator CreateScriptAsset_CR(string _asset, string _filename)
		{
			AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(_asset);
			
			yield return handle;
			
			ProjectWindowUtil.CreateScriptAssetFromTemplateFile(AssetDatabase.GetAssetPath(handle.Result), _filename + ".cs");
		}

		public static void OnWillCreateAsset(string _path)
		{
			_path = _path.Replace(".meta", "");
			int index = _path.LastIndexOf(".", StringComparison.Ordinal);
			if(index < 0)
				return;

			string file = _path.Substring(index);
			if(file != ".cs" && file != ".js")
				return;

			index = Application.dataPath.LastIndexOf("Assets", StringComparison.Ordinal);
			_path = Application.dataPath.Substring(0, index) + _path;
			if(!File.Exists(_path))
				return;

			string fileContent = File.ReadAllText(_path);

			fileContent = fileContent.Replace("#HEADER#", TunaTkEditorSettings.DocumentHeader);
			fileContent = fileContent.Replace("#CREATE_NAME#", TunaTkEditorSettings.OverrideUsername ? TunaTkEditorSettings.Username : GetDisplayname());
			fileContent = fileContent.Replace("#CREATE_DATE#", DateTime.Now.ToString("dd/MM/yyyy"));
			fileContent = fileContent.Replace("#CREATE_TIME#", DateTime.Now.ToString("hh:mm tt"));
			fileContent = fileContent.Replace("#CREATE_MACHINE#", Environment.MachineName);
			fileContent = fileContent.Replace("#NAMESPACE#", GetNamespaceFromPath(_path));
			fileContent = fileContent.Replace("#SCRIPTNAME_NOEDITOR#", GetScriptnameWithout(_path, "Editor"));
			fileContent = fileContent.Replace("#SCRIPTNAME_NODRAWER#", GetScriptnameWithout(_path, "Drawer"));

			File.WriteAllText(_path, fileContent);
			AssetDatabase.Refresh();
		}

		private static string GetDisplayname()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(EditorWindow));
			object uc = assembly.CreateInstance("UnityEditor.Connect.UnityConnect", false, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null, null);
			// Cache type of UnityConnect.
			if(uc != null)
			{
				Type t = uc.GetType();
				// Get user info object from UnityConnect.
				object userInfo = t.GetProperty("userInfo")?.GetValue(uc, null);
				// Retrieve user id from user info.
				if(userInfo != null)
				{
					Type userInfoType = userInfo.GetType();

					return userInfoType.GetProperty("displayName")?.GetValue(userInfo, null) as string;
				}
			}

			return "";
		}

		private static string GetSafeNamespace(string _namespace)
		{
			string safeNamespace = _namespace;
			safeNamespace = RemoveCharacter(safeNamespace, ' ');
			safeNamespace = RemoveCharacter(safeNamespace, '-');

			return safeNamespace;
		}

		private static string RemoveCharacter(string _stringToChange, char _charToRemove)
		{
			string toChange = _stringToChange;
			int index = toChange.IndexOf(_charToRemove);

			while(index != -1)
			{
				toChange = toChange.Remove(index, 1);
				index = toChange.IndexOf(_charToRemove);
			}

			return toChange;
		}

		private static string GetNamespaceFromPath(string _path)
		{
			int index = _path.LastIndexOf("Assets", StringComparison.Ordinal) + ("Assets").Length;
			string localPath = _path.Substring(index, _path.Length - index);

			string newNamespace = localPath.Replace('/', '.');
			newNamespace = newNamespace.Replace("Scripts.", "");
			newNamespace = newNamespace.Replace("Code.", "");

			if(TunaTkEditorSettings.HideEditorNamespace)
				newNamespace = newNamespace.Replace("Editor.", "");

			if(TunaTkEditorSettings.HideModuleNamespace)
			{
				newNamespace = newNamespace.Replace("Module.", "");
				newNamespace = newNamespace.Replace("Modules.", "");
			}

			if(TunaTkEditorSettings.HidePluginsNamespace)
			{
				newNamespace = newNamespace.Replace("Plugin.", "");
				newNamespace = newNamespace.Replace("Plugins.", "");
			}

			string[] keywords = TunaTkEditorSettings.KeywordsToRemove.Split(',');
			foreach(string keyword in keywords)
			{
				if(keyword != "")
				{
					newNamespace = newNamespace.Replace(keyword, "");
				}
			}

			while(newNamespace[0] == '.')
			{
				newNamespace = newNamespace.Substring(1);
			}

			newNamespace = newNamespace.Substring(0, newNamespace.Length - 3);
			newNamespace = newNamespace.IndexOf('.') != -1 ? newNamespace.Substring(0, newNamespace.LastIndexOf('.')) : Application.productName;

			return GetSafeNamespace(newNamespace);
		}

		private static string GetScriptnameWithout(string _path, string _toRemove)
		{
			FileInfo file = new FileInfo(_path);
			string fileName = file.Name.Replace(".cs", "");

			return fileName.Replace(_toRemove, "");
		}
	}
}
#endif