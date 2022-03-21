#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 12/12/2019 11:32 AM
// Created On: LAPTOP-DENHO8T8

using UnityEngine;
using UnityEditor;

using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace TunaTK
{
    public delegate void ModuleExportCallback();

	public class ModuleExporterEditor : EditorWindow 
	{
        private ModuleExportCallback callback;

        private string changeLog = "";

        private int versionIndex = 0;

        private PackageVersionInfo versionInfo;

        private List<PackageVersionInfo> versions = new List<PackageVersionInfo>();
        private List<int> versionsIndexes = new List<int>();

        private static Vector2 size = new Vector2(400, 185);

		// Opens the window
        public static void Open(ModuleExportCallback _callback)
        {
            ModuleExporterEditor window = ScriptableObject.CreateInstance<ModuleExporterEditor>();
            window.titleContent = new GUIContent("Module Exporter");
            window.CenterOnMainWindow(size.x, size.y);

            window.callback = _callback;
            window.ShowModalUtility();
        }

        private void OnEnable()
        {
            LoadVersionInfos();

            versionIndex = versions.IndexOf(versions.Where(version => version.id == "Core").FirstOrDefault());

            IncrementVersionString();
        }

        // Draw the custom editor window
        private void OnGUI()
		{
            minSize = size;
            maxSize = minSize;

            EditorGUILayout.LabelField("Please enter the name of the module you wish to export.");

            EditorGUI.BeginChangeCheck();
            versionIndex = EditorGUILayout.IntPopup("Module To Export", versionIndex, versions.Select(version => version.id).ToArray(), versionsIndexes.ToArray());
            versionInfo = versions[versionIndex];
            
            if (EditorGUI.EndChangeCheck())
            {
                IncrementVersionString();
            }

            EditorGUI.BeginChangeCheck();
            versionInfo.version = EditorGUILayout.TextField("Version", versionInfo.version);

            if (EditorGUI.EndChangeCheck())
            {
                UpdateVersionInfoFile();
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Change Log");
            changeLog = EditorGUILayout.TextArea(changeLog, GUILayout.Height(EditorGUIUtility.singleLineHeight * 4));

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Export Module"))
                {
                    string safeVersionID = versionInfo.id.Replace(" ", "");
                    string lowerVersionID = safeVersionID.ToLower();

                    TextAsset asset = Resources.Load<TextAsset>($@"TunaTK\Version Information\{lowerVersionID}");
                    PackageVersionInfo info = JsonUtility.FromJson<PackageVersionInfo>(asset.text);

                    IEnumerable<string> paths = AssetHelper.GetAllAssets(info.contentPaths);
                    List<string> pathList = new List<string>(paths);
                    pathList.Add(AssetDatabase.GetAssetPath(asset));

                    AssetDatabase.ExportPackage(pathList.ToArray(), Path.Combine(TunaTkEditorSettings.ReleaseVersionDirectory, $@"packages\TunaTK.{safeVersionID}.unitypackage"));
                    File.WriteAllText(Path.Combine(TunaTkEditorSettings.ReleaseVersionDirectory, $@"versions\{lowerVersionID}.json"), JsonUtility.ToJson(versionInfo));
                    ReleaseRepoHelper.Push($"Updated {versionInfo.id} Module to version {versionInfo.version}.\n\n{changeLog}");

                    if (callback != null)
                    {
                        callback();
                    }

                    Close();
                }

                if (GUILayout.Button("Cancel"))
                {
                    Close();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void IncrementVersionString()
        {
            if (versionInfo == null)
            {
                versionInfo = versions[versionIndex];
            }
            string[] splitVersion = versionInfo.version.Split('.');
            int major = int.Parse(splitVersion[0]);
            int minor = int.Parse(splitVersion[1]);
            int patch = int.Parse(splitVersion[2]);

            patch++;

            versionInfo.version = string.Join(".", major, minor, patch);

            UpdateVersionInfoFile();
        }

        private void LoadVersionInfos()
        {
            TextAsset[] assets = Resources.LoadAll<TextAsset>(@"TunaTK\Version Information");
            versions.Clear();
            versionsIndexes.Clear();

            int index = 0;
            foreach (var asset in assets)
            {
                versions.Add(JsonUtility.FromJson<PackageVersionInfo>(asset.text));
                versionsIndexes.Add(index++);
            }
        }

        private void UpdateVersionInfoFile()
        {
            string safeVersionID = versionInfo.id.Replace(" ", "");
            string lowerVersionID = safeVersionID.ToLower();

            TextAsset asset = Resources.Load<TextAsset>($@"TunaTK\Version Information\{lowerVersionID}");
            string path = AssetDatabase.GetAssetPath(asset);
            File.WriteAllText(path, JsonUtility.ToJson(versionInfo));
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }
	}
}
#endif