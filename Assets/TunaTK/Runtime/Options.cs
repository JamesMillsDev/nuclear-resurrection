// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/22/2018 8:28:21 PM
// Created On: CHRONOS

using System;

using UnityEngine;

namespace TunaTK
{
	public static class Options
	{
		/// <summary>
		/// Saves a specific value to player preferences. The value type can be any of the following: integer, boolean, string, float or enum
		/// </summary>
		/// <param name="_key">The key to save the value to preferences under.</param>
		/// <param name="_value"></param>
		public static void SaveValue<T>(string _key, T _value)
		{
			if(typeof(T) == typeof(int))
			{
				PlayerPrefs.SetInt(_key, Convert.ToInt32(_value));
			}
			else if(typeof(T) == typeof(bool))
			{
				PlayerPrefs.SetInt(_key, Convert.ToBoolean(_value) ? 1 : 0);
			}
			else if(typeof(T) == typeof(string))
			{
				PlayerPrefs.SetString(_key, Convert.ToString(_value));
			}
			else if(typeof(T) == typeof(float))
			{
				PlayerPrefs.SetFloat(_key, Convert.ToSingle(_value));
			}
			else if(typeof(T) == typeof(Enum))
			{
				PlayerPrefs.SetString(_key, Convert.ToString(_value));
			}

			PlayerPrefs.Save();
		}

		/// <summary>
		/// Gets a specific value from the player preferences. The value type can be any of the following: integer, boolean, string, float or enum
		/// </summary>
		/// <param name="_key">The key to get the value from preferences under.</param>
		/// <param name="_defaultValue"></param>
		public static T GetValue<T>(string _key, T _defaultValue)
		{
			if(typeof(T) == typeof(int))
				return (T)Convert.ChangeType(PlayerPrefs.GetInt(_key, Convert.ToInt32(_defaultValue)), typeof(T));

			if(typeof(T) == typeof(bool))
				return (T)Convert.ChangeType(PlayerPrefs.GetInt(_key, Convert.ToBoolean(_defaultValue) ? 1 : 0) == 1, typeof(T));

			if(typeof(T) == typeof(string))
				return (T)Convert.ChangeType(PlayerPrefs.GetString(_key, Convert.ToString(_defaultValue)), typeof(T));

			if(typeof(T) == typeof(float))
				return (T)Convert.ChangeType(PlayerPrefs.GetFloat(_key, Convert.ToSingle(_defaultValue)), typeof(T));

			if(typeof(T) == typeof(Enum))
				return (T)Enum.Parse(typeof(T), PlayerPrefs.GetString(_key, Convert.ToString(_defaultValue)));

			throw new InvalidCastException($"Type {typeof(T)} is not a valid option type");
		}

		/// <summary>
		/// Deletes a setting from the stored settings system. Caution... CANNOT BE UNDONE.
		/// </summary>
		/// <param name="_key">The key to delete the setting from preferences.</param>
		public static void Delete(string _key) => PlayerPrefs.DeleteKey(_key);

		public static void Reset() => PlayerPrefs.DeleteAll();
	}
}