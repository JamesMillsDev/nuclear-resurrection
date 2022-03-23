using NuclearResurrection.Utilities;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

using TMPro;

namespace NuclearResurrection.UI.Options
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private AnimationCurve volumeCurve;
        [SerializeField] private string volumeParameter;
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private TextMeshProUGUI volumeText;

        private string PreferenceKey => $"<Volume>/{volumeParameter}";

        private Slider slider;

        // Start is called before the first frame update
        private void Start()
        {
            slider = gameObject.GetComponent<Slider>();
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = PlayerPrefs.GetFloat(PreferenceKey, 1);

            slider.onValueChanged.AddListener(OnVolumeChanged);

            TrySetVolumeText();
        }

        private void OnVolumeChanged(float _newVolume)
        {
            mixer.SetFloat(volumeParameter, volumeCurve.Evaluate(_newVolume).Remap(0, 1, -80, 20));
            PlayerPrefs.SetFloat(PreferenceKey, _newVolume);

            TrySetVolumeText();
        }

        private void TrySetVolumeText()
        {
            if(volumeText != null)
                volumeText.text = slider.value.Remap(0, 1, 0, 10).ToString();
        }
    }
}