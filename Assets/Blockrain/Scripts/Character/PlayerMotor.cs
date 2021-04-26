using UnityEngine;
using UnityEngine.InputSystem;

namespace Blockrain.Character
{
    public class PlayerMotor : MonoBehaviour
    {
        public bool IsGrounded { get; private set; }
        public Rigidbody Body => rigidbody;

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

        private new CameraController camera;
        private new Rigidbody rigidbody;
        private new CapsuleCollider collider;

        private float speedOnGroundModifier = 1;
        private float speedInAirModifier = 1;

        private float lastTimeInAir = 0f;
        private float inAirGroundCheckDelay = .2f;

        private bool isJumpPressed = false;
        private bool isSprintPressed = false;
        private bool isCrouchPressed = false;

        public void Setup(CameraController _camera, Rigidbody _rigidbody, CapsuleCollider _collider)
        {
            camera = _camera;
            rigidbody = _rigidbody;
            collider = _collider;

            // Cache required components

            jumpAction.action.performed += (_context) => isJumpPressed = true;
            jumpAction.action.canceled += (_context) => isJumpPressed = false;
            sprintAction.action.performed += (_context) => isSprintPressed = IsGrounded ? true : false;
            sprintAction.action.canceled += (_context) => isSprintPressed = false;
            crouchAction.action.performed += (_context) => isCrouchPressed = IsGrounded ? true : false;
            crouchAction.action.canceled += (_context) => isCrouchPressed = false;
        }

        public void FixedUpdate()
        {
            if(!camera.Enabled)
                return;

            CheckGrounded();
            HandleMovement(moveAction.action.ReadValue<Vector2>());
            ApplyExtraGravity();
            UpdateJump();
        }

        private void CheckGrounded()
        {
            float chosenGroundCheckDistance = IsGrounded ? groundDistanceCheck : groundDistanceInAirCheck;

            if(Time.time >= lastTimeInAir + inAirGroundCheckDelay)
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
                        else
                        {
                            IsGrounded = false;
                        }
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
            if(!camera.Enabled)
                return;

            float maxSpeed = IsGrounded ? maxSpeedOnGround : maxSpeedInAir;
            float modifier = IsGrounded ? speedOnGroundModifier : speedInAirModifier;
            modifier *= isSprintPressed && IsGrounded ? sprintMultiplier : isCrouchPressed ? crouchMultiplier : 1;

            Vector3 forward = transform.forward * _axis.y;
            Vector3 right = transform.right * _axis.x;
            Vector3 desiredVelocity = ((forward + right) * maxSpeed * modifier) - rigidbody.velocity;

            if(CanMoveInDirection(desiredVelocity))
            {
                rigidbody.AddForce(new Vector3(desiredVelocity.x, 0, desiredVelocity.z), ForceMode.Impulse);
            }
        }

        private void ApplyExtraGravity()
        {
            if(rigidbody.velocity.y < 0)
            {
                rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if(rigidbody.velocity.y > 0 && !isJumpPressed)
            {
                rigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        private bool CanMoveInDirection(Vector3 _targetDir)
        {
            RaycastHit[] hits = CapsuleCastAllInDirection(_targetDir, 0.5f);

            foreach(RaycastHit hit in hits)
            {
                if(hit.collider.CompareTag(wallTag) || hit.collider.gameObject.layer == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private RaycastHit[] CapsuleCastAllInDirection(Vector3 _dir, float _distance)
        {
            Vector3 top = transform.position + collider.center + Vector3.up * ((collider.height * 0.5f) - collider.radius);
            Vector3 bot = transform.position + collider.center - Vector3.up * ((collider.height * 0.5f) - collider.radius);

            return Physics.CapsuleCastAll(top, bot, collider.radius * 0.9f, _dir, _distance, layerChecks);
        }

        private void UpdateJump()
        {
            if(!camera.Enabled)
                return;

            if(isJumpPressed && IsGrounded)
            {
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                IsGrounded = false;

                lastTimeInAir = Time.time;
            }
        }
    }
}