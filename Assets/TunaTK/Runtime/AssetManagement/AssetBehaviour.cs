// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:29 AM

using Sirenix.OdinInspector;

using UnityEngine;

namespace TunaTK.AssetManagement
{
	public class AssetBehaviour : SerializedMonoBehaviour, IAssetBehaviour
	{
		public string Guid
		{
			get => guid;
			set => guid = value;
		}

		[SerializeField, ReadOnly] private string guid; 

		public virtual void Reset() => Guid = RegenerateGuid();

		public string RegenerateGuid() => System.Guid.NewGuid().ToString();
	}
}