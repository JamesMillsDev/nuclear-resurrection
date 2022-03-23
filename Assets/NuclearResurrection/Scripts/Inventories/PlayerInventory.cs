// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 23/03/2022 11:28 AM

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace NuclearResurrection.Inventories
{
	[Serializable]
	public class PlayerInventory : IInventory
	{
		public int InventorySize => inventorySize;

		[SerializeField] private int inventorySize = 10;

		[SerializeField] private ItemStack[] inventory;

		public PlayerInventory() => inventory = new ItemStack[inventorySize];

		public IEnumerable<ItemStack> GetInventory() => inventory;

		public ItemStack GetItem(int _index) => inventory[_index];

		public void AddItem(Item _item)
		{
			ItemStack itemStack = Find(_item);

			int itemIndex = Array.IndexOf(inventory, itemStack);
			if(itemIndex > -1 && itemStack != default(ItemStack))
			{
				inventory[itemIndex].Increment();
			}
			else
			{
				for(int i = 0; i < inventorySize; i++)
				{
					if(inventory[i] == default(ItemStack))
					{
						inventory[i] = new ItemStack(_item);
						break;
					}
				}
			}
		}

		public void RemoveItem(Item _item)
		{
			ItemStack itemStack = Find(_item);

			int itemIndex = Array.IndexOf(inventory, itemStack);
			if(itemIndex > -1)
			{
				inventory[itemIndex].Decrement();
				if(inventory[itemIndex].Count == 0)
					inventory[itemIndex] = null;
			}
			else
			{
				Debug.LogException(new InvalidOperationException($"Item {_item.name} isn't in the inventory."));
			}
		}

		public ItemStack Find(Item _item) => inventory.FirstOrDefault(_itemStack =>
		{
			if(_itemStack != default(ItemStack))
				return _itemStack.Held == _item;

			return false;
		});
	}
}