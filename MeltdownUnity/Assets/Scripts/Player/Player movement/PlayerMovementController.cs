using System.Collections;
using System.Collections.Generic;
using CustomAttributes;
using UnityEngine;

/// <summary>
/// Player movement. math, a lot of math.
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
	#region Varaibles
	[Header("Movement locking")]

	[Tooltip("Weather the player can move or not")]
	public bool Locked = false;


	[Header("Speed variables")]

	[ShowOnly] // used to multiply the move direction.
	public float speed = 0;

	[Tooltip("The speed for walking")]
	public float WalkSpeed = 5f;

	[Tooltip("The speed for sprinting")]
	public float SprintSpeed = 10f;

	[Tooltip("The multiplyer for movement speed in the air, 0 = nothing, 1 = normal")]
	public float AirMovementMultiplier = 0.2f;

	[Tooltip("The max speed allowed in the air")]
	public float MaxAirSpeed = 10f;

	private float _crouchSpeedScale = 1; // 1 = normal speed. Its like speed penalty.

	private bool _isSprinting = false;


	[Header("Gravity")]

	[Tooltip("Normal gravity")]
	public float Gravity = 9.81f;

	[Tooltip("Gravity when grounded")]
	public float IdleGravity = -4f;


	[Header("Jumping")]
	public float JumpHeight = 1.4f;


	[Header("Ignored layers and tags")]
	public LayerMask IgnoredLayers;
	public string[] IgnoredTags;


	[Header("Ground check state")]

	[Tooltip("Weather the player is touching the ground or not")]
	public bool isGrounded = false;

	private bool _isUnableToUncrouch = false;
	private bool _isCrouched = false;


	[Header("Defult height")]

	[Tooltip("The defult height of the player")]
	public float _playerStandingHeight = 1.75f;

	private CharacterController _characterContoller;
	private float _playerCrouchingHeight;

	[Tooltip("Used to move the camera when crouching")]
	public Transform CameraHolderTransform;

	private float _cameraStandingHeight = 0.6f;
	private float _cameraCrouchHeight;


	/// <summary>
	/// Varibles that have no place or are all private.
	/// </summary>
	[Header("Velocity")]
	[ShowOnly] public Vector3 velocity; // special variable.
	private Vector3 finalMoveDir;
	private Vector3 _gravityVelocity;
	private Vector3 _jumpVector;

	// used to move the player along a slope.
	private Vector3 _slopeNormal = Vector3.zero;
	private bool _isOnSlope = false;

	// used to reduce how much control the player has on icy surfaces.
	private bool _isOnIce = false;

	[Header("Moveing check")]
	// for audio, this serves not other purpose.
	public bool IsMoving = false;
	#endregion


	void Start()
	{
		// set the variable. We do this to save energy with drag and drop.
		_characterContoller = GetComponent<CharacterController>();

		// Set the heights.
		_playerCrouchingHeight = _playerStandingHeight / 2f;
		_cameraCrouchHeight = _cameraStandingHeight / 2f;

		// make suere the plauer is not locked.
		Locked = false;
	}


	void Update()
	{
		// Stops movement
		if (Locked) return;

		// calls the functions so the player can work.
		// there is so much code, putting it in update will be too much.
		// each section is broken down into idividual functions.
		// their order is really important. If a funtion is move, it will break somthing.

		HandleGravity();

		HandleGroundCheck();

		HandleMovement();
		HandleJumping();

		HandleCrouching();
	}

	private Vector3 GetInput()
	{
		float zPos = 0;
		float zNeg = 0;
		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Forward))) zPos = 1f;
		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Backward))) zNeg = -1f;

		float xPos = 0;
		float xNeg = 0;
		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Right))) xPos = 1f;
		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Left))) xNeg = -1f;

		Vector3 moveDirection = Vector3.zero;

		moveDirection.x = xPos + xNeg;
		moveDirection.z = zPos + zNeg;


		moveDirection = transform.right * moveDirection.x + transform.forward * moveDirection.z;

		return moveDirection.normalized;
	}


	private void HandleMovement()
	{
		// get input
		Vector3 moveDirection = GetInput();

		// normalize the move direction

		// sprint check, we check if the player is sprinting / can sprint.
		if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !_isOnIce)
		{
			_isSprinting = true;
		}
		else
		{
			_isSprinting = false;
		}

		// setting the speed mult.
		// Sprint check.
		if (_isSprinting && !_isCrouched && isGrounded)
		{
			speed = SprintSpeed;
		}
		// crouch check.
		else if (_isCrouched && isGrounded)
		{
			speed = WalkSpeed * _crouchSpeedScale;
		}
		// defualt.
		else
		{
			speed = WalkSpeed;
		}


		// start of useless

		// for detecting walking. If you can't read the code here is a run down.
		// the move direction is from the input directly. x being horizontal (A = -1 and D = 1) and z being vertical (W = 1 and S = -1).
		// This is normalised to get a total value of one.
		// The movedirection is then compared to vector zero (0,0,0). If move direction does not equall the vector zero, then the plauer is moving.
		// otherwise the player is not moving.
		if (moveDirection.normalized != Vector3.zero)
		{
			IsMoving = true;
		}
		else
		{
			IsMoving = false;
		}
		// end of useless


		// we get the final move direction with the speed.
		finalMoveDir = moveDirection.normalized * speed;

		// we check if we are on the ground and have traction.
		if (isGrounded && !_isOnIce && !_isOnSlope)
		{
			// we set the velocity.
			velocity.x = finalMoveDir.x;
			velocity.z = finalMoveDir.z;
		}
		else if (!isGrounded)
		{
			// we add to the velocity

			// ! This may be from a old speed cap and can be remmoved. the Y.
			// we seperate the y as we want to keep its' values.
			float y = velocity.y;

			// we add to the velocity
			if (moveDirection != Vector3.zero) velocity += moveDirection.normalized * AirMovementMultiplier * (Time.deltaTime);

			// we return the y.
			velocity.y = y;
		}


	}

	// we check to see if we collided into somthing and add force because character contoller cannot do that.
	void OnCollisionEnter(Collision other)
	{
		// Now this is very important, a null check is used to see if we can add a force to a rigid body.
		// If not, we dont get any errors.
		if (other.rigidbody != null)
		{
			other.rigidbody.AddForce(new Vector3(other.transform.position.x - transform.position.x, 0, other.transform.position.z - transform.position.z), ForceMode.Impulse);
		}
	}

	// we check to see if we collided into somthing and add force because character contoller cannot do that.
	void OnCollisionStay(Collision other)
	{
		// Now this is very important, a null check is used to see if we can add a force to a rigid body.
		// If not, we dont get any errors.
		if (other.rigidbody != null)
		{
			other.rigidbody.AddForce(new Vector3(other.transform.position.x - transform.position.x, 0, other.transform.position.z - transform.position.z), ForceMode.Impulse);
		}
	}

	// handles the Y velocity.
	private void HandleGravity()
	{
		// Store the hit referance.
		RaycastHit hit;

		// we get the object hit.
		Physics.Raycast(transform.position, Vector3.down, out hit, _characterContoller.height / 2f + 0.2f);

		// we check if we are on ice.
		if (hit.collider != null && hit.collider.tag == "Ice") _isOnIce = true;
		else _isOnIce = false;

		// we check if we are grounded and add the apropriate gravity velocity.
		if (isGrounded && velocity.y < -IdleGravity)
		{
			velocity.y = (-IdleGravity);
		}
		else
		{
			velocity.y += (-Gravity) * Time.deltaTime;
		}

		// this is getting the 2d vector x and z from the 3d vector velocity, and checking if the magnitude is larger
		// than the max air speed.
		if (Mathf.Pow(velocity.z, 2f) + Mathf.Pow(velocity.x, 2f) > Mathf.Pow(MaxAirSpeed, 2f))
		{
			// we cache the y velocity because we are not dealing with that.
			float _ = velocity.y;

			// set the velocity to the correct speed.
			velocity = velocity.normalized * MaxAirSpeed;

			// add the Y velocity back.
			velocity.y = _;
		}

		// We only call this once and in once place, here.
		_characterContoller.Move(velocity * Time.deltaTime);

		// We check if the plauer hit their head.
		if (_characterContoller.velocity.y <= 0 && velocity.y > _characterContoller.velocity.y)
		{
			velocity.y = _characterContoller.velocity.y;
		}

		// prevents the player of being stuck in endless propulsion.

		// TODO need to change into a a reduce to speed with decline instad of removing the velocity.

		if (velocity.z != _characterContoller.velocity.z && finalMoveDir.magnitude <= 1f)
		{
			velocity.z = _characterContoller.velocity.z;
		}

		if (velocity.x != _characterContoller.velocity.x && finalMoveDir.magnitude <= 1f)
		{
			velocity.x = _characterContoller.velocity.x;
		}


	}

	// does what its named to do.
	private void HandleJumping()
	{
		// we check if the player press the jump key. We also check if we are grounded and not on a slope or falling.
		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Jump)) && isGrounded && !_isOnSlope && velocity.y <= 0)
		{
			// we get the 2d movemment vector and store it.
			Vector3 MovementPlaneXZ = new Vector3(finalMoveDir.x, 0, finalMoveDir.z);

			// we normalise the speed.
			if (MovementPlaneXZ.magnitude > MaxAirSpeed)
			{
				MovementPlaneXZ = MovementPlaneXZ.normalized * MaxAirSpeed;
			}

			// we create a new varible to store data.
			float verticalPlane = 0f;

			// if we have less speed than what jumping can achive, then add the jump force.
			if (velocity.y < Mathf.Sqrt(JumpHeight * -2f * (-Gravity)))
			{
				verticalPlane = Mathf.Sqrt(JumpHeight * -2f * (-Gravity)) - velocity.y;
			}

			// we add the combined forces to get the jump vector.
			_jumpVector = new Vector3(MovementPlaneXZ.x, verticalPlane, MovementPlaneXZ.z);

			// we add the jump vector to the player.
			velocity += _jumpVector;
		}
	}

	// Deals with crouching.
	private void HandleCrouching()
	{

		// player layer is 6
		// we need to make sure we dont raycast the player, so we do bit shifting to exlude the layer.
		// I will not explain how a computer works. please google layer shifting unity or use https://discussions.unity.com/t/bit-shifting-layer-mask/211874
		int layer = 6;
		int mask = ~(1 << layer);

		// if we are crouched and there is somthing above us then we cannot uncrouch. otherwise we can.
		if (_isCrouched && Physics.SphereCast(transform.position, _characterContoller.radius, transform.up, out RaycastHit hit, (_playerStandingHeight + _playerCrouchingHeight - _characterContoller.radius) * 0.5f, mask))
		{
			_isUnableToUncrouch = true;
		}
		else
		{
			_isUnableToUncrouch = false;
		}

		// ! do not see the point of this check.
		// if we are unable to uncrouch we set the heights to crouch.
		if (_isUnableToUncrouch)
		{
			_characterContoller.height = _playerCrouchingHeight;
			MoveCamY(_cameraCrouchHeight);

		}

		// if we are crouching, then we crouch.
		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Crouch)))
		{
			_characterContoller.height = _playerCrouchingHeight;
			MoveCamY(_cameraCrouchHeight);
			_isCrouched = true;
			_crouchSpeedScale = 0.5f;
		}
		// if we are able to uncrouch, then we can uncrouch.
		else if (!_isUnableToUncrouch)
		{
			_characterContoller.height = _playerStandingHeight;
			MoveCamY(_cameraStandingHeight);
			_isCrouched = false;
			_crouchSpeedScale = 1f;
		}
	}

	// a funtion to simplify the process.
	// moves the camera's local y position.
	private void MoveCamY(float y)
	{
		CameraHolderTransform.localPosition = new Vector3(0, y, 0);
	}



	// FYI the void means that the function returns nothing. so there is no reqiurement to return a value.
	private void HandleGroundCheck()
	{
		// store the data.
		RaycastHit hit;

		// there is a lot here.
		// The first part is getting the surface hit.
		// The second part checks if the normal of the surface hit is greater than the slope limit of the character controller.
		// we are on a slope. 
		if (Physics.Raycast(transform.position, -transform.up, out hit, (_characterContoller.height / 2) + _characterContoller.stepOffset, ~IgnoredLayers) && Vector3.Dot(hit.normal, Vector3.up) < 1 - (_characterContoller.slopeLimit / 180))
		{

			_isOnSlope = true;
			_slopeNormal = hit.normal;
		}
		else
		{
			_isOnSlope = false;
		}

		// ground check

		// did the raycast hit somthing? if not then return gournd is false. We also use a sphere check. the math here is just positioning the checks.
		if (!Physics.Raycast(transform.position, -transform.up, out hit, (_characterContoller.height / 2) + _characterContoller.stepOffset, ~IgnoredLayers) &&
		!Physics.CheckSphere(transform.position - (Vector3.up * ((_characterContoller.height / 2) + (_characterContoller.stepOffset - _characterContoller.radius))), _characterContoller.radius, ~IgnoredLayers))
		{
			isGrounded = false;
			return;
		}


		// did the hit object have a ignored tag?
		if (Physics.Raycast(transform.position, -transform.up, out hit, (_characterContoller.height / 2) + _characterContoller.stepOffset, ~IgnoredLayers))
		{
			if (CompareTag(hit))
			{
				isGrounded = false;
				return;
			}
		}


		// otherwise after all of that then we are grounded.
		isGrounded = true;


	}

	// simple fucntion. This just checks if the hit object has a ignored tag.
	private bool CompareTag(RaycastHit hit)
	{
		if (IgnoredTags.Length > 0)
		{
			foreach (var tag in IgnoredTags)
			{
				if (hit.collider.CompareTag(tag))
				{
					return true;
				}
			}
		}

		return false;
	}
}
