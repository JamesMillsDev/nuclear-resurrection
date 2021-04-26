using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blockrain.Character
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Player : MonoBehaviour
    {
        public CameraController Camera => camera;
        public PlayerMotor Motor => motor;

        private new Rigidbody rigidbody;
        private new CapsuleCollider collider;

        private new CameraController camera;
        private PlayerMotor motor;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = gameObject.GetComponent<Rigidbody>();
            collider = gameObject.GetComponent<CapsuleCollider>();

            camera = gameObject.GetComponentInChildren<CameraController>();
            motor = gameObject.GetComponentInChildren<PlayerMotor>();

            motor.Setup(camera, rigidbody, collider);
            camera.Setup(transform);

            camera.Enable();
        }
    }
}