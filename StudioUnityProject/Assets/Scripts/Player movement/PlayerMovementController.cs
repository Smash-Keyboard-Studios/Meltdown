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

	public float GroundResistance = 0.2f;
	public float AirResistance = 0f;
	public float Drag = 2f;

	public float Mass = 72.5f;


	private CharacterController _characterContoller;

	private Vector3 _moveVelocity;
	private Vector3 _gravityVelocity;
	private Vector3 _verticalVelocity;
	private Vector3 _velocity;

	private Vector3 _gravityVector;

	private Vector3 moveDirection;

	private Vector3 airRis;

	private Vector3 finalVector;

	private Vector3 acc;

	private Vector3 drag;

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

		if (Locked) return; // Stops movement
		HandleGroundCheck();



		HandleMovementDir();
		HandleJumping();

		HandleCrouching();

		HandleGravity();

		HandleResistance();

		Vector3 _targVelocity = _moveVelocity + _gravityVelocity + _verticalVelocity;

		_velocity += _targVelocity;

		_characterContoller.Move(_velocity * Time.deltaTime);

		if (_characterContoller.velocity.y <= 0 && _velocity.y > _characterContoller.velocity.y)
		{
			// may change later
			_velocity.y = _characterContoller.velocity.y;
		}

	}

	private void HandleMovementDir()
	{
		// redo with new input system.
		float xAxis = Input.GetAxisRaw("Horizontal");
		float zAxis = Input.GetAxisRaw("Vertical");

		moveDirection = -transform.right * xAxis + -transform.forward * zAxis;

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


		Vector3 wishDir = moveDirection * _speed * _crouchSpeedScale;

		_moveVelocity += wishDir - _moveVelocity * Time.deltaTime;

		if (!_isGrounded)
		{
			_moveVelocity = _moveVelocity * (_isGrounded ? 1 : AirMovementMultiplyer);
		}




		// float yVel = _velocity.y;
		// _velocity.y = 0;

		// if (_velocity.magnitude > _speed)
		// {
		// 	_velocity = _velocity.normalized * _speed;
		// }

		// _velocity.y = yVel;
	}

	private void HandleResistance()
	{
		// can reduce
		if (_isGrounded)
		{
			_resistance = GroundResistance;
			_moveVelocity += -_moveVelocity * _resistance;
		}
		else
		{
			_resistance = AirResistance;
			_moveVelocity += -_moveVelocity * _resistance;
		}

		drag = -_velocity * Mass * Drag;

		// // can reduce
		// if (_isGrounded && !_isOnSlope)
		// {
		// 	_resistance = GroundResistance;
		// 	_velocity += -_velocity * _resistance;
		// }
		// else if (!_isGrounded && !_isOnSlope)
		// {
		// 	_resistance = AirResistance;
		// 	_velocity += -_velocity * _resistance;
		// }
		// else if (_isOnSlope)
		// {
		// 	_resistance = GroundResistance;
		// 	// print(Vector3.ProjectOnPlane(_velocity, slopeHit.normal));
		// 	_velocity += -_velocity * _resistance;
		// }


	}


	private void HandleGravity()
	{
		// if (_isGrounded && _velocity.y <= 0)
		// {
		// 	_gravityVelocity.y += -2f - _gravityVelocity.y;
		// }
		// else
		// {
		// 	_gravityVelocity.y += -Gravity * Mass * Time.deltaTime;
		// }

		_gravityVelocity.y = -Gravity;

		//_characterContoller.Move(_moveVelocity * Time.deltaTime);
	}

	private void Force(Vector3 dir)
	{
		_velocity += dir / Mass;
	}

	private void Accelerate(Vector3 dir)
	{
		_velocity += (dir * Mass) * Time.deltaTime;
	}

	private void SetVelocity(Vector3 vel)
	{
		_velocity = vel;
	}

	private void HandleJumping()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_isGrounded)
			{
				_verticalVelocity.y = Mathf.Sqrt(JumpHeight * -2f * -Gravity * Mass);
			}
		}
		else
		{
			if (_verticalVelocity != Vector3.zero && _verticalVelocity.y > 0)
			{
				_verticalVelocity += Vector3.zero - _verticalVelocity * Time.deltaTime * Gravity;
			}
			else if (_verticalVelocity != Vector3.zero && _verticalVelocity.y < 0)
			{
				_verticalVelocity = Vector3.zero;
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
