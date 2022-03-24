// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 16/03/2022 9:58 PM

using CMF;

using UnityEngine;
using UnityEngine.InputSystem;

namespace NuclearResurrection.Compatibility.CMF
{
	public class CharacterMovementInput : CharacterInput
	{
		[SerializeField] private InputActionReference movementAction;
		[SerializeField] private InputActionReference jumpAction;

		private float horizontalMovement;
		private float verticalMovement;

		private void Start()
		{
			movementAction.action.Enable();
			jumpAction.action.Enable();
		}

		private void Update()
		{
			Vector2 movement = movementAction.action.ReadValue<Vector2>();
			horizontalMovement = movement.x;
			verticalMovement = movement.y;
		}

		public override float GetHorizontalMovementInput() => horizontalMovement;

		public override float GetVerticalMovementInput() => verticalMovement;

		public override bool IsJumpKeyPressed() => Mathf.Approximately(jumpAction.action.ReadValue<float>(), 1);
	}
}