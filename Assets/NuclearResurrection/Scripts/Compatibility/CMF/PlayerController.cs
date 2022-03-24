// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 24/03/2022 1:52 PM

using CMF;

using UnityEngine;

namespace NuclearResurrection.Compatibility.CMF
{
	public class PlayerController : Controller
	{
		//References to attached components;
		protected Transform tr;
		protected Mover mover;
		protected CharacterInput characterInput;
		protected CeilingDetector ceilingDetector;

		//Jump key variables;
		private bool jumpInputIsLocked;
		private bool jumpKeyWasPressed;
		private bool jumpKeyWasLetGo;
		private bool jumpKeyIsPressed;

		//Movement speed;
		public float movementSpeed = 7f;

		//How fast the controller can change direction while in the air;
		//Higher values result in more air control;
		public float airControlRate = 2f;

		//Jump speed;
		public float jumpSpeed = 10f;

		//Jump duration variables;
		public float jumpDuration = 0.2f;
		private float currentJumpStartTime;

		//'AirFriction' determines how fast the controller loses its momentum while in the air;
		//'GroundFriction' is used instead, if the controller is grounded;
		public float airFriction = 0.5f;
		public float groundFriction = 100f;

		//Current momentum;
		protected Vector3 momentum = Vector3.zero;

		//Saved velocity from last frame;
		private Vector3 savedVelocity = Vector3.zero;

		//Saved horizontal movement velocity from last frame;
		private Vector3 savedMovementVelocity = Vector3.zero;

		//Amount of downward gravity;
		public float gravity = 30f;

		[Tooltip("How fast the character will slide down steep slopes.")]
		public float slideGravity = 5f;

		//Acceptable slope angle limit;
		public float slopeLimit = 80f;

		[Tooltip("Whether to calculate and apply momentum relative to the controller's transform.")]
		public bool useLocalMomentum;

		//Enum describing basic controller states; 
		public enum ControllerState
		{
			Grounded,
			Sliding,
			Falling,
			Rising,
			Jumping
		}

		private ControllerState currentControllerState = ControllerState.Falling;

		[Tooltip("Optional camera transform used for calculating movement direction. If assigned, character movement will take camera view into account.")]
		public Transform cameraTransform;

		private bool isSetup;

		//This function is called right after Awake(); It can be overridden by inheriting scripts;
		public void Setup(CharacterInput _input, CeilingDetector _detector, Transform _cameraTransform)
		{
			cameraTransform = _cameraTransform;
			mover = GetComponent<Mover>();
			tr = transform;
			characterInput = _input;
			ceilingDetector = _detector;

			if(characterInput == null)
				Debug.LogWarning("No character input script has been attached to this GameObject", gameObject);

			isSetup = true;
		}

		private void Update()
		{
			if(isSetup)
				HandleJumpKeyInput();
		}

		//Handle jump booleans for later use in FixedUpdate;
		private void HandleJumpKeyInput()
		{
			bool newJumpKeyPressedState = IsJumpKeyPressed();

			if(!jumpKeyIsPressed && newJumpKeyPressedState)
				jumpKeyWasPressed = true;

			if(jumpKeyIsPressed && !newJumpKeyPressedState)
			{
				jumpKeyWasLetGo = true;
				jumpInputIsLocked = false;
			}

			jumpKeyIsPressed = newJumpKeyPressedState;
		}

		private void FixedUpdate()
		{
			if(isSetup)
				ControllerUpdate();
		}

		//Update controller;
		//This function must be called every fixed update, in order for the controller to work correctly;
		private void ControllerUpdate()
		{
			//Check if mover is grounded;
			mover.CheckForGround();

			//Determine controller state;
			currentControllerState = DetermineControllerState();

			//Apply friction and gravity to 'momentum';
			HandleMomentum();

			//Check if the player has initiated a jump;
			HandleJumping();

			//Calculate movement velocity;
			Vector3 velocity = Vector3.zero;
			if(currentControllerState == ControllerState.Grounded)
				velocity = CalculateMovementVelocity();

			//If local momentum is used, transform momentum into world space first;
			Vector3 worldMomentum = momentum;
			if(useLocalMomentum)
				worldMomentum = tr.localToWorldMatrix * momentum;

			//Add current momentum to velocity;
			velocity += worldMomentum;

			//If player is grounded or sliding on a slope, extend mover's sensor range;
			//This enables the player to walk up/down stairs and slopes without losing ground contact;
			mover.SetExtendSensorRange(IsGrounded());

			//Set mover velocity;		
			mover.SetVelocity(velocity);

			//Store velocity for next frame;
			savedVelocity = velocity;

			//Save controller movement velocity;
			savedMovementVelocity = CalculateMovementVelocity();

			//Reset jump key booleans;
			jumpKeyWasLetGo = false;
			jumpKeyWasPressed = false;

			//Reset ceiling detector, if one is attached to this GameObject;
			if(ceilingDetector != null)
				ceilingDetector.ResetFlags();
		}

		//Calculate and return movement direction based on player input;
		//This function can be overridden by inheriting scripts to implement different player controls;
		protected virtual Vector3 CalculateMovementDirection()
		{
			//If no character input script is attached to this object, return;
			if(characterInput == null)
				return Vector3.zero;

			Vector3 velocity = Vector3.zero;

			//If no camera transform has been assigned, use the character's transform axes to calculate the movement direction;
			if(cameraTransform == null)
			{
				velocity += tr.right * characterInput.GetHorizontalMovementInput();
				velocity += tr.forward * characterInput.GetVerticalMovementInput();
			}
			else
			{
				//If a camera transform has been assigned, use the assigned transform's axes for movement direction;
				//Project movement direction so movement stays parallel to the ground;
				velocity += Vector3.ProjectOnPlane(cameraTransform.right, tr.up).normalized * characterInput.GetHorizontalMovementInput();
				velocity += Vector3.ProjectOnPlane(cameraTransform.forward, tr.up).normalized * characterInput.GetVerticalMovementInput();
			}

			//If necessary, clamp movement vector to magnitude of 1f;
			if(velocity.magnitude > 1f)
				velocity.Normalize();

			return velocity;
		}

		//Calculate and return movement velocity based on player input, controller state, ground normal [...];
		protected virtual Vector3 CalculateMovementVelocity()
		{
			//Calculate (normalized) movement direction;
			Vector3 velocity = CalculateMovementDirection();

			//Multiply (normalized) velocity with movement speed;
			velocity *= movementSpeed;

			return velocity;
		}

		//Returns 'true' if the player presses the jump key;
		//If no character input script is attached to this object, return;
		protected virtual bool IsJumpKeyPressed() => characterInput != null && characterInput.IsJumpKeyPressed();

		//Determine current controller state based on current momentum and whether the controller is grounded (or not);
		//Handle state transitions;
		private ControllerState DetermineControllerState()
		{
			//Check if vertical momentum is pointing upwards;
			bool isRising = IsRisingOrFalling() && (VectorMath.GetDotProduct(GetMomentum(), tr.up) > 0f);
			//Check if controller is sliding;
			bool isSliding = mover.IsGrounded() && IsGroundTooSteep();

			//Grounded;
			if(currentControllerState == ControllerState.Grounded)
			{
				if(isRising)
				{
					OnGroundContactLost();
					return ControllerState.Rising;
				}

				if(!mover.IsGrounded())
				{
					OnGroundContactLost();
					return ControllerState.Falling;
				}

				if(isSliding)
				{
					OnGroundContactLost();
					return ControllerState.Sliding;
				}

				return ControllerState.Grounded;
			}

			//Falling;
			if(currentControllerState == ControllerState.Falling)
			{
				if(isRising)
					return ControllerState.Rising;

				if(mover.IsGrounded() && !isSliding)
				{
					OnGroundContactRegained();
					return ControllerState.Grounded;
				}

				return isSliding ? ControllerState.Sliding : ControllerState.Falling;
			}

			//Sliding;
			if(currentControllerState == ControllerState.Sliding)
			{
				if(isRising)
				{
					OnGroundContactLost();
					return ControllerState.Rising;
				}

				if(!mover.IsGrounded())
				{
					OnGroundContactLost();
					return ControllerState.Falling;
				}

				if(mover.IsGrounded() && !isSliding)
				{
					OnGroundContactRegained();
					return ControllerState.Grounded;
				}

				return ControllerState.Sliding;
			}

			//Rising;
			if(currentControllerState == ControllerState.Rising)
			{
				if(!isRising)
				{
					if(mover.IsGrounded() && !isSliding)
					{
						OnGroundContactRegained();
						return ControllerState.Grounded;
					}

					if(isSliding)
						return ControllerState.Sliding;

					if(!mover.IsGrounded())
						return ControllerState.Falling;
				}

				//If a ceiling detector has been attached to this GameObject, check for ceiling hits;
				if(ceilingDetector != null)
				{
					if(ceilingDetector.HitCeiling())
					{
						OnCeilingContact();
						return ControllerState.Falling;
					}
				}

				return ControllerState.Rising;
			}

			//Jumping;
			if(currentControllerState == ControllerState.Jumping)
			{
				//Check for jump timeout;
				if((Time.time - currentJumpStartTime) > jumpDuration)
					return ControllerState.Rising;

				//Check if jump key was let go;
				if(jumpKeyWasLetGo)
					return ControllerState.Rising;

				//If a ceiling detector has been attached to this GameObject, check for ceiling hits;
				if(ceilingDetector != null)
				{
					if(ceilingDetector.HitCeiling())
					{
						OnCeilingContact();
						return ControllerState.Falling;
					}
				}

				return ControllerState.Jumping;
			}

			return ControllerState.Falling;
		}

		//Check if player has initiated a jump;
		private void HandleJumping()
		{
			if(currentControllerState == ControllerState.Grounded)
			{
				if((jumpKeyIsPressed || jumpKeyWasPressed) && !jumpInputIsLocked)
				{
					//Call events;
					OnGroundContactLost();
					OnJumpStart();

					currentControllerState = ControllerState.Jumping;
				}
			}
		}

		//Apply friction to both vertical and horizontal momentum based on 'friction' and 'gravity';
		//Handle movement in the air;
		//Handle sliding down steep slopes;
		private void HandleMomentum()
		{
			//If local momentum is used, transform momentum into world coordinates first;
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			Vector3 verticalMomentum = Vector3.zero;
			Vector3 horizontalMomentum = Vector3.zero;

			//Split momentum into vertical and horizontal components;
			if(momentum != Vector3.zero)
			{
				verticalMomentum = VectorMath.ExtractDotVector(momentum, tr.up);
				horizontalMomentum = momentum - verticalMomentum;
			}

			//Add gravity to vertical momentum;
			verticalMomentum -= tr.up * (gravity * Time.deltaTime);

			//Remove any downward force if the controller is grounded;
			if(currentControllerState == ControllerState.Grounded && VectorMath.GetDotProduct(verticalMomentum, tr.up) < 0f)
				verticalMomentum = Vector3.zero;

			//Manipulate momentum to steer controller in the air (if controller is not grounded or sliding);
			if(!IsGrounded())
			{
				Vector3 movementVelocity = CalculateMovementVelocity();

				//If controller has received additional momentum from somewhere else;
				if(horizontalMomentum.magnitude > movementSpeed)
				{
					//Prevent unwanted accumulation of speed in the direction of the current momentum;
					if(VectorMath.GetDotProduct(movementVelocity, horizontalMomentum.normalized) > 0f)
						movementVelocity = VectorMath.RemoveDotVector(movementVelocity, horizontalMomentum.normalized);

					//Lower air control slightly with a multiplier to add some 'weight' to any momentum applied to the controller;
					float _airControlMultiplier = 0.25f;
					horizontalMomentum += movementVelocity * (Time.deltaTime * airControlRate * _airControlMultiplier);
				}
				//If controller has not received additional momentum;
				else
				{
					//Clamp _horizontal velocity to prevent accumulation of speed;
					horizontalMomentum += movementVelocity * (Time.deltaTime * airControlRate);
					horizontalMomentum = Vector3.ClampMagnitude(horizontalMomentum, movementSpeed);
				}
			}

			//Steer controller on slopes;
			if(currentControllerState == ControllerState.Sliding)
			{
				//Calculate vector pointing away from slope;
				Vector3 pointDownVector = Vector3.ProjectOnPlane(mover.GetGroundNormal(), tr.up).normalized;

				//Calculate movement velocity;
				Vector3 slopeMovementVelocity = CalculateMovementVelocity();
				//Remove all velocity that is pointing up the slope;
				slopeMovementVelocity = VectorMath.RemoveDotVector(slopeMovementVelocity, pointDownVector);

				//Add movement velocity to momentum;
				horizontalMomentum += slopeMovementVelocity * Time.fixedDeltaTime;
			}

			//Apply friction to horizontal momentum based on whether the controller is grounded;
			horizontalMomentum = VectorMath.IncrementVectorTowardTargetVector(horizontalMomentum, currentControllerState == ControllerState.Grounded ? groundFriction : airFriction, Time.deltaTime, Vector3.zero);

			//Add horizontal and vertical momentum back together;
			momentum = horizontalMomentum + verticalMomentum;

			//Additional momentum calculations for sliding;
			if(currentControllerState == ControllerState.Sliding)
			{
				//Project the current momentum onto the current ground normal if the controller is sliding down a slope;
				momentum = Vector3.ProjectOnPlane(momentum, mover.GetGroundNormal());

				//Remove any upwards momentum when sliding;
				if(VectorMath.GetDotProduct(momentum, tr.up) > 0f)
					momentum = VectorMath.RemoveDotVector(momentum, tr.up);

				//Apply additional slide gravity;
				Vector3 slideDirection = Vector3.ProjectOnPlane(-tr.up, mover.GetGroundNormal()).normalized;
				momentum += slideDirection * (slideGravity * Time.deltaTime);
			}

			//If controller is jumping, override vertical velocity with jumpSpeed;
			if(currentControllerState == ControllerState.Jumping)
			{
				momentum = VectorMath.RemoveDotVector(momentum, tr.up);
				momentum += tr.up * jumpSpeed;
			}

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//Events;

		//This function is called when the player has initiated a jump;
		private void OnJumpStart()
		{
			//If local momentum is used, transform momentum into world coordinates first;
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			//Add jump force to momentum;
			momentum += tr.up * jumpSpeed;

			//Set jump start time;
			currentJumpStartTime = Time.time;

			//Lock jump input until jump key is released again;
			jumpInputIsLocked = true;

			//Call event;
			OnJump?.Invoke(momentum);

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//This function is called when the controller has lost ground contact, i.e. is either falling or rising, or generally in the air;
		private void OnGroundContactLost()
		{
			//If local momentum is used, transform momentum into world coordinates first;
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			//Get current movement velocity;
			Vector3 velocity = GetMovementVelocity();

			//Check if the controller has both momentum and a current movement velocity;
			if(velocity.sqrMagnitude >= 0f && momentum.sqrMagnitude > 0f)
			{
				//Project momentum onto movement direction;
				Vector3 projectedMomentum = Vector3.Project(momentum, velocity.normalized);
				//Calculate dot product to determine whether momentum and movement are aligned;
				float dot = VectorMath.GetDotProduct(projectedMomentum.normalized, velocity.normalized);

				//If current momentum is already pointing in the same direction as movement velocity,
				//Don't add further momentum (or limit movement velocity) to prevent unwanted speed accumulation;
				if(projectedMomentum.sqrMagnitude >= velocity.sqrMagnitude && dot > 0f)
					velocity = Vector3.zero;
				else if(dot > 0f)
					velocity -= projectedMomentum;
			}

			//Add movement velocity to momentum;
			momentum += velocity;

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//This function is called when the controller has landed on a surface after being in the air;
		private void OnGroundContactRegained()
		{
			//Call 'OnLand' event;
			if(OnLand != null)
			{
				Vector3 collisionVelocity = momentum;
				//If local momentum is used, transform momentum into world coordinates first;
				if(useLocalMomentum)
					collisionVelocity = tr.localToWorldMatrix * collisionVelocity;

				OnLand(collisionVelocity);
			}
		}

		//This function is called when the controller has collided with a ceiling while jumping or moving upwards;
		private void OnCeilingContact()
		{
			//If local momentum is used, transform momentum into world coordinates first;
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			//Remove all vertical parts of momentum;
			momentum = VectorMath.RemoveDotVector(momentum, tr.up);

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//Helper functions;

		//Returns 'true' if vertical momentum is above a small threshold;
		private bool IsRisingOrFalling()
		{
			//Calculate current vertical momentum;
			Vector3 verticalMomentum = VectorMath.ExtractDotVector(GetMomentum(), tr.up);

			//Setup threshold to check against;
			//For most applications, a value of '0.001f' is recommended;
			float _limit = 0.001f;

			//Return true if vertical momentum is above '_limit';
			return (verticalMomentum.magnitude > _limit);
		}

		//Returns true if angle between controller and ground normal is too big (> slope limit), i.e. ground is too steep;
		private bool IsGroundTooSteep()
		{
			if(!mover.IsGrounded())
				return true;

			return (Vector3.Angle(mover.GetGroundNormal(), tr.up) > slopeLimit);
		}

		//Getters;

		//Get last frame's velocity;
		public override Vector3 GetVelocity() => savedVelocity;

		//Get last frame's movement velocity (momentum is ignored);
		public override Vector3 GetMovementVelocity() => savedMovementVelocity;

		//Get current momentum;
		public Vector3 GetMomentum()
		{
			Vector3 worldMomentum = momentum;
			if(useLocalMomentum)
				worldMomentum = tr.localToWorldMatrix * momentum;

			return worldMomentum;
		}

		//Returns 'true' if controller is grounded (or sliding down a slope);
		public override bool IsGrounded() => currentControllerState == ControllerState.Grounded || currentControllerState == ControllerState.Sliding;

		//Returns 'true' if controller is sliding;
		public bool IsSliding() => currentControllerState == ControllerState.Sliding;

		//Add momentum to controller;
		public void AddMomentum(Vector3 _momentum)
		{
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			momentum += _momentum;

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//Set controller momentum directly;
		public void SetMomentum(Vector3 _newMomentum)
		{
			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * _newMomentum;
			else
				momentum = _newMomentum;
		}
	}
}