// Property of PYROCRACY STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 15/01/2019 09:54 AM
// Created On: DESKTOP-JN21C3R

using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace TunaTK
{
	/// <summary>Allows an enum to be marked as a flags drawer within the inspector.</summary>
	[AttributeUsage(AttributeTargets.Enum)]
	[ComVisible(true)]
	[Serializable]
	public class EnumFlagsAttribute : FlagsAttribute
	{
	}
}