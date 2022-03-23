// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 23/03/2022 11:26 AM

using System.Collections.Generic;

namespace NuclearResurrection.Inventories
{
	public interface IInventory
	{
		public int InventorySize { get; }

		public IEnumerable<ItemStack> GetInventory();

		public void AddItem(Item _item);

		public void RemoveItem(Item _item);

		public ItemStack Find(Item _item);
	}
}