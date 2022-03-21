using TunaTK.Augments;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Blockrain.Entity.Player
{
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
	public class PlayerEntity : User<PlayerEntity>
	{
		public Rigidbody Body { get; private set; }
		public CapsuleCollider Collider { get; private set; }
		public PlayerInput Input => input;

		[SerializeField] private PlayerInput input;

		// Start is called before the first frame update
		protected override void Start()
		{
			Body = gameObject.GetComponent<Rigidbody>();
			Collider = gameObject.GetComponent<CapsuleCollider>();

			base.Start();
		}
	}
}