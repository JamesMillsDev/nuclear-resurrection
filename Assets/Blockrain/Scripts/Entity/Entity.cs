using Blockrain.Entity.Player;

using TunaTK.AssetManagement;

namespace Blockrain.Entity
{
	public abstract class Entity : AssetBehaviour
	{
		public abstract void PlayerInteract(PlayerEntity _entity);
	}
}