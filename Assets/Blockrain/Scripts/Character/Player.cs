using TunaTK.Augments;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Blockrain.Character
{
	[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
	public class Player : User<Player>
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