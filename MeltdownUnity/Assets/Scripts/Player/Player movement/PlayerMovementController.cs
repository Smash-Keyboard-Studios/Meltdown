using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class PlayerMovementController : MonoBehaviour
{
	[HideInInspector] // no need to touch this
	public bool Locked = false;

	public float WalkSpeed = 5f;
	public float SprintSpeed = 10f;

	public float Gravity = 9.81f;
	public float IdleGravity = -4f;

	public float JumpHeight = 1.4f;

	public float AirMovementMultiplier = 0.2f;
	public float MaxAirSpeed = 10f;

	public LayerMask IgnoredLayers;
	public string[] IgnoredTags;

	// public float Mass = 72.5f;



	private CharacterController _characterContoller;

	private Vector3 _gravityVelocity;
	public Vector3 velocity;
	private Vector3 _jumpVector;
	Vector3 finalMoveDir;



	public float speed = 0;

	// 1 = normal speed. Its like speed penalty.
	private float _crouchSpeedScale = 1;

	public bool isGrounded = false;

	private bool _isUnableToUncrouch = false;

	private bool _isCrouched = false;

	public float _playerStandingHeight = 1.75f;
	private float _playerCrouchingHeight;

	public Transform CameraHolderTransform;

	private float _cameraStandingHeight = 0.6f;
	private float _cameraCrouchHeight;

	private bool _isSprinting = false;

	private bool _isOnIce = false;

	private bool _isOnSlope = false;
	private Vector3 _slopeNormal = Vector3.zero;


	// Start is called before the first frame update
	void Start()
	{
		_characterContoller = GetComponent<CharacterController>();

		_playerCrouchingHeight = _playerStandingHeight / 2f;
		_cameraCrouchHeight = _cameraStandingHeight / 2f;

		Locked = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Locked) return; // Stops movement
		HandleGravity();

		HandleGroundCheck();

		HandleMovement();
		HandleJumping();

		HandleCrouching();
	}


	private void HandleMovement()
	{
		// redo with new input system.

		float xAxis = Input.GetAxisRaw("Horizontal");
		float zAxis = Input.GetAxisRaw("Vertical");

		Vector3 moveDirection = transform.right * xAxis + transform.forward * zAxis;

		// float zPos = 0;
		// float zNeg = 0;
		// if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Forward))) zPos = 1f;
		// if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Backward))) zNeg = -1f;

		// float xPos = 0;
		// float xNeg = 0;
		// if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Right))) xPos = 1f;
		// if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Left))) xNeg = -1f;

		// Vector3 moveDirection = Vector3.zero;

		// moveDirection.x = xPos + xNeg;
		// moveDirection.z = zPos + zNeg;

		// moveDirection = transform.right * moveDirection.x + transform.forward * moveDirection.z;

		moveDirection.Normalize();

		if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !_isOnIce)
		{
			_isSprinting = true;
		}
		else
		{
			_isSprinting = false;
		}


		if (_isSprinting && !_isCrouched && isGrounded)
		{
			speed = SprintSpeed;
		}
		else if (_isCrouched && isGrounded)
		{
			speed = WalkSpeed * _crouchSpeedScale;
		}
		else
		{
			speed = WalkSpeed;
		}

		finalMoveDir = moveDirection.normalized * speed;
		if (isGrounded && !_isOnIce && !_isOnSlope)
		{
			velocity.x = finalMoveDir.x;
			velocity.z = finalMoveDir.z;
			// _characterContoller.Move(finalMoveDir * Time.deltaTime);
		}
		else if (!isGrounded)
		{
			float y = velocity.y;
			if (moveDirection != Vector3.zero)
				velocity += moveDirection.normalized * AirMovementMultiplier * (Time.deltaTime);
			velocity.y = y;
		}


	}

	void OnCollisionEnter(Collision other)
	{
		if (other.rigidbody != null)
		{
			other.rigidbody.AddForce(new Vector3(other.transform.position.x - transform.position.x, 0, other.transform.position.z - transform.position.z), ForceMode.Impulse);
		}
	}

	void OnCollisionStay(Collision other)
	{
		if (other.rigidbody != null)
		{
			other.rigidbody.AddForce(new Vector3(other.transform.position.x - transform.position.x, 0, other.transform.position.z - transform.position.z), ForceMode.Impulse);
		}
	}


	private void HandleGravity()
	{
		RaycastHit hit;

		Physics.Raycast(transform.position, Vector3.down, out hit, _characterContoller.height / 2f + 0.2f);

		//bool onSlope = Vector3.Dot(hit.normal, Vector3.up) != 1 ? true : false && Physics.Raycast(transform.position, Vector3.down, _characterContoller.height / 2f + 0.5f);

		if (hit.collider != null && hit.collider.tag == "Ice") _isOnIce = true;
		else _isOnIce = false;


		if (isGrounded && velocity.y < -IdleGravity)
		{
			// ! Temp comment
			// if (_isOnSlope && velocity.y > (-IdleGravity) * 2f)
			// {
			// 	velocity.x = velocity.x * 0.1f * AirMovementMultiplier;
			// 	velocity.z = velocity.z * 0.1f * AirMovementMultiplier;

			// 	Vector3 newVel = velocity;
			// 	newVel.y = (-IdleGravity);

			// 	newVel = Vector3.ProjectOnPlane(newVel, _slopeNormal);

			// 	// i hate this
			// 	velocity = newVel;
			// }
			// else
			// {
			velocity.y = (-IdleGravity);
			// }
		}
		else
		{
			// if (_isOnSlope)
			// {
			// 	Vector3 newVel = velocity;
			// 	newVel.y = IdleGravity;

			// 	newVel = Vector3.ProjectOnPlane(newVel, _slopeNormal);

			// 	velocity = newVel;
			// }
			// else
			// {
			velocity.y += (-Gravity) * Time.deltaTime;
			// }
		}

		if (isGrounded && velocity.y < 0 && !_isOnIce && !_isOnSlope)
		{
			// stop moving the player through velocity when on
			// velocity.x = 0;
			// velocity.z = 0;
		}

		if (Mathf.Pow(velocity.z, 2f) + Mathf.Pow(velocity.x, 2f) > Mathf.Pow(MaxAirSpeed, 2f))
		{
			float _ = velocity.y;

			velocity = velocity.normalized * MaxAirSpeed;

			velocity.y = _;
		}

		// velocity += _gravityVelocity;

		_characterContoller.Move(velocity * Time.deltaTime);

		if (_characterContoller.velocity.y <= 0 && velocity.y > _characterContoller.velocity.y)
		{
			// may change later
			velocity.y = _characterContoller.velocity.y;
		}

		// prevents the player of being stuck in endless propulsion.

		if (velocity.z != _characterContoller.velocity.z && finalMoveDir.magnitude <= 1f)
		{
			velocity.z = _characterContoller.velocity.z;
		}

		if (velocity.x != _characterContoller.velocity.x && finalMoveDir.magnitude <= 1f)
		{
			velocity.x = _characterContoller.velocity.x;
		}


	}

	private void HandleJumping()
	{
		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Jump)))
		{
			if (isGrounded && !_isOnSlope && velocity.y <= 0)
			{
				// _velocity.y = Mathf.Sqrt(JumpHeight * -2f * (-Gravity));
				Vector3 MovementPlaneXZ = new Vector3(finalMoveDir.x, 0, finalMoveDir.z);

				if (MovementPlaneXZ.magnitude > MaxAirSpeed)
				{
					MovementPlaneXZ = MovementPlaneXZ.normalized * MaxAirSpeed;
				}


				Vector3 verticalPlane = Vector3.zero;

				if (velocity.y < Mathf.Sqrt(JumpHeight * -2f * (-Gravity)))
				{
					verticalPlane.y = Mathf.Sqrt(JumpHeight * -2f * (-Gravity)) - velocity.y;
				}

				_jumpVector = new Vector3(MovementPlaneXZ.x, verticalPlane.y, MovementPlaneXZ.z);

				velocity += _jumpVector;
			}
		}
	}

	private void HandleCrouching()
	{

		// player layer
		int layer = 6;
		int mask = ~(1 << layer);


		if (_isCrouched && Physics.SphereCast(transform.position, _characterContoller.radius, transform.up, out RaycastHit hit, (_playerStandingHeight + _playerCrouchingHeight - _characterContoller.radius) * 0.5f, mask))
		{
			_isUnableToUncrouch = true;
		}
		else
		{
			_isUnableToUncrouch = false;
		}

		if (_isUnableToUncrouch)
		{
			_characterContoller.height = _playerCrouchingHeight;
			MoveCamY(_cameraCrouchHeight);

		}

		// TODO replace with new input system
		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Crouch)))
		{
			_characterContoller.height = _playerCrouchingHeight;
			MoveCamY(_cameraCrouchHeight);
			_isCrouched = true;
			_crouchSpeedScale = 0.5f;
		}
		else if (!_isUnableToUncrouch)
		{
			_characterContoller.height = _playerStandingHeight;
			MoveCamY(_cameraStandingHeight);
			_isCrouched = false;
			_crouchSpeedScale = 1f;
		}
	}

	private void MoveCamY(float y)
	{
		CameraHolderTransform.localPosition = new Vector3(0, y, 0);
	}

	// void OnDrawGizmos()
	// {
	// 	// Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - (_characterContoller.height / 2f) + _characterContoller.radius / 2f, transform.position.z), _characterContoller.radius / 4f);
	// }


	// FYI the void means that the function returns nothing. so there is no reqiurement to return a value.
	private void HandleGroundCheck()
	{

		//Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - ((_characterContoller.height / 2f) + _characterContoller.radius / 2f), transform.position.z), _characterContoller.radius / 4f, ~(1 << 6))
		//Physics.Raycast(transform.position, -transform.up, (_characterContoller.height / 2f) + 0.1f)


		RaycastHit hit;


		if (Physics.Raycast(transform.position, -transform.up, out hit, (_characterContoller.height / 2) + _characterContoller.stepOffset, ~IgnoredLayers) && Vector3.Dot(hit.normal, Vector3.up) < 1 - (_characterContoller.slopeLimit / 180))
		{

			_isOnSlope = true;
			_slopeNormal = hit.normal;
		}
		else
		{
			_isOnSlope = false;
		}
		//print(Vector3.Dot(hit.normal, Vector3.up) + " " + (1 - (_characterContoller.slopeLimit / 360)) + " " + (_characterContoller.slopeLimit / 180));




		// ground

		// did the raycast hit somthing? if not then return gournd is false.

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



		isGrounded = true;


	}

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
