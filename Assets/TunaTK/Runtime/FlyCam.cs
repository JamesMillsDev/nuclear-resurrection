// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 30/10/2019 08:29 PM
// Created On: CHRONOS

using UnityEngine;

namespace TunaTK
{
	/// <summary>A simple fly camera that emulates that of the SceneView camera. This is really useful for testing environments in the game as they would look in builds or other things.</summary>
	public class FlyCam : MonoBehaviour
	{
		/// <summary>How sensitive the camera rotation is.</summary>
		[SerializeField] private float sensitivity = 90;

		/// <summary>How fast the camera will climb or descend when the corresponding keys are pressed.</summary>
		[SerializeField] private float climbSpeed = 4;

		/// <summary>The general speed of the camera. </summary>
		[SerializeField] private float moveSpeed = 10;

		/// <summary>How fast the camera moves when the control key is pressed.</summary>
		[SerializeField] private float slowMoveFactor = 0.25f;

		/// <summary>How fast the camera moves when the shift key is pressed.</summary>
		[SerializeField] private float fastMoveFactor = 3f;

		// @cond
		private Vector2 rotation;

		private void OnValidate()
		{
			sensitivity = Mathf.Clamp(sensitivity, 0, float.PositiveInfinity);
			climbSpeed = Mathf.Clamp(climbSpeed, 0, float.PositiveInfinity);
			moveSpeed = Mathf.Clamp(moveSpeed, 0, float.PositiveInfinity);
			slowMoveFactor = Mathf.Clamp(slowMoveFactor, 0, float.PositiveInfinity);
			fastMoveFactor = Mathf.Clamp(fastMoveFactor, 0, float.PositiveInfinity);
		}

		// Use this for initialization
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Update is called once per frame
		private void Update()
		{
			// Calculate the rotation of the camera based on the input
			rotation.x += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
			rotation.y += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
			rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);

			// Apply the calculated rotation
			transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
			transform.localRotation *= Quaternion.AngleAxis(rotation.y, Vector3.left);

			// Create the variables to store the speed changes
			float speed = moveSpeed;
			float climb = 0;

			// If either shift key is pressed...
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				// Update the speed variable by the fast move factor
				speed *= fastMoveFactor;
			// If either control key is pressed...
			else if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
				// Update the speed variable by the slow move factor
				speed *= slowMoveFactor;

			// Set the climb variables by the input
			if(Input.GetKey(KeyCode.Q))
				climb = climbSpeed;
			if(Input.GetKey(KeyCode.E))
				climb = -climbSpeed;

			// If the end key was pressed, invert the Cursor state
			if(Input.GetKeyDown(KeyCode.End))
			{
				Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
				Cursor.visible = !Cursor.visible;
			}

			// Apply the calculated movement to the position
			transform.position += transform.forward * (speed * Time.deltaTime * Input.GetAxis("Vertical"));
			transform.position += transform.right * (speed * Time.deltaTime * Input.GetAxis("Horizontal"));
			transform.position += transform.up * (climb * Time.deltaTime);
		}

		// @endcond
	}
}