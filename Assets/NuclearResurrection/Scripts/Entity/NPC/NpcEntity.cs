using NuclearResurrection.Entity.Player;

using UnityEngine;

namespace NuclearResurrection.Entity.NPC
{
	public class NpcEntity : Entity
	{
		public override void PlayerInteract(PlayerEntity _entity)
		{
			Debug.Log(gameObject.name);
		}
	}
}