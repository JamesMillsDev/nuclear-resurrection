using UnityEngine;
using UnityEngine.InputSystem;

namespace Blockrain.Character
{
    public class CameraController : MonoBehaviour
    {
        public bool Enabled => Cursor.lockState == CursorLockMode.Locked;

        [SerializeField] private InputActionReference lookAction;

        [Space]

        [SerializeField, Range(0, 3)] private float sensitivity = .5f;
        [SerializeField, Range(0, 90)] private float verticalBounds = 90f;

        private new Camera camera;
        private Transform player;

        private Vector2 rotation;

        public void Enable()
        {
            camera.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Disable()
        {
            camera.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Setup(Transform _player)
        {
            player = _player;

            // Get the camera component and enabled it
            camera = gameObject.GetComponent<Camera>();
        }

        public void Update()
        {
            SetCursorState(!Enabled);

            if(Enabled)
            {
                // Get the actual look movement from the input action
                Vector2 lookVector = lookAction.action.ReadValue<Vector2>();

                // Apply the rotation to the vector and clamp the vertical
                rotation.x += lookVector.x * sensitivity;
                rotation.y += lookVector.y * sensitivity;
                rotation.y = Mathf.Clamp(rotation.y, -verticalBounds, verticalBounds);

                // Set the camera and player rotations to the calculated ones
                transform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);
                player.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
            }
        }

        private void SetCursorState(bool _state)
        {
            Cursor.lockState = _state ? CursorLockMode.None :  CursorLockMode.Locked;
            Cursor.visible = _state;
        }
    }
}