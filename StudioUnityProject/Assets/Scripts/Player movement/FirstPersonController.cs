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
	[SerializeField] private float jumpForce = 5.0f;
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
	private CharacterController characterController;

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

		RaycastHit hit;
		bool raycastCrouch = Physics.Raycast(transform.position, Vector3.up, out hit, 0.85f / 2f);

		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Crouch)) && characterController.isGrounded)
		{
			isCrouched = true;

			characterController.height = _yCrouch;
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.425f, transform.position.z);

		}
		else if (raycastCrouch == true)
		{
			characterController.height = _yCrouch;
			isCrouched = false;
			underObject = true;
		}

		if (Input.GetKeyUp(InputManager.GetKey(InputActions.KeyAction.Crouch)))
		{
			characterController.height = _yNorm;
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.425f, transform.position.z);
			underObject = false;
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
		float speedDivider = Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Crouch)) ? crouchDivider : 1f;

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

		float verticalSpeed = (zFrw + zBac) * walkSpeed * speedMultiplier / speedDivider;
		float horizontalSpeed = (xFrw + xBac) * walkSpeed * speedMultiplier / speedDivider;

		Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
		horizontalMovement = transform.rotation * horizontalMovement;

		HandleGravityAndJumping();

		currentMovement.x = horizontalMovement.x;
		currentMovement.z = horizontalMovement.z;

		characterController.Move(currentMovement.normalized * Time.deltaTime);
	}

	void HandleGravityAndJumping()
	{

		if (characterController.isGrounded)
		{
			currentMovement.y = -0.5f;

			if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Jump)))
			{
				currentMovement.y = jumpForce;
				if (isCrouched == true)
				{
					characterController.height = _yNorm;
					transform.position = new Vector3(transform.position.x, transform.position.y + 0.425f, transform.position.z);
				}
			}
		}
		else
		{
			currentMovement.y -= gravity * Time.deltaTime;
		}
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
