using NuclearResurrection.Entity.Player;
using NuclearResurrection.Inventories;

using UnityEngine;

namespace NuclearResurrection.Entity.NPC
{
	public class NpcEntity : Entity
	{
		[SerializeField] private Item givingItem;
		public override void PlayerInteract(PlayerEntity _entity)
		{
			if(_entity.TryGetAugment(out InventoryAugment inventory))
			{
				inventory.Inventory.AddItem(givingItem);
			}
		}
	}
}