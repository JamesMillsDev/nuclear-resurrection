using NuclearResurrection.Entity.Player;

using TunaTK.AssetManagement;

namespace NuclearResurrection.Entity
{
	public abstract class Entity : AssetBehaviour
	{
		public abstract void PlayerInteract(PlayerEntity _entity);
	}
}