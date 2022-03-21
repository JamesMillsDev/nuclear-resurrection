#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: Tunacorn Studios
// Creation Time: 20/11/2019 06:58 PM
// Created On: LAPTOP-DENHO8T8

using Serializable = System.SerializableAttribute;

namespace TunaTK
{
	[Serializable]
	public class PackageVersionInfo
	{
		public string id;
		public string version;
		public string[] dependencies;
		public string[] contentPaths;

		public string PackagePath { get; set; }

		public PackageStatus Status { get; set; }

		public PackageVersionInfo(string _id, string _version)
		{
			id = _id;
			version = _version;
		}
	}
}
#endif