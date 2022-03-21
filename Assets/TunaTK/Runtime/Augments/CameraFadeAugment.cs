// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 05/03/2022 9:35 PM

using System;
using System.Threading;
using System.Threading.Tasks;

using TunaTK.Augments;
using TunaTK.Utility;

using UnityAsync;

using UnityEngine;

namespace TunaTK.Augments
{
	[RequireComponent(typeof(CanvasGroup))]
	public class CameraFadeAugment<USER> : Augment<USER> where USER : User<USER>
	{
		private static CameraFadeAugment<USER> instance;

		private CanvasGroup canvasGroup;

		[SerializeField] protected float duration;

		public static void FadeDown(Action _callback = null) => instance.Fade_Async(1, _callback).Process();

		public static void FadeUp(Action _callback = null) => instance.Fade_Async(0, _callback).Process();

		protected override Task OnInitialisation(USER _user, object[] _params)
		{
			if(instance == null)
			{
				instance = this;
			}
			else if(instance != this)
			{
				_user.RemoveAugment(this);
				return Task.FromCanceled(CancellationToken.None);
			}
			
			canvasGroup = gameObject.GetComponent<CanvasGroup>();
			
			return base.OnInitialisation(_user, _params);
		}

		private async Task Fade_Async(float _value, Action _callback)
		{
			float time = 0;
			float initialAlpha = canvasGroup.alpha;

			while(time < duration)
			{
				canvasGroup.alpha = Mathf.Lerp(initialAlpha, _value, Mathf.Clamp01(time / duration));
				
				await new WaitForEndOfFrame();

				time += Time.deltaTime;
			}

			canvasGroup.alpha = _value;
			_callback?.Invoke();
		}
	}
}