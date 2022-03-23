// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 23/03/2022 12:15 PM

using Sirenix.OdinInspector;

using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace NuclearResurrection.Inventories
{
	public class InventorySlot : SerializedMonoBehaviour
	{
		[SerializeField] private Image image;
		[SerializeField] private TextMeshProUGUI countText;

		private ItemStack heldItem;

		private void Start()
		{
			image.sprite = null;
			image.color = Color.clear;
			countText.text = "";
		}

		public void SetItem(ItemStack _item)
		{
			heldItem = _item;
			countText.text = heldItem.Count > 1 ? heldItem.Count.ToString() : "";
			image.color = Color.white;
			image.sprite = heldItem.Held.Icon;
		}
	}
}