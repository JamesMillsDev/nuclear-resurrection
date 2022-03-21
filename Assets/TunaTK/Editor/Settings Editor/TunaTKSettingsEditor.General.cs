#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/22/2018 10:29:08 PM
// Created On: CHRONOS

using UnityEditor;

namespace TunaTK
{
    public partial class TunaTkSettingsEditor
    {
        private void OnGeneralInit()
        {

        }

        public void OnGeneralTab()
        {
            TunaTKSettings.EnableDeveloperMode = EditorGUILayout.Toggle("Enable Developer Mode", TunaTKSettings.EnableDeveloperMode);
            TunaTKSettings.UseExtendedLogging = EditorGUILayout.Toggle("Use Extended Logging", TunaTKSettings.UseExtendedLogging);
            TunaTkEditorSettings.PlayModeAutoSave = EditorGUILayout.Toggle("Auto Save On Play", TunaTkEditorSettings.PlayModeAutoSave);
        }
    }
}
#endif