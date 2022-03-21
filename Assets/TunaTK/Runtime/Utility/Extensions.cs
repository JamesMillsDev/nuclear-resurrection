// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 17/05/2021 04:26 PM
// Created On: NS00J002003DE01

using System.Threading.Tasks;

using UnityEngine;

namespace TunaTK.Utility
{
	public static class Extensions
	{
		public static void Reset(this Transform _transform)
		{
			_transform.localPosition = Vector3.zero;
			_transform.localRotation = Quaternion.identity;
			_transform.localScale = Vector3.one;
		}
		
		public static async void Process(this Task _task) => await _task;
	}
}