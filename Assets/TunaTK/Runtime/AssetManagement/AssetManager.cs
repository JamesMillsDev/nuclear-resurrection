// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:32 AM

using Sirenix.OdinInspector;

using System.Linq;

using UnityEngine;

namespace TunaTK.AssetManagement
{
	[HideMonoScript]
	public class AssetManager : Singleton<AssetManager>
	{
		[ShowInInspector, ShowIf("showRegistry"), BoxGroup("Registry")]
		private AssetRegistry Registry { get; set; }

		[SerializeField, BoxGroup("General", false)]
		private bool autoRegisterScene;

		[SerializeField, BoxGroup("Registry"), LabelText("Show")]
		// ReSharper disable once NotAccessedField.Local
		private bool showRegistry;

		#region Asset Registry

		public static bool Register<ASSET>(ASSET _behaviour) where ASSET : AssetBehaviour => Instance.Registry.Register(_behaviour);

		public static bool Deregister<ASSET>(ASSET _behaviour) where ASSET : AssetBehaviour => Instance.Registry.Deregister(_behaviour);

		public static ASSET FindAsset<ASSET>(string _guid) where ASSET : AssetBehaviour => Instance.Registry.FindAsset<ASSET>(_guid);

		#endregion

		private void Awake()
		{
			CreateSingletonInstance();
			Registry = new AssetRegistry();

			if(autoRegisterScene)
			{
				foreach(AssetBehaviour behaviour in FindObjectsOfType<AssetBehaviour>().Where(_behaviour => _behaviour != this))
					Registry.Register(behaviour);
			}
		}

		private void OnDestroy()
		{
			instance = null;
			Registry = null;
		}
	}
}