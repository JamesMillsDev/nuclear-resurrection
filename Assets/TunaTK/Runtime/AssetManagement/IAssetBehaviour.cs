// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:33 AM

namespace TunaTK.AssetManagement
{
	public interface IAssetBehaviour
	{
		public string Guid { get; set; }

		public string RegenerateGuid();
	}
}