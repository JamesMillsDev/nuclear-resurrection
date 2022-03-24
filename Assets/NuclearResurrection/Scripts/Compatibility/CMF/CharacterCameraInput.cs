// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 16/03/2022 10:01 PM

using CMF;

using UnityEngine;
using UnityEngine.InputSystem;

namespace NuclearResurrection.Compatibility.CMF
{
	public class CharacterCameraInput : CameraInput
	{
		[SerializeField] private InputActionReference mouseInput;

		public override float GetHorizontalCameraInput() => mouseInput.action.ReadValue<Vector2>().x;

		public override float GetVerticalCameraInput() => -mouseInput.action.ReadValue<Vector2>().y;
	}
}