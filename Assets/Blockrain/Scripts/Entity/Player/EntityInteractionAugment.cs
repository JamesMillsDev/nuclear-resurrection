using System.Threading.Tasks;

using TunaTK.Augments;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Blockrain.Entity.Player
{
	public class EntityInteractionAugment : Augment<PlayerEntity>
	{
		[SerializeField] private InputActionReference interactButton;
		private Entity closeEntity;

		protected override Task OnInitialisation(PlayerEntity _user, object[] _params)
		{
			InputAction interact = _user.Input.actions.FindAction(interactButton.action.id);
			interact.performed += _ =>
			{
				if(closeEntity != null)
					closeEntity.PlayerInteract(user);
			};
			
			return base.OnInitialisation(_user, _params);
		}

		private void OnTriggerEnter(Collider _other)
		{
			Entity entity = _other.GetComponent<Entity>();

			if(entity != null && closeEntity == null)
				closeEntity = entity;
		}

		private void OnTriggerExit(Collider _other)
		{
			Entity entity = _other.GetComponent<Entity>();

			if(entity != null && closeEntity == entity)
				closeEntity = null;
		}
	}
}