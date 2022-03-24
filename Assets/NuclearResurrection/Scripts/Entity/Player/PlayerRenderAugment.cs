// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 24/03/2022 3:31 PM

using CMF;

using System;
using System.Threading.Tasks;

using TunaTK.Augments;
using TunaTK.Utility;

using UnityEngine;

namespace NuclearResurrection.Entity.Player
{
	public class PlayerRenderAugment : Augment<PlayerEntity>
	{
		private static readonly int forwardMovement = Animator.StringToHash("forward_movement");
		private static readonly int turnMovement = Animator.StringToHash("turn_movement");

		[SerializeField] private float speedSmoothing = .25f;
		[SerializeField] private Animator animator;
		
		[SerializeField] private TurnTowardControllerVelocity turnController;

		private Vector2 velocity;
		private Vector2 lastSpeed;

		protected override Task OnInitialisation(PlayerEntity _user, object[] _params)
		{
			turnController.controller = _user.PlayerController;
			
			return base.OnInitialisation(_user, _params);
		}

		protected override void InitialisedLateUpdate()
		{
			Vector3 movement = user.PlayerController.GetVelocity();
			movement = Vector3.ProjectOnPlane(movement, transform.up);
			float forward = Math.Abs(Vector3.Distance(Vector3.zero, movement));

			Vector2 target = new Vector2
			{
				y = SharedMethods.Remap(forward, 0, user.PlayerController.movementSpeed, 0, 1)
			};

			Vector2 speed = Vector2.SmoothDamp(lastSpeed, target, ref velocity, speedSmoothing);
			
			animator.SetFloat(forwardMovement, speed.y);

			lastSpeed = speed;
		}
	}
}