using UnityEngine;

using TMPro;

using System.Collections.Generic;

namespace NuclearResurrection.UI.Options
{
    public class QualityDropdown : OptionsDropdown
    {
        protected override string Option => "Quality";

        protected override int GetDefaultValue() => QualitySettings.GetQualityLevel();

        protected override List<TMP_Dropdown.OptionData> GetOptions()
        {
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (string level in QualitySettings.names)
                options.Add(new TMP_Dropdown.OptionData(level));

            return options;
        }

        protected override void OnOptionSelected(int _option) => QualitySettings.SetQualityLevel(_option);
    }
}