// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:33 AM

using System;

namespace TunaTK.AssetManagement
{
	public class AssetRegistryException : SystemException
	{
		public AssetRegistryException(string _message) : base(_message) { }
	}
}