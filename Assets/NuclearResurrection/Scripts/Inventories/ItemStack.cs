// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 23/03/2022 11:29 AM

using Sirenix.OdinInspector;

using System;

namespace NuclearResurrection.Inventories
{
	[Serializable]
	public class ItemStack
	{
		[ShowInInspector] public Item Held { get; }
		[ShowInInspector] public int Count { get; private set; }

		public ItemStack(Item _held)
		{
			Held = _held;
			Count = 1;
		}

		public void Increment() => Count++;
		public void Decrement() => Count--;
	}
}