#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 23/07/2018 08:14 PM
// Created On: CHRONOS

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace TunaTK
{
	public static class AssetHelper
	{
		public static Object[] LoadAllAssetsAtPath(string _path, Type _type)
		{
			List<Object> assets = new List<Object>();

			if(Directory.Exists(_path))
			{
				foreach(string file in Directory.GetFiles(_path))
				{
					string assetPath = file.Substring(file.IndexOf("Assets/", StringComparison.Ordinal));

					FileInfo fileInfo = new FileInfo(assetPath);
					if(fileInfo.Extension != ".meta")
					{
						assets.Add(AssetDatabase.LoadAssetAtPath(assetPath, _type));
					}
				}
			}

			return assets.ToArray();
		}

		public static IEnumerable<string> GetAllAssets(string[] _paths)
		{
			List<string> paths = new List<string>();

			foreach(string path in _paths)
			{
				string filePath = Path.Combine(Application.dataPath, path.Replace("Assets/", ""));
				filePath = filePath.Replace("\\", "/");

				FileAttributes fileAttr = File.GetAttributes(filePath);
				if(fileAttr.HasFlag(FileAttributes.Directory))
				{
					IEnumerable<string> internalPaths = GetAllAssetsAtPath(filePath);

					foreach(string newPath in internalPaths)
					{
						string fixedPath = newPath.Replace((char)92, '/');
						paths.Add(fixedPath.Replace(Application.dataPath, "Assets"));
					}
				}
				else
				{
					paths.Add(path);
				}
			}

			return paths.ToArray();
		}

		public static IEnumerable<string> GetAllAssetsAtPath(string _path) => Directory.GetFiles(_path, "*", SearchOption.AllDirectories).Where(_file => !_file.EndsWith(".meta")).ToArray();
	}
}
#endif