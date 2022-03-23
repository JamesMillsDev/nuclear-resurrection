using System.Threading.Tasks;

using TunaTK.Augments;
using TunaTK.Events;

using UnityEngine;
using UnityEngine.InputSystem;

namespace NuclearResurrection.Entity.Player
{
	public class CameraAugment : Augment<PlayerEntity>
	{
		public bool Enabled => Cursor.lockState == CursorLockMode.Locked;

		private Transform CamTransform => camera.transform;

		[SerializeField] private InputActionReference lookAction;

		[Space] [SerializeField, Range(0, 3)] private float sensitivity = .5f;
		[SerializeField] private Vector2 verticalBounds = new Vector2(90f, -45f);

		[Header("Collisions")] [SerializeField, Range(.05f, 1f)]
		private float collisionRadius = .25f;

		[SerializeField, Min(.1f)] private float cameraDistance = .1f;
		[SerializeField, Range(.05f, .2f)] private float damping = .1f;

		private new Camera camera;
		private Transform player;

		private Vector2 rotation;
		private Vector3 velocity = Vector3.zero;

		public void Enable()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			EventBus.Raise(new CameraStateChangeEvent(true));
		}

		public void Disable()
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			EventBus.Raise(new CameraStateChangeEvent(false));
		}
		
		protected override Task OnInitialisation(PlayerEntity _user, object[] _params)
		{
			player = _user.transform;
			
			// Get the camera component and enabled it
			camera = gameObject.GetComponentInChildren<Camera>();
			CamTransform.localPosition = new Vector3(0, 0, -cameraDistance);
			
			_user.Input.camera = camera;
			SetCameraState(true);
			
			return base.OnInitialisation(_user, _params);
		}

		private void OnValidate()
		{
			verticalBounds.x = Mathf.Clamp(verticalBounds.x, -90f, 0f);
			verticalBounds.y = Mathf.Clamp(verticalBounds.y, 0f, 90f);
		}

		protected override void InitialisedUpdate()
		{
			if(Enabled)
			{
				UpdateRotation();
				UpdatePosition();
			}
		}

		private void UpdateRotation()
		{
			// Get the actual look movement from the input action
			Vector2 lookVector = lookAction.action.ReadValue<Vector2>();

			// Apply the rotation to the vector and clamp the vertical
			rotation.x += lookVector.x * sensitivity;
			rotation.y += lookVector.y * sensitivity;
			rotation.y = Mathf.Clamp(rotation.y, verticalBounds.x, verticalBounds.y);

			ApplyRotation(rotation);
		}

		private void ApplyRotation(Vector2 _rotation)
		{
			// Set the camera and player rotations to the calculated ones
			transform.localRotation = Quaternion.AngleAxis(_rotation.y, Vector3.left);
			player.localRotation = Quaternion.AngleAxis(_rotation.x, Vector3.up);
		}

		private void UpdatePosition()
		{
			Vector3 newPos = Vector3.SmoothDamp(CamTransform.position, CalculatePos(), ref velocity, damping);

			CamTransform.position = newPos;
		}

		private void SetCameraState(bool _state)
		{
			Cursor.lockState = _state ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = !_state;
			
			EventBus.Raise(new CameraStateChangeEvent(_state));
		}

		private Vector3 CalculatePos()
		{
			Vector3 newPos = transform.position - transform.forward * cameraDistance;

			// Calculate actual end point
			Vector3 direction = -CamTransform.forward;

			// Cast a ray from the target to the camera with the length as the distance
			if(Physics.Raycast(transform.position, direction, out RaycastHit hit, cameraDistance))
			{
				newPos = hit.point + (hit.normal * collisionRadius);
			}

			return newPos;
		}
	}
}