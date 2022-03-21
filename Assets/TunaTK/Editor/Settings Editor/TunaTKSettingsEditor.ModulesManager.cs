#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: Tunacorn Studios
// Creation Time: 20/11/2019 07:08 PM
// Created On: LAPTOP-DENHO8T8

using System.Collections.Generic;

using TunaTK.Utilities;

using UnityEditor;

using UnityEngine;

namespace TunaTK
{
	public partial class TunaTkSettingsEditor
	{
		private readonly (string, string, string)[] packages =
		{
			("tunatk", "TunaTK", "com.tunacornstudios.tunatk"),
			("tunatk-characters", "TunaTK - Characters", "com.tunacornstudios.tunatk-characters"),
			("tunatk-developerconsole", "TunaTK - Developer Console", "com.tunacornstudios.tunatk-developerconsole"),
			("tunatk-dialogue", "TunaTK - Dialogue", "com.tunacornstudios.tunatk-dialogue"),
			("tunatk-events", "TunaTK - Events", "com.tunacornstudios.tunatk-events"),
			("tunatk-inventory", "TunaTK - Inventory", "com.tunacornstudios.tunatk-inventory"),
			("tunatk-mirror", "TunaTK - Mirror", "com.tunacornstudios.tunatk-mirror"),
			("tunatk-physics", "TunaTK - Physics", "com.tunacornstudios.tunatk-physics"),
			("tunatk-steamworks", "TunaTK - Steamworks", "com.tunacornstudios.tunatk-steamworks"),
			("tunatk-tabs", "TunaTK - Tabs", "com.tunacornstudios.tunatk-tabs"),
			("tunatk-worldpainter", "TunaTK - Worldpainter", "com.tunacornstudios.tunatk-worldpainter")
		};

		private ManualCountdown autoCheckCountdown;

		// Use this for initialization
		private void OnModuleManagerInit()
		{
			autoCheckCountdown = new ManualCountdown(new TimeInput(30), OnAutoCheckCountdownComplete, true);
			autoCheckCountdown.Start();
		}

		// Draw the custom editor window
		public void OnModuleManagerTab()
		{
			autoCheckCountdown.Update();

			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				Dictionary<string, string> foundPackages = PackageUtils.CheckTunacornPackages();

				foreach((string packageName, string prettyName, string code) in packages)
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField(prettyName);

						if(foundPackages.ContainsKey(code))
						{
							if(GUILayout.Button("Uninstall"))
							{
								PackageUtils.UninstallPackage(code);
							}
						}
						else
						{
							if(GUILayout.Button("Install"))
							{
								PackageUtils.InstallPackage(code, PackageUtils.GetRepoUrl(code));
							}
						}
					}
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
		}

		private void OnAutoCheckCountdownComplete()
		{
			
		}
	}
}
#endif