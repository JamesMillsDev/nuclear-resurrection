#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 23/11/2019 09:22 AM
// Created On: CHRONOS

using System;
using System.Diagnostics;
using System.IO;

using UnityEngine;

namespace TunaTK
{
	internal static class ReleaseRepoHelper
	{
		internal static string Username { get; private set; } = "";

		internal static string Password { get; private set; } = "";

		internal static void SetCredentials(string _username, string _password)
		{
			Username = _username;
			Password = _password;
		}

		public static void Pull() => RunGitCommand("pull");

		public static void Push(string _commitMessage)
		{
			RunGitCommand("add .");
			RunGitCommand($"commit -m \"{_commitMessage}\"");
			RunGitCommand("push");
		}

		public static void Clone()
		{
			AssignReleaseWorkingDirectory();
			RunGitCommand($@"clone https://{Username}:{Password}@gitlab.com/tunacorn-studios/tunatk-release.git {TunaTkEditorSettings.ReleaseVersionDirectory}", Application.dataPath);
		}

		public static bool IsCloned() => Directory.Exists(TunaTkEditorSettings.ReleaseVersionDirectory);

		public static void AssignReleaseWorkingDirectory() => TunaTkEditorSettings.ReleaseVersionDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "tunatk-release");

		private static void RunGitCommand(string _command, string _overrideWorkingDir = "")
		{
			ProcessStartInfo startInfo = new ProcessStartInfo()
			{
				FileName = "git",
				WorkingDirectory = string.IsNullOrEmpty(_overrideWorkingDir) ? TunaTkEditorSettings.ReleaseVersionDirectory : _overrideWorkingDir,
				Arguments = _command,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};

			Process process = new Process();
			process.StartInfo = startInfo;
			process.Start();
			process.WaitForExit();
		}
	}
}
#endif