// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 30/10/2019 09:19 AM
// Created On: CHRONOS

using TunaTK.Utility;

using UnityEngine;

using Serializable = System.SerializableAttribute;

namespace TunaTK.Utilities
{
	public class TransformTweener : BaseTweener
	{
		[Serializable]
		public struct TransformTweenSettings
		{
			/// <summary></summary>
			public bool tweenPosition;

			/// <summary></summary>
			public bool tweenRotation;

			/// <summary></summary>
			public bool tweenScale;

			/// <summary></summary>
			public bool world;

			/// <param name="_position"></param>
			/// <param name="_rotation"></param>
			/// <param name="_scale"></param>
			/// <param name="_world"></param>
			public TransformTweenSettings(bool _position, bool _rotation, bool _scale, bool _world)
			{
				tweenPosition = _position;
				tweenRotation = _rotation;
				tweenScale = _scale;
				world = _world;
			}
		}

		//@cond
		private readonly Transform controlled;
		private readonly Transform[] transforms;
		private readonly TransformTweenSettings settings;

		private int index;
		//@endcond

		/// <param name="_controlled">The transform that is being controlled.</param>
		/// <param name="_transforms">The list of transforms that the controlled one is tweening between.</param>
		/// <param name="_settings">The settings that we are using to define the behaviour of the tweening.</param>
		public TransformTweener(Transform _controlled, Transform[] _transforms, TransformTweenSettings _settings)
		{
			controlled = _controlled;
			transforms = _transforms;
			settings = _settings;
		}

		/// <summary>The function that completes the setup phase of the Tween Runner, this won't actually start the tweening.</summary>
		/// <param name="_time">How long a single step of the tween will take.</param>
		/// <param name="_runner">The monobehaviour running the tweener. This is used for the coroutine.</param>
		/// <param name="_mode">The mode we will tween in.<see cref="TweenMode"/>.</param>
		/// <param name="_curve">The curve that the tweening will take. This allows for more complex tweening behaviours like easing, or linear tweening.</param>
		/// <param name="_callback">The function that will run when a single frame of the tween is complete.</param>
		public override void Initialise(TimeInput _time, MonoBehaviour _runner, TweenMode _mode, AnimationCurve _curve, Callback _callback = null)
		{
			if(_mode == TweenMode.PingPong)
				_mode = TweenMode.Looped;

			base.Initialise(_time, _runner, _mode, _curve, _callback);
		}

		/// <summary>This is the actual handling of the tweening. In this case it's updating transforms.</summary>
		/// <param name="_factor">The amount we need to tween based on the current time.</param>
		public override void Tween(float _factor)
		{
			// Calculate the start and end points based on the current index. 
			Transform start = transforms[index];
			Transform end = index == transforms.Length - 1 ? transforms[0] : transforms[index + 1];

			// Determine whether or not we should be doing this in world space or local space
			if(settings.world)
			{
				// If we need to tween the position, do so.
				if(settings.tweenPosition)
					controlled.position = Vector3.Lerp(start.position, end.position, _factor);

				// If we need to tween the rotation, do so.
				if(settings.tweenRotation)
					controlled.rotation = Quaternion.Lerp(start.rotation, end.rotation, _factor);
			}
			else
			{
				// If we need to tween the position, do so.
				if(settings.tweenPosition)
					controlled.localPosition = Vector3.Lerp(start.localPosition, end.localPosition, _factor);

				// If we need to tween the rotation, do so.
				if(settings.tweenRotation)
					controlled.localRotation = Quaternion.Lerp(start.localRotation, end.localRotation, _factor);
			}

			// If we need to tween the scale, do so.
			if(settings.tweenScale)
				controlled.localScale = Vector3.Lerp(start.localScale, end.localScale, _factor);
		}

		/// <summary>The function that runs when the Tween is complete.</summary>
		protected override void OnTweenComplete()
		{
			// If the mode is set to OneShot, we increment the index
			if(mode == TweenMode.OneShot)
			{
				index++;

				// If the index is less than the array of transforms, start the tween again.
				if(index < transforms.Length - 2)
					Start(true);
			}
			else
			{
				// We aren't running this as a single shot, so increment the index and attempt to wrap it.
				SharedMethods.IncrementAndWrap(ref index, 0, transforms.Length);
			}
		}
	}
}