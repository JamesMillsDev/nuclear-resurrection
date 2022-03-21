// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/22/2018 9:30:25 PM
// Created On: CHRONOS

using UnityEngine;

namespace TunaTK
{
    /// <summary>A class that contains some settings that is always available for any game development such as <see cref="EnableDeveloperMode"/> which is useful for developer options that can make testing easier.</summary>
    public static class TunaTKSettings
    {
        /// <summary>Converts the key to the correct path using the product name.</summary>
        /// <param name="_key">The key to convert.</param>
        private static string GetKeyWithProjectID(string _key) => $"<{Application.productName}>/{_key}";

        #region Setting Keys

        //@cond
        private const string EXTENDED_LOGGING_KEY = "<TunaTK>/UseExtendedLogging";
        private const string ENABLE_DEVELOPER_MODE_KEY = "<TunaTK>/EnableDeveloperMode";
        private const string MAPPED_INPUT_KEYBINDINGS_KEY = "<TunaTK>/MappedInputKeybindings";
        //@endcond

        #endregion

        /// <summary>Enabling this shows additional log messages in the console.</summary>
        public static bool UseExtendedLogging
        {
            get => LoadBool(EXTENDED_LOGGING_KEY, true);
            set => SaveBool(EXTENDED_LOGGING_KEY, value);
        }

        /// <summary>Enabling this will give extra developer functionality.</summary>
        public static bool EnableDeveloperMode
        {
            get => LoadBool(ENABLE_DEVELOPER_MODE_KEY, true);
            set => SaveBool(ENABLE_DEVELOPER_MODE_KEY, value);
        }

        /// <summary>Clears all key mappings from the settings system.</summary>
        public static void ClearKeyMappings() => Options.Delete(GetKeyWithProjectID(MAPPED_INPUT_KEYBINDINGS_KEY));

        /// <summary>Saves the passed <paramref name="_value"/> with to the <paramref name="_key"/> to the PlayerPrefs.</summary>
        /// <param name="_key">The key for the value we are saving.</param>
        /// <param name="_value">The value we are saving that will correspond with the key.</param>
        private static void SaveInt(string _key, int _value) => Options.SaveValue<int>(GetKeyWithProjectID(_key), _value);
        /// <summary>Saves the passed <paramref name="_value"/> with to the <paramref name="_key"/> to the PlayerPrefs.</summary>
        /// <param name="_key">The key for the value we are saving.</param>
        /// <param name="_value">The value we are saving that will correspond with the key.</param>
        private static void SaveString(string _key, string _value) => Options.SaveValue<string>(GetKeyWithProjectID(_key), _value);
        /// <summary>Saves the passed <paramref name="_value"/> with to the <paramref name="_key"/> to the PlayerPrefs.</summary>
        /// <param name="_key">The key for the value we are saving.</param>
        /// <param name="_value">The value we are saving that will correspond with the key.</param>
        private static void SaveBool(string _key, bool _value) => Options.SaveValue<bool>(GetKeyWithProjectID(_key), _value);
        /// <summary>Saves the passed <paramref name="_value"/> with to the <paramref name="_key"/> to the PlayerPrefs.</summary>
        /// <param name="_key">The key for the value we are saving.</param>
        /// <param name="_value">The value we are saving that will correspond with the key.</param>
        private static void SaveFloat(string _key, float _value) => Options.SaveValue<float>(GetKeyWithProjectID(_key), _value);

        /// <summary>Loads the passed value at <paramref name="_key"/> from the PlayerPrefs using <paramref name="_defaultValue"/> when the key doesn't exist.</summary>
        /// <param name="_key">The key for the value we are loading.</param>
        /// <param name="_defaultValue">The default value that will be used if the corresponding with the key doesn't exist in PlayerPrefs.</param>
        private static int LoadInt(string _key, int _defaultValue) => Options.GetValue<int>(GetKeyWithProjectID(_key), _defaultValue);
        /// <summary>Loads the passed value at <paramref name="_key"/> from the PlayerPrefs using <paramref name="_defaultValue"/> when the key doesn't exist.</summary>
        /// <param name="_key">The key for the value we are loading.</param>
        /// <param name="_defaultValue">The default value that will be used if the corresponding with the key doesn't exist in PlayerPrefs.</param>
        private static string LoadString(string _key, string _defaultValue) => Options.GetValue<string>(GetKeyWithProjectID(_key), _defaultValue);
        /// <summary>Loads the passed value at <paramref name="_key"/> from the PlayerPrefs using <paramref name="_defaultValue"/> when the key doesn't exist.</summary>
        /// <param name="_key">The key for the value we are loading.</param>
        /// <param name="_defaultValue">The default value that will be used if the corresponding with the key doesn't exist in PlayerPrefs.</param>
        private static bool LoadBool(string _key, bool _defaultValue) => Options.GetValue<bool>(GetKeyWithProjectID(_key), _defaultValue);
        /// <summary>Loads the passed value at <paramref name="_key"/> from the PlayerPrefs using <paramref name="_defaultValue"/> when the key doesn't exist.</summary>
        /// <param name="_key">The key for the value we are loading.</param>
        /// <param name="_defaultValue">The default value that will be used if the corresponding with the key doesn't exist in PlayerPrefs.</param>
        private static float LoadFloat(string _key, float _defaultValue) => Options.GetValue<float>(GetKeyWithProjectID(_key), _defaultValue);
    }
}
