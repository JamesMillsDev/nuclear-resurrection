// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 04/05/2021 03:14 PM
// Created On: MODI

using Sirenix.OdinInspector;

using System.Diagnostics.CodeAnalysis;

using TunaTK.Utility;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TunaTK.UI.Settings
{
#pragma warning disable CS0649
	[RequireComponent(typeof(Slider)), HideMonoScript]
	[SuppressMessage("ReSharper", "InvalidXmlDocComment")]
	public class VolumeSlider : SerializedMonoBehaviour
	{
		[SerializeField, Required]
		///<summary>The audio mixer to update the parameters on.</summary>
		private AudioMixer mixer;

		[SerializeField]
		///<summary>The exposed property that will be updated when the slider changed.</summary>
		private string volumeProperty;

		[Space] [SerializeField]
		///<summary>The curve that will determine the fade of the volume when the slider is updated.</summary>
		private AnimationCurve volumeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[SerializeField]
		///<summary>The text that will display the volume on the screen, this is shown from 0-100</summary>
		private Text valueDisplay;

		//@cond
		private string OptionID => $"<{Application.productName}>/<Volume>/{volumeProperty}";
		private Slider slider;

		// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only)
		private void OnValidate()
		{
			if(slider == null)
				slider = gameObject.ValidateComponent<Slider>();

			slider.value = Options.GetValue<float>(OptionID, 1);
			slider.minValue = 0;
			slider.maxValue = 1;
			TrySetDisplay(slider.value);
		}

		// Use this for initialization
		private void Start()
		{
			if(slider == null)
				slider = gameObject.ValidateComponent<Slider>();

			slider.onValueChanged.AddListener(OnValueChanged);
		}
		//@endcond

		/// <summary>The callback for when the value on the slider is changed.</summary>
		/// <param name="_value">The new value of the slider when it was changed.</param>
		private void OnValueChanged(float _value)
		{
			mixer.SetFloat(volumeProperty, SharedMethods.Remap(volumeCurve.Evaluate(_value), 0, 1, -80, 20));
			TrySetDisplay(_value);

			Options.SaveValue(OptionID, _value);
		}

		/// <summary>Attempts to change the text if it is set in the inspector.</summary>
		/// <param name="_value">The value to be displayed onto the screen.</param>
		private void TrySetDisplay(float _value)
		{
			if(valueDisplay != null)
				valueDisplay.text = SharedMethods.RoundToInt(SharedMethods.Remap(_value, 0, 1, 0, 100)).ToString();
		}
	}
}