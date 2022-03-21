// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:32 AM

using Sirenix.OdinInspector;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace TunaTK.AssetManagement
{
	[Serializable]
	public class AssetRegistry
	{
		public IEnumerable<AssetBehaviour> All => assets.Values;

		[ShowInInspector] private readonly Dictionary<string, AssetBehaviour> assets = new Dictionary<string, AssetBehaviour>();

		public bool Register<ASSET>(ASSET _behaviour) where ASSET : AssetBehaviour
		{
			if(!assets.ContainsKey(_behaviour.Guid))
			{
				_behaviour.SendMessage("OnRegister", _behaviour, SendMessageOptions.DontRequireReceiver);
				assets.Add(_behaviour.Guid, _behaviour);
				return true;
			}

			throw new AssetRegistryException($"Behaviour {_behaviour.GetType().Name} on {_behaviour.gameObject.name} already registered in the system.");
		}

		public bool Deregister<ASSET>(ASSET _behaviour) where ASSET : AssetBehaviour
		{
			if(assets.ContainsKey(_behaviour.Guid))
			{
				_behaviour.SendMessage("OnDeregister", _behaviour, SendMessageOptions.DontRequireReceiver);
				assets.Remove(_behaviour.Guid);
				return true;
			}

			throw new AssetRegistryException($"Behaviour {_behaviour.GetType().Name} on {_behaviour.gameObject.name} was never registered in the system.");
		}

		public ASSET FindAsset<ASSET>(string _guid) where ASSET : AssetBehaviour
		{
			assets.TryGetValue(_guid, out AssetBehaviour behaviour);
			return (ASSET)behaviour;
		}
	}
}