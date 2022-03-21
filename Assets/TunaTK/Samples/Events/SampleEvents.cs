// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 10/03/2022 1:26 PM

using System;

using TunaTK.Events;

using UnityEngine;

namespace TunaTK.Samples.Events
{
	public class SampleEvents : MonoBehaviour, IEventHandler
	{
		public bool canRunMessage;
		private void Start()
		{
			EventBus.RegisterObject(this);
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.A) && canRunMessage)
			{
				EventBus.Raise(new MessageEvent()
				{
					message = "Test"
				});
			}
		}

		[SubscribeEvent]
		private void OnMessageEvent(MessageEvent _e)
		{
			Debug.LogError(_e.message);
		}
	}
}