// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/14/2018 10:12:31 PM
// Created On: CHRONOS

using System;

using UnityEngine;

namespace TunaTK.Utility
{
	/// <summary>An attribute that can be placed on string types which will allow the string field as a Scene instead. This is useful for LevelManagers.</summary>
	public class SceneAttribute : PropertyAttribute
	{
		/// <summary>Converts the path of the file to the actual scene name.</summary>
		/// <param name="_path">The full path to the asset including the extension and 'Assets/' components.</param>
		public static string LoadableName(string _path)
		{
			const string START = "Assets/";
			const string END = ".unity";

			// If the path contains the end, remove it.
			if(_path.EndsWith(END))
				_path = _path.Substring(0, _path.LastIndexOf(END, StringComparison.Ordinal));

			// If the path contains the start, remove it.
			if(START.StartsWith(START))
				_path = _path.Substring(START.Length);

			return _path;
		}
	}
}