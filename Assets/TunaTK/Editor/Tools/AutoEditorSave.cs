#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: Tunacorn Studios
// Creation Time: 20/11/2019 06:10 PM
// Created On: LAPTOP-DENHO8T8

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;

namespace TunaTK
{
	[InitializeOnLoad]
	public class AutoEditorSave
	{
		static AutoEditorSave() => EditorApplication.playModeStateChanged += SaveOnPlay;

		private static void SaveOnPlay(PlayModeStateChange _state)
		{
			if(_state == PlayModeStateChange.ExitingEditMode && TunaTkEditorSettings.PlayModeAutoSave)
			{
				if(TunaTKSettings.UseExtendedLogging)
					Debug.Log("Autosaving....");

				EditorSceneManager.SaveOpenScenes();
				AssetDatabase.SaveAssets();
			}
		}
	}
}
#endif