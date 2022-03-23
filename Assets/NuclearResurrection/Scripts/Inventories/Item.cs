using Sirenix.OdinInspector;

using UnityEngine;

namespace NuclearResurrection.Inventories
{
	[CreateAssetMenu(fileName = "BasicItem", menuName = "Nuclear Resurrection/Inventory/Basic Item", order = 0)]
	public class Item : SerializedScriptableObject
	{
		public Sprite Icon => icon;
		
		[SerializeField] private Sprite icon;
	}
}