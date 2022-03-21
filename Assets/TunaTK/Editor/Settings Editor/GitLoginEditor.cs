#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 15/01/2020 08:59 PM
// Created On: CHRONOS

using UnityEditor;

using UnityEngine;

namespace TunaTK
{
	public delegate void LoginCallback();

	public class GitLoginEditor : EditorWindow
	{
		private string username = "";
		private string password = "";
		private bool showPassword;

		private LoginCallback callback;

		// Opens the window
		public static void Open(LoginCallback _callback)
		{
			GitLoginEditor window = CreateInstance<GitLoginEditor>();
			window.titleContent = new GUIContent("Git Login");
			window.CenterOnMainWindow(350, 100);
			window.ShowModalUtility();
			window.callback = _callback;
		}

		// Use this for initialization
		private void OnEnable()
		{
			username = ReleaseRepoHelper.Username;
			password = ReleaseRepoHelper.Password;
		}

		// Draw the custom editor window
		private void OnGUI()
		{
			EditorGUI.BeginChangeCheck();

			GUILayout.Label("Username");
			string usernameField = GUILayout.TextField(this.username);

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Password");
				if(GUILayout.Button((showPassword ? "Hide" : "Show") + " Password"))
				{
					showPassword = !showPassword;
				}
			}
			GUILayout.EndHorizontal();

			string passwordField = showPassword ? GUILayout.TextField(password) : GUILayout.PasswordField(password, '*');

			if(EditorGUI.EndChangeCheck())
			{
				username = usernameField;
				password = passwordField;
			}

			EditorGUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Login"))
				{
					ReleaseRepoHelper.SetCredentials(username, password);

					if(callback != null)
					{
						callback();
						callback = null;
					}

					Close();
				}

				if(GUILayout.Button("Cancel"))
					Close();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif