using System.Threading.Tasks;

using TunaTK.Augments;
using TunaTK.Events;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Blockrain.Entity.Player
{
    public class MovementAugment : EventAugment<PlayerEntity>
    {
        public bool IsGrounded { get; private set; }
        public Rigidbody Body { get; private set; }

        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference crouchAction;

        [Space]

        [SerializeField, Range(1, 20)] private float jumpForce = 8f;

        [Space]

        [SerializeField, Range(1, 20)] private float maxSpeedOnGround = 8;
        [SerializeField, Range(1, 20)] private float maxSpeedInAir = 8;

        [Space]

        [SerializeField, Min(.1f)] private float groundDistanceCheck = 1f;
        [SerializeField, Min(.2f)] private float groundDistanceInAirCheck = .2f;

        [Space]

        [SerializeField, Range(.5f, 5f)] private float sprintMultiplier = 2f;
        [SerializeField, Range(.5f, 5f)] private float crouchMultiplier = .5f;

        [Space]

        [SerializeField, Min(.1f)] private float fallMultiplier = 2.5f;
        [SerializeField, Min(.2f)] private float lowJumpMultiplier = 2f;

        [Space]

        [SerializeField] private LayerMask layerChecks;
        [SerializeField] private string groundTag;
        [SerializeField] private string wallTag;

        private new CapsuleCollider collider;

        private const float SPEED_ON_GROUND_MODIFIER = 1;
        private const float SPEED_IN_AIR_MODIFIER = 1;
        private const float IN_AIR_GROUND_CHECK_DELAY = .2f;

        private float lastTimeInAir;

        private bool isJumpPressed;
        private bool isSprintPressed;
        private bool isCrouchPressed;
        private bool cameraEnabled;

        protected override Task OnInitialisation(PlayerEntity _user, object[] _params)
        {
            Body = user.Body;
            collider = user.Collider;

            InputAction jump = _user.Input.actions.FindAction(jumpAction.action.id);
            jump.performed += _ => isJumpPressed = true;
            jump.canceled += _ => isJumpPressed = false;

            InputAction sprint = _user.Input.actions.FindAction(sprintAction.action.id);
            sprint.performed += _ => isSprintPressed = IsGrounded;
            sprint.canceled += _ => isSprintPressed = false;
            
            InputAction crouch = _user.Input.actions.FindAction(crouchAction.action.id);
            crouch.performed += _ => isCrouchPressed = IsGrounded;
            crouch.canceled += _ => isCrouchPressed = false;
            
            return base.OnInitialisation(_user, _params);
        }

        [SubscribeEvent]
        // ReSharper disable once UnusedMember.Local
        private void OnCameraStateChanged(CameraStateChangeEvent _event) => cameraEnabled = _event.state;

        protected override void InitialisedFixedUpdate()
        {
            if(!cameraEnabled)
                return;

            CheckGrounded();
            HandleMovement(moveAction.action.ReadValue<Vector2>());
            ApplyExtraGravity();
            UpdateJump();
        }

        private void CheckGrounded()
        {
            float chosenGroundCheckDistance = IsGrounded ? groundDistanceCheck : groundDistanceInAirCheck;

            if(Time.time >= lastTimeInAir + IN_AIR_GROUND_CHECK_DELAY)
            {
                RaycastHit[] hits = CapsuleCastAllInDirection(-transform.up, chosenGroundCheckDistance);

                if(hits.Length > 0)
                {
                    foreach(RaycastHit hit in hits)
                    {
                        if(hit.collider.CompareTag(groundTag) || hit.collider.gameObject.layer == 0)
                        {
                            IsGrounded = true;
                            return;
                        }

                        IsGrounded = false;
                    }
                }
                else
                {
                    IsGrounded = false;
                }
            }
        }

        private void HandleMovement(Vector2 _axis)
        {
            float maxSpeed = IsGrounded ? maxSpeedOnGround : maxSpeedInAir;
            float modifier = IsGrounded ? SPEED_ON_GROUND_MODIFIER : SPEED_IN_AIR_MODIFIER;
            modifier *= isSprintPressed && IsGrounded ? sprintMultiplier : isCrouchPressed ? crouchMultiplier : 1;

            Vector3 forward = transform.forward * _axis.y;
            Vector3 right = transform.right * _axis.x;
            Vector3 desiredVelocity = (forward + right) * (maxSpeed * modifier) - Body.velocity;

            if(CanMoveInDirection(desiredVelocity))
                Body.AddForce(new Vector3(desiredVelocity.x, 0, desiredVelocity.z), ForceMode.Impulse);
        }

        private void ApplyExtraGravity()
        {
            if(Body.velocity.y < 0)
            {
                Body.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
            else if(Body.velocity.y > 0 && !isJumpPressed)
            {
                Body.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
            }
        }

        private bool CanMoveInDirection(Vector3 _targetDir)
        {
            RaycastHit[] hits = CapsuleCastAllInDirection(_targetDir, 0.5f);

            foreach(RaycastHit hit in hits)
            {
                if(hit.collider.CompareTag(wallTag) || hit.collider.gameObject.layer == 0)
                    return false;
            }

            return true;
        }

        private RaycastHit[] CapsuleCastAllInDirection(Vector3 _dir, float _distance)
        {
            Vector3 top = transform.position + collider.center + Vector3.up * (collider.height * 0.5f - collider.radius);
            Vector3 bot = transform.position + collider.center - Vector3.up * (collider.height * 0.5f - collider.radius);

            return Physics.CapsuleCastAll(top, bot, collider.radius * 0.9f, _dir, _distance, layerChecks);
        }

        private void UpdateJump()
        {
            if(isJumpPressed && IsGrounded)
            {
                Body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                IsGrounded = false;

                lastTimeInAir = Time.time;
            }
        }
    }
}