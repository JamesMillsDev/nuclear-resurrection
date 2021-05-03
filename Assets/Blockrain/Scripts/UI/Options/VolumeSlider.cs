using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

using TMPro;

using static Blockrain.Utilities.Maths;

namespace Blockrain.UI.Options
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private AnimationCurve volumeCurve = default;
        [SerializeField] private string volumeParameter = default;
        [SerializeField] private AudioMixer mixer = default;
        [SerializeField] private TextMeshProUGUI volumeText = null;

        private string PreferenceKey => $"<Volume>/{volumeParameter}";

        private Slider slider;

        // Start is called before the first frame update
        void Start()
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
            mixer.SetFloat(volumeParameter, Map(volumeCurve.Evaluate(_newVolume), 0, 1, -80, 20));
            PlayerPrefs.SetFloat(PreferenceKey, _newVolume);

            TrySetVolumeText();
        }

        private void TrySetVolumeText()
        {
            if(volumeText != null)
                volumeText.text = Map(slider.value, 0, 1, 0, 10).ToString();
        }
    }
}