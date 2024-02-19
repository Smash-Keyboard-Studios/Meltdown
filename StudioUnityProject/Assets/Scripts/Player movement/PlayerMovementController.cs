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

	public float JumpHeight = 1.4f;

	public float AirMovementMultiplyer = 0.2f;

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

	private float _playerStandingHeight = 1.75f;
	private float _playerCrouchingHeight;

	private Transform _cameraHolderTransform;

	private float _cameraStandingHeight = 0.6f;
	private float _cameraCrouchHeight;

	private bool _isSprinting = false;


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
		// float xAxis = Input.GetAxisRaw("Horizontal");
		// float zAxis = Input.GetAxisRaw("Vertical");

		// Vector3 moveDirection = transform.right * xAxis + transform.forward * zAxis;

		Vector3 moveDirection = Vector3.zero;

		moveDirection.x = Input.GetAxisRaw("Horizontal");
		moveDirection.z = Input.GetAxisRaw("Vertical");

		moveDirection = transform.right * moveDirection.x + transform.forward * moveDirection.z;

		moveDirection.Normalize();

		if (Input.GetKey(KeyCode.LeftShift) && _isGrounded)
		{
			_isSprinting = true;
		}
		else
		{
			_isSprinting = false;
		}


		if (_isSprinting)
		{
			_speed = SprintSpeed;
		}
		else
		{
			_speed = WalkSpeed;
		}

		if (_isGrounded)
		{
			finalMoveDir = moveDirection * _speed;
			_characterContoller.Move(moveDirection * _speed * Time.deltaTime);
		}
		else
		{
			float y = _velocity.y;
			if (moveDirection != Vector3.zero)
				_velocity += moveDirection * AirMovementMultiplyer;
			_velocity.y = y;
		}


	}


	private void HandleGravity()
	{
		RaycastHit hit;

		Physics.Raycast(transform.position, Vector3.down, out hit, _characterContoller.height / 2f + 0.2f);

		bool onSlope = Vector3.Dot(hit.normal, Vector3.up) != 1 ? true : false && Physics.Raycast(transform.position, Vector3.down, _characterContoller.height / 2f + 0.5f);



		if (_isGrounded && _velocity.y <= 0 && !onSlope)
		{
			_velocity.y = -2f;
		}
		else
		{
			_velocity.y += (-Gravity) * Time.deltaTime;
		}

		if (_isGrounded && _velocity.y <= 0)
		{
			// haha locked. such a pain.
			_velocity.x = 0;
			_velocity.z = 0;
		}

		if (Mathf.Pow(_velocity.z, 2f) + Mathf.Pow(_velocity.x, 2f) > Mathf.Pow(SprintSpeed, 2f))
		{
			float _ = _velocity.y;

			_velocity = _velocity.normalized * SprintSpeed;

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
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_isGrounded)
			{
				// _velocity.y = Mathf.Sqrt(JumpHeight * -2f * (-Gravity));
				_jumpVector = new Vector3(finalMoveDir.x, Mathf.Sqrt(JumpHeight * -2f * (-Gravity)), finalMoveDir.z);
				// _characterContoller.Move(_jumpVector);
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

		// TODO replace with new input system
		if (_isUnableToUncrouch)
		{
			_characterContoller.height = _playerCrouchingHeight;
			MoveCamY(_cameraCrouchHeight);

		}

		if (Input.GetKey(KeyCode.C))
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

	private void HandleGroundCheck()
	{
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
