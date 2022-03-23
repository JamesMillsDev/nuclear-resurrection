using UnityEngine;

using TMPro;

using Enum = System.Enum;
using System.Collections.Generic;

namespace NuclearResurrection.UI.Options
{
    public class FullscreenDropdown : OptionsDropdown
    {
        protected override string Option => "Fullscreen";

        protected override int GetDefaultValue() => (int)FullScreenMode.FullScreenWindow;

        protected override List<TMP_Dropdown.OptionData> GetOptions()
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach(FullScreenMode mode in Enum.GetValues(typeof(FullScreenMode)))
                options.Add(new TMP_Dropdown.OptionData(mode.ToString()));

            return options;
        }

        protected override void OnOptionSelected(int _option) => Screen.fullScreenMode = (FullScreenMode)_option;
    }
}