using CMF;

using NuclearResurrection.Compatibility.CMF;

using TunaTK.Augments;

using UnityEngine;
using UnityEngine.InputSystem;

namespace NuclearResurrection.Entity.Player
{
	public class PlayerEntity : User<PlayerEntity>
	{
		public PlayerController PlayerController { get; private set; }
		public CapsuleCollider Collider { get; private set; }
		public Rigidbody Rigidbody { get; private set; }
		
		public PlayerInput Input => input;

		[SerializeField] private PlayerInput input;

		// Start is called before the first frame update
		protected override void Start()
		{
			PlayerController = gameObject.GetComponent<PlayerController>();
			Collider = gameObject.GetComponent<CapsuleCollider>();
			Rigidbody = gameObject.GetComponent<Rigidbody>();
			
			base.Start();
		}
	}
}