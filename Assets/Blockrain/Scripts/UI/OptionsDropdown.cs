using TMPro;

using UnityEngine;

using System.Collections.Generic;

namespace Blockrain.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public abstract class OptionsDropdown : MonoBehaviour
    {
        protected TMP_Dropdown dropdown;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            dropdown = gameObject.GetComponent<TMP_Dropdown>();
            SetupOptions();
            dropdown.onValueChanged.AddListener(OnOptionChanged);
        }

        protected virtual void SetupOptions()
        {
            dropdown.ClearOptions();

            dropdown.AddOptions(GetOptions());
            dropdown.value = GetDefaultValue();
            dropdown.RefreshShownValue();
        }

        protected abstract int GetDefaultValue();
        protected abstract List<TMP_Dropdown.OptionData> GetOptions();
        protected abstract void OnOptionSelected(int _option);

        private void OnOptionChanged(int _option) => OnOptionSelected(_option);
    }
}