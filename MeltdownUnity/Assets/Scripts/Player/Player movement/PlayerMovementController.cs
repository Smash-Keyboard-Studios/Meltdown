using System.Collections;
using System.Collections.Generic;
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

	// public float Mass = 72.5f;



	private CharacterController _characterContoller;

	private Vector3 _velocity;
	private Vector3 _jumpVector;
	Vector3 finalMoveDir;



	private float _speed = 0;

	// 1 = normal speed. Its like speed penalty.
	private float _crouchSpeedScale = 1;

	private bool _isGrounded = false;

	private bool _isUnableToUncrouch = false;

	private bool _isCrouched = false;

	public float _playerStandingHeight = 1.75f;
	private float _playerCrouchingHeight;

	private Transform _cameraHolderTransform;

	private float _cameraStandingHeight = 0.6f;
	private float _cameraCrouchHeight;

	private bool _isSprinting = false;

	private bool _isOnIce = false;


	// Start is called before the first frame update
	void Start()
	{
		_characterContoller = GetComponent<CharacterController>();
		_cameraHolderTransform = Camera.main.transform.parent.GetComponent<Transform>();

		_playerCrouchingHeight = _playerStandingHeight / 2f;
		_cameraCrouchHeight = _cameraStandingHeight / 2f;

		Locked = false;
	}

	// Update is called once per frame
	void Update()
	{
		HandleGravity();

		if (Locked) return; // Stops movement
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

		if (Input.GetKey(KeyCode.LeftShift) && _isGrounded && !_isOnIce)
		{
			_isSprinting = true;
		}
		else
		{
			_isSprinting = false;
		}


		if (_isSprinting && !_isCrouched)
		{
			_speed = SprintSpeed;
		}
		else if (_isCrouched)
		{
			_speed = WalkSpeed * _crouchSpeedScale;
		}
		else
		{
			_speed = WalkSpeed;
		}

		if (_isGrounded && !_isOnIce)
		{
			finalMoveDir = moveDirection * _speed;
			_characterContoller.Move(finalMoveDir * Time.deltaTime);
		}
		else
		{
			float y = _velocity.y;
			if (moveDirection != Vector3.zero)
				_velocity += moveDirection * AirMovementMultiplier;
			_velocity.y = y;
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

		bool onSlope = Vector3.Dot(hit.normal, Vector3.up) != 1 ? true : false && Physics.Raycast(transform.position, Vector3.down, _characterContoller.height / 2f + 0.5f);

		if (hit.collider != null && hit.collider.tag == "Ice") _isOnIce = true;
		else _isOnIce = false;


		if (_isGrounded && _velocity.y <= 0)
		{
			_velocity.y = IdleGravity;
		}
		else
		{
			_velocity.y += (-Gravity) * Time.deltaTime;
		}

		if (_isGrounded && _velocity.y <= 0 && !_isOnIce)
		{
			// haha locked. such a pain. huh, what? what do you mean?
			_velocity.x = 0;
			_velocity.z = 0;
		}

		if (Mathf.Pow(_velocity.z, 2f) + Mathf.Pow(_velocity.x, 2f) > Mathf.Pow(MaxAirSpeed, 2f))
		{
			float _ = _velocity.y;

			_velocity = _velocity.normalized * MaxAirSpeed;

			_velocity.y = _;
		}

		_characterContoller.Move(_velocity * Time.deltaTime);

		if (_characterContoller.velocity.y <= 0 && _velocity.y > _characterContoller.velocity.y)
		{
			// may change later
			_velocity.y = _characterContoller.velocity.y;
		}
	}

	private void HandleJumping()
	{
		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Jump)))
		{
			if (_isGrounded)
			{
				// _velocity.y = Mathf.Sqrt(JumpHeight * -2f * (-Gravity));
				_jumpVector = new Vector3(finalMoveDir.x, Mathf.Sqrt(JumpHeight * -2f * (-Gravity)), finalMoveDir.z);
				// _characterContoller.Move(_jumpVector);

				// if (Mathf.Pow(_jumpVector.z, 2f) + Mathf.Pow(_jumpVector.x, 2f) > Mathf.Pow(MaxAirSpeed, 2f))
				// {
				// 	float _ = _jumpVector.y;

				// 	_jumpVector = _jumpVector.normalized * MaxAirSpeed;

				// 	_jumpVector.y = _;
				// }

				_velocity += _jumpVector;
			}
		}

		// if (!_isGrounded)
		// {
		// 	// _jumpVector += _velocity;
		// 	_velocity.x = _jumpVector.x;
		// 	_velocity.z = _jumpVector.z;
		// }
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
		_cameraHolderTransform.localPosition = new Vector3(0, y, 0);
	}

	// void OnDrawGizmos()
	// {
	// 	// Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - (_characterContoller.height / 2f) + _characterContoller.radius / 2f, transform.position.z), _characterContoller.radius / 4f);
	// }

	private void HandleGroundCheck()
	{

		//Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - ((_characterContoller.height / 2f) + _characterContoller.radius / 2f), transform.position.z), _characterContoller.radius / 4f, ~(1 << 6))
		//Physics.Raycast(transform.position, -transform.up, (_characterContoller.height / 2f) + 0.1f)
		if (_characterContoller.isGrounded || Physics.Raycast(transform.position, -transform.up, (_characterContoller.height / 2f) + 0.1f))
		{
			_isGrounded = true;
		}
		else
		{
			_isGrounded = false;
		}

	}
}
