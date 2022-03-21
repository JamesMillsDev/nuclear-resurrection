using Blockrain.Entity.Player;

using Sirenix.OdinInspector;

using System.Threading;
using System.Threading.Tasks;

using TunaTK.Augments;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Blockrain.Inventories
{
	public class InventoryAugment : Augment<PlayerEntity>
	{
		[SerializeField, TabGroup("InventoryAugment", "Input Actions")]
		private InputActionReference openInventoryAction;

		[SerializeField, TabGroup("InventoryAugment", "Input Actions")]
		private InputActionReference closeAction;

		[SerializeField, TabGroup("InventoryAugment", "Inventory Settings")]
		private int inventorySize = 10;

		private CameraAugment cameraAugment;

		protected override Task OnInitialisation(PlayerEntity _user, object[] _params)
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