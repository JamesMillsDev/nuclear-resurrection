// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: Tunacorn Studios
// Creation Time: 05/11/2019 01:31 PM
// Created On: LAPTOP-DENHO8T8

using TunaTK.Utility;

using UnityEngine;

namespace TunaTK.Utilities
{
	public class MaterialTweener : BaseTweener
	{
		private readonly Renderer renderer;

		private readonly Material[] materials;
		private int materialIndex;

		public MaterialTweener(Renderer _renderer, params Material[] _materials)
		{
			renderer = _renderer;
			materials = _materials;
		}

		public override void Initialise(TimeInput _time, MonoBehaviour _runner, TweenMode _mode, AnimationCurve _curve, Callback _callback = null)
		{
			if(_mode == TweenMode.PingPong)
				_mode = TweenMode.Looped;

			base.Initialise(_time, _runner, _mode, _curve, _callback);
		}

		public override void Tween(float _factor)
		{
			Material start = materials[materialIndex];
			Material end = materialIndex == materials.Length - 1 ? materials[0] : materials[materialIndex + 1];

			renderer.material.Lerp(start, end, _factor);
		}

		protected override void OnTweenComplete()
		{
			if(mode == TweenMode.OneShot)
			{
				materialIndex++;
				if(materialIndex < materials.Length)
					Start();
			}
			else
			{
				SharedMethods.IncrementAndWrap(ref materialIndex, materials.Length);
			}
		}
	}
}