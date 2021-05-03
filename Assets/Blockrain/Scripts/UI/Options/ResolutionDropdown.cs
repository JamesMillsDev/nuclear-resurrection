using UnityEngine;

using TMPro;

using System.Collections.Generic;

namespace Blockrain.UI.Options
{ 
    public class ResolutionDropdown : OptionsDropdown
    {
        protected override string Option => "Resolution";

        private List<Resolution> resolutions;

        protected override void Start()
        {
            resolutions = new List<Resolution>(Screen.resolutions);

            base.Start();
        }

        protected override int GetDefaultValue() => resolutions.IndexOf(Screen.currentResolution);

        protected override List<TMP_Dropdown.OptionData> GetOptions()
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (Resolution res in resolutions)
                options.Add(new TMP_Dropdown.OptionData($"{res.width}x{res.height}"));

            return options;
        }

        protected override void OnOptionSelected(int _option)
        {
            Resolution res = resolutions[_option];

            Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRate);
        }
    }
}
