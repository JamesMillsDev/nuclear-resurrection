using System.Threading;
using System.Threading.Tasks;

using TunaTK.Augments;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Blockrain.Character
{
	public class InventoryAugment : Augment<Player>
	{
		[SerializeField] private InputActionReference openInventoryAction;
		[SerializeField] private InputActionReference closeAction;

		private CameraAugment cameraAugment;

		protected override Task OnInitialisation(Player _user, object[] _params)
		{
			if(!_user.TryGetAugment(out cameraAugment))
			{
				Debug.LogException(new MissingReferenceException("Missing Camera Augment"));
				return Task.FromCanceled(CancellationToken.None);
			}

			InputAction action = _user.Input.actions.FindAction(openInventoryAction.action.id);
			action.performed += OnOpenInventoryPerformed;

			action = _user.Input.actions.FindAction(closeAction.action.id);
			action.performed += OnCloseInventoryPerformed;

			return base.OnInitialisation(_user, _params);
		}

		private void OnOpenInventoryPerformed(InputAction.CallbackContext _context) => cameraAugment.Disable();
		private void OnCloseInventoryPerformed(InputAction.CallbackContext _context) => cameraAugment.Enable();
	}
}