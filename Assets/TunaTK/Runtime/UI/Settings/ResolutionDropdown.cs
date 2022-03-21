// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 04/05/2021 04:21 PM
// Created On: MODI

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace TunaTK.UI.Settings
{
	public class ResolutionDropdown : OptionsDropdown 
	{
        //@cond
        protected override string Option => "Resolution";
        protected override void Start()
        {
            resolutions = new List<Resolution>(Screen.resolutions);
            base.Start();
        }
        //@endcond

        /// <summary>A list of every supported resolution for the monitor. This is filled out in Start.</summary>
        private List<Resolution> resolutions;

        /// <summary>Gets the default resolution for the monitor as the default value.</summary>
        protected override int GetDefaultValue() => resolutions.IndexOf(Screen.currentResolution);

        /// <summary>Returns a set of options for each resolution supported by the current monitor. They will be shown as width x height.</summary>
        protected override List<Dropdown.OptionData> GetOptions()
        {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

            foreach(Resolution res in resolutions)
                options.Add(new Dropdown.OptionData($"{res.width}x{res.height}"));

            return options;
        }

        /// <summary>Updates the resolution of the screen based on the passed option value.</summary>
        protected override void OnOptionSelected(int _option)
        {
            Resolution res = resolutions[_option];
            Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRate);
        }
    }
}