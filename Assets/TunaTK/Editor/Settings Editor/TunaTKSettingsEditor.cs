#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/22/2018 10:13:33 PM
// Created On: CHRONOS

using System.Collections;

using Unity.EditorCoroutines.Editor;

using UnityEngine;
using UnityEditor;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TunaTK
{
    public partial class TunaTkSettingsEditor : EditorWindow, IHasCustomMenu
    {
        private Vector2 scrollPosition = Vector2.zero;
        private Texture logo;
        private Texture banner;

        private static Color defaultGUIColor = Color.clear;
        private static Color defaultBackgroundColor = Color.clear;
        private static Color defaultContentColor = Color.clear;

        private readonly string[] tabNames = { "General", "Script Templates", "Module Manager" };

        [MenuItem("TunaTK/Settings Manager", priority = 100)]
        public static void Init()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(TunaTkSettingsEditor));
            window.maxSize = new Vector2(385, 1920);
            window.minSize = new Vector2(window.maxSize.x, 300);
        }

        public void AddItemsToMenu(GenericMenu _menu)
        {
            _menu.AddItem(new GUIContent("Rebuild GUI Styles"), false, RebuildGUIStyles);
            _menu.AddItem(new GUIContent("Rebuild GUI Colors"), false, RebuildGUIColors);
        }

        private void OnEnable()
        {
            EditorCoroutineUtility.StartCoroutine(LoadLogo(), this);
            EditorCoroutineUtility.StartCoroutine(LoadBanner(), this);
            
            OnGeneralInit();
            OnScriptTemplatesInit();
            OnModuleManagerInit();
        }

        private IEnumerator LoadLogo()
        {
            AsyncOperationHandle<Texture> handle = Addressables.LoadAssetAsync<Texture>("TunaTK/Logo");

            yield return handle;

            logo = handle.Result;
            titleContent = new GUIContent("TunaTK Manager", logo);
        }

        private IEnumerator LoadBanner()
        {
            AsyncOperationHandle<Texture> handle = Addressables.LoadAssetAsync<Texture>("TunaTK/TunaTK-Banner");

            yield return handle;

            banner = handle.Result;
        }

        private void OnGUI()
        {
            SetupGUIColors();
            EditorCoroutineUtility.StartCoroutine(GUIStyles.Rebuild(), this);

            EditorHelper.DrawBanner(tabNames[TunaTkEditorSettings.SelectedTab], TunaTkEditorSettings.SelectedTab == 0 ? 40 : 25, banner);

            GUILayout.BeginArea(new Rect(0, 100, position.width, position.height - 100));
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                {
                    SetWindowMinMax(400, 300, 1920);

                    TunaTkEditorSettings.SelectedTab = GUILayout.Toolbar(TunaTkEditorSettings.SelectedTab, tabNames);

                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        switch (TunaTkEditorSettings.SelectedTab)
                        {
                            case 0:
                                OnGeneralTab();
                                break;
                            case 1:
                                OnScriptTemplatesTab();
                                break;
                            case 2:
                                OnModuleManagerTab();
                                break;
                            default:
                                break;
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void SetWindowMinMax(int _minWidth, int _minHeight, int aMaxHeight)
        {
            minSize = new Vector2(_minWidth, _minHeight);
            maxSize = new Vector2(_minWidth, aMaxHeight);
        }

        private void RebuildGUIStyles()
        {
            GUIStyles.Rebuild();
        }

        private void RebuildGUIColors()
        {
            defaultGUIColor = Color.clear;
            defaultBackgroundColor = Color.clear;
            defaultContentColor = Color.clear;
        }

        private void SetupGUIColors()
        {
            if (defaultGUIColor == Color.clear)
            {
                defaultGUIColor = GUI.color;
            }

            if (defaultBackgroundColor == Color.clear)
            {
                defaultBackgroundColor = GUI.backgroundColor; 
            }

            if (defaultContentColor == Color.clear)
            {
                defaultContentColor = GUI.contentColor;
            }
        }
    }
}
#endif