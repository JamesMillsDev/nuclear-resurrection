using NuclearResurrection.Entity.Player;

using Sirenix.OdinInspector;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TunaTK.Augments;

using UnityEngine;
using UnityEngine.InputSystem;

using NuclearResurrection.Compatibility.CMF;

namespace NuclearResurrection.Inventories
{
	public class InventoryAugment : Augment<PlayerEntity>
	{
		[ShowInInspector, ReadOnly, TabGroup("InventoryAugment", "Inventory")]
		public PlayerInventory Inventory { get; private set; }

		[SerializeField, TabGroup("InventoryAugment", "Input Actions")]
		private InputActionReference openInventoryAction;

		[SerializeField, TabGroup("InventoryAugment", "Input Actions")]
		private InputActionReference closeAction;

		[SerializeField, TabGroup("InventoryAugment", "Inventory")]
		private InventorySlot inventorySlotTemplate;
		
		[SerializeField, TabGroup("InventoryAugment", "Inventory")]
		private GameObject inventoryHolder;

		private CmfAugment cmfAugment;

		private readonly List<InventorySlot> slots = new List<InventorySlot>();

		protected override Task OnInitialisation(PlayerEntity _user, object[] _params)
		{
			if(!_user.TryGetAugment(out cmfAugment))
			{
				Debug.LogException(new MissingReferenceException("Missing Camera Augment"));
				return Task.FromCanceled(CancellationToken.None);
			}

			Inventory = new PlayerInventory();

			for(int i = 0; i < Inventory.InventorySize; i++)
			{
				InventorySlot newSlot = Instantiate(inventorySlotTemplate, inventoryHolder.transform);
				newSlot.name = $"Slot ({i + 1})";
				newSlot.gameObject.SetActive(true);
				slots.Add(newSlot);
			}
			
			inventoryHolder.SetActive(false);

			InputAction action = _user.Input.actions.FindAction(openInventoryAction.action.id);
			action.performed += OnOpenInventoryPerformed;

			action = _user.Input.actions.FindAction(closeAction.action.id);
			action.performed += OnCloseInventoryPerformed;

			return base.OnInitialisation(_user, _params);
		}

		private void OnOpenInventoryPerformed(InputAction.CallbackContext _context)
		{
			cmfAugment.Disable();

			for(int i = 0; i < slots.Count; i++)
			{
				ItemStack itemStack = Inventory.GetItem(i);
				if(itemStack != default(ItemStack))
					slots[i].SetItem(itemStack);
			}
			
			inventoryHolder.SetActive(true);
		}

		private void OnCloseInventoryPerformed(InputAction.CallbackContext _context)
		{
			cmfAugment.Enable();
			inventoryHolder.SetActive(false);
		}
	}
}