// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 04/05/2021 04:00 PM
// Created On: MODI

using System.Collections.Generic;

using TunaTK.AssetManagement;

using UnityEngine;
using UnityEngine.UI;

namespace TunaTK.UI.Settings
{
	[RequireComponent(typeof(Dropdown))]
	public abstract class OptionsDropdown : AssetBehaviour 
	{
        /// <summary>The option name that will be used in the <see cref="OptionID"/> for the setting.</summary>
        protected abstract string Option { get; }
        /// <summary>The option ID that will be used when the option is saved.</summary>
        private string OptionID => $"<{Application.productName}>/<Graphics>/{Option}";

        //@cond
        protected Dropdown dropdown;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            dropdown = gameObject.GetComponent<Dropdown>();
            SetupOptions();
            dropdown.onValueChanged.AddListener(OnOptionChanged);
        }
        //@endcond

        /// <summary>Called in <see cref="Start"/> so that the dropdown can all be setup.</summary>
        protected virtual void SetupOptions()
        {
            dropdown.ClearOptions();

            dropdown.AddOptions(GetOptions());
            dropdown.value = Options.GetValue(OptionID, GetDefaultValue());
            dropdown.RefreshShownValue();
        }

        /// <summary>Abstract function that will get the default value for the dropdown if it has never been set before.</summary>
        protected abstract int GetDefaultValue();
        /// <summary>Get all the options available for this dropdown so it can be added to the dropdown itself.</summary>
        protected abstract List<Dropdown.OptionData> GetOptions();
        /// <summary>Used for setting the actual option in the editor like setting the fullscreen mode.</summary>
        protected abstract void OnOptionSelected(int _option);

        /// <summary>The callback for when the dropdown option is changed.</summary>
        /// <param name="_option">The newly selected option from the dropdown.</param>
        private void OnOptionChanged(int _option)
        {
            OnOptionSelected(_option);
            Options.SaveValue(OptionID, _option);
        }
    }
}