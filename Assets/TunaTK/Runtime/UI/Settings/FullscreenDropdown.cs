// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 04/05/2021 04:15 PM
// Created On: MODI

using UnityEngine;
using UnityEngine.UI;

using Enum = System.Enum;
using System.Collections.Generic;

namespace TunaTK.UI.Settings
{
	public class FullscreenDropdown : OptionsDropdown
	{
        //@cond
        protected override string Option => "Fullscreen";
        //@endcond

        /// <summary></summary>
        protected override int GetDefaultValue() => (int)FullScreenMode.FullScreenWindow;

        /// <summary></summary>
        protected override List<Dropdown.OptionData> GetOptions()
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            foreach(FullScreenMode mode in Enum.GetValues(typeof(FullScreenMode)))
                options.Add(new Dropdown.OptionData(mode.ToString()));

            return options;
        }

        /// <summary></summary>
        /// <param name="_option"></param>
        protected override void OnOptionSelected(int _option) => Screen.fullScreenMode = (FullScreenMode)_option;
	}
}