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

	public float Gravity = -9.81f;

	public float JumpHeight = 1.4f;

	public float AirMovementMultiplyer = 0.2f;

	public float GroundResistance = 0.2f;
	public float AirResistance = 0f;


	private CharacterController _characterContoller;

	private Vector3 _velocity;

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

	private float _resistance = 0;

	private bool _isOnSlope = false;
	RaycastHit slopeHit;


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

		_isOnSlope = IsOnSlope();


		HandleMovement();
		HandleJumping();

		HandleCrouching();


		HandleResistance();

		_characterContoller.Move(_velocity * Time.deltaTime);

	}

	private void HandleMovement()
	{
		// redo with new input system.
		float xAxis = Input.GetAxisRaw("Horizontal");
		float zAxis = Input.GetAxisRaw("Vertical");

		Vector3 moveDirection = transform.right * xAxis + transform.forward * zAxis;

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

		if (_isOnSlope)
		{
			moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
		}

		_velocity += moveDirection * (_isGrounded ? 1 : AirMovementMultiplyer) * _speed * _crouchSpeedScale;

		float yVel = _velocity.y;
		_velocity.y = 0;

		if (_velocity.magnitude > _speed)
		{
			_velocity = _velocity.normalized * _speed;
		}

		_velocity.y = yVel;
	}

	private void HandleResistance()
	{
		// can reduce
		if (_isGrounded && !_isOnSlope)
		{
			_resistance = GroundResistance;
			_velocity += -_velocity * _resistance;
		}
		else if (!_isGrounded && !_isOnSlope)
		{
			_resistance = AirResistance;
			_velocity += -_velocity * _resistance;
		}
		else if (_isOnSlope)
		{
			_resistance = GroundResistance;
			// print(Vector3.ProjectOnPlane(_velocity, slopeHit.normal));
			_velocity += -_velocity * _resistance;
		}


	}

	private void HandleGravity()
	{
		if (_isGrounded && !_isOnSlope && _velocity.y <= 0)
		{
			_velocity.y = -2f;
		}
		else if (!_isOnSlope)
		{
			_velocity.y += Gravity * Time.deltaTime;
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
			if (_isGrounded && !_isOnSlope)
			{
				_velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity * _resistance);
			}
			else if (_isOnSlope)
			{
				_velocity += (transform.up + slopeHit.normal) * Mathf.Sqrt(JumpHeight * -2f * Gravity * _resistance);
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

	private bool IsOnSlope()
	{
		if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, _characterContoller.height)) // might need to make this value scale with player later
		{
			if (slopeHit.normal != Vector3.up)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
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
