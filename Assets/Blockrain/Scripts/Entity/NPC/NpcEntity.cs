using Blockrain.Entity.Player;

using UnityEngine;

namespace Blockrain.Entity.NPC
{
	public class NpcEntity : Entity
	{
		public override void PlayerInteract(PlayerEntity _entity)
		{
			Debug.Log(gameObject.name);
		}
	}
}