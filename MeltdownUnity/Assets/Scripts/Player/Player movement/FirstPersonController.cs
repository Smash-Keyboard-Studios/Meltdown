using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

	private float _yNorm = 1.75f;
	private float _yCrouch = 1.75f / 2f;

	[Header("Movement Speeds")]
	[SerializeField] private float walkSpeed = 1.5f;
	[SerializeField] private float sprintMultiplier = 2.0f;
	[SerializeField] private float crouchDivider = 2.0f;

	[Header("Jump Parmeters")]
	[SerializeField] private float jumpHeight = 5.0f;
	[SerializeField] private float gravity = 9.81f;

	[Header("Look Sensitivity")]
	[SerializeField] private float mouseSensitivity = 2.0f;
	[SerializeField] private float upRange = 90.0f;
	[SerializeField] private float downRange = 80.0f;

	// [Header("Inputs Customisation")]
	// [SerializeField] private string horizontalMoveInput = "Horizontal";
	// [SerializeField] private string verticalMoveInput = "Vertical";
	// [SerializeField] private string MouseXInput = "Mouse X";
	// [SerializeField] private string MouseYInput = "Mouse Y";
	// [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
	// [SerializeField] private KeyCode jumpKey = KeyCode.Space;
	// [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

	[SerializeField] private float range = 2f;

	private Camera mainCamera;
	private float verticalRotation;
	private Vector3 currentMovement = Vector3.zero;
	private Vector3 _velocity = Vector3.zero;
	private CharacterController characterController;

	private bool _isGrounded;
	private bool isCrouched;
	private bool underObject;

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
		mainCamera = Camera.main;

	}

	private void Update()
	{
		HandleMovement();
		HandleRotation();
		HandleGravity();
		HandleGroundCheck();

		RaycastHit hit;
		bool raycastCrouch = Physics.Raycast(transform.position, Vector3.up, out hit, 0.85f / 2f);

		// when the player presses the crouch key.
		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Crouch)) && _isGrounded)
		{
			isCrouched = true;

			characterController.height = _yCrouch;
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.425f, transform.position.z);

		}
		// also do raycast with cc grounded because cc grounded only works when the player moves.
		else if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Crouch)) && !_isGrounded)
		{
			isCrouched = true;

			characterController.height = _yCrouch;
			//transform.position = new Vector3(transform.position.x, transform.position.y - 0.425f, transform.position.z);

		}
		else if (raycastCrouch == true && _isGrounded)
		{
			characterController.height = _yCrouch;
			isCrouched = false;
			underObject = true;
		}

		// when the player lets go of the crouch key.
		if (Input.GetKeyUp(InputManager.GetKey(InputActions.KeyAction.Crouch)))
		{
			characterController.height = _yNorm;
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.425f, transform.position.z);
			underObject = false;
			isCrouched = false;
		}
		else if (isCrouched == false && raycastCrouch == false)
		{
			characterController.height = _yNorm;
			underObject = false;
		}
	}

	void HandleMovement()
	{
		// huh?
		if (isCrouched == true || underObject == true)
		{
			sprintMultiplier = 1.0f;
		}
		else
		{
			sprintMultiplier = 2.0f;
		}


		float speedMultiplier = Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Sprint)) ? sprintMultiplier : 1f;
		float speedDivider = isCrouched || underObject ? crouchDivider : 1f;

		float zFrw = 0;
		float zBac = 0;

		float xFrw = 0;
		float xBac = 0;

		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Forward)))
		{
			zFrw = 1;
		}

		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Backward)))
		{
			zBac = -1;
		}

		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Left)))
		{
			xBac = -1;
		}

		if (Input.GetKey(InputManager.GetKey(InputActions.KeyAction.Right)))
		{
			xFrw = 1;
		}

		float verticalSpeed = (zFrw + zBac);
		float horizontalSpeed = (xFrw + xBac);

		Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
		horizontalMovement = transform.rotation * horizontalMovement;
		currentMovement = horizontalMovement.normalized * walkSpeed * (speedMultiplier / speedDivider);

		HandleGravityAndJumping();


		characterController.Move(currentMovement * Time.deltaTime);
	}

	void HandleGravityAndJumping()
	{
		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Jump)) && _isGrounded)
		{

			_velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
			_velocity.y = jumpHeight;

			if (isCrouched && !underObject)
			{
				isCrouched = false;
				characterController.height = _yNorm;
				transform.position = new Vector3(transform.position.x, transform.position.y + 0.425f, transform.position.z);
			}
		}
	}

	private void HandleGroundCheck()
	{
		// ~(1 << 3) layer mask thing
		if (characterController.isGrounded || Physics.Raycast(transform.position, -transform.up, 1.1f))
		{
			_isGrounded = true;
		}
		else
		{
			_isGrounded = false;
		}

	}


	private void HandleGravity()
	{
		if (_isGrounded && _velocity.y < 0)
		{
			_velocity.y = -2f;
		}
		else
		{
			_velocity.y += -gravity * Time.deltaTime;
		}

		characterController.Move(_velocity * Time.deltaTime);
	}

	void HandleRotation()
	{
		float mouseXRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, mouseXRotation, 0);

		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -downRange, upRange);
		mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
	}
}
