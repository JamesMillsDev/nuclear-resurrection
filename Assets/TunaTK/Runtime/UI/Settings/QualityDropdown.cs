// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 04/05/2021 04:19 PM
// Created On: MODI

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace TunaTK.UI.Settings
{
	public class QualityDropdown : OptionsDropdown 
	{
        //@cond
        protected override string Option => "Quality";
        //@endcond

        /// <summary></summary>
        protected override int GetDefaultValue() => QualitySettings.GetQualityLevel();

        /// <summary></summary>
        protected override List<Dropdown.OptionData> GetOptions()
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            foreach(string quality in QualitySettings.names)
                options.Add(new Dropdown.OptionData(quality));

            return options;
        }

        /// <summary></summary>
        /// <param name="_option"></param>
        protected override void OnOptionSelected(int _option) => QualitySettings.SetQualityLevel(_option);
    }
}