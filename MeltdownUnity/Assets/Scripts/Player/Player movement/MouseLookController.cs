using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookController : MonoBehaviour
{
	[Header("Look locking")]

	[Tooltip("Stops the player from being able to look around")]
	public bool Locked = false;

	[Header("Variables that need to be set")]

	[Tooltip("We need this to rotate up and down")]
	public Transform CameraHolder;


	[Header("Head Bob")]

	[Tooltip("The object we move for bobbing")]
	public Transform HeadBobObject;

	[Tooltip("How much we move left and right")]
	public float HeadBobHorizontalAmplitude;

	[Tooltip("How much we move up and down")]
	public float HeadBobVerticalAmplitude;

	[Tooltip("How much we bob")]
	public float MaxBobFreq = 4.5f;

	[Range(0, 1), Tooltip("Lerp smoothing")] public float HeadBobSmoothing;


	// storing stuff
	private float _headBobFrequency;
	private float _walkingTime;
	private Vector3 targetCameraPosition;

	private PlayerMovementController movementController;
	private CharacterController characterController;

	private float _sensitivity = 1f;
	private Transform _playerBody;
	private float _yRotation;


	void Start()
	{
		// what is this? what is this bro? :face_vomiting:
		// yeah, like, you can just use transform.
		_playerBody = GetComponent<Transform>();

		// get other components.
		movementController = GetComponent<PlayerMovementController>();
		characterController = GetComponent<CharacterController>();

		// we set the sensitivity.
		if (SaveManager.current != null)
		{
			SaveManager.current.ForceLoad();
			_sensitivity = SaveData.Current.Sensitivity;
		}

		// stops frustrating mistakse.
		Locked = false;
	}

	void Update()
	{
		HandleLook();

		// we start head bob.
		MainHeadBobbing();
	}

	private void HandleLook()
	{
		// if we are locked than dont do anything.
		if (Locked) return;

		// create a new vector 2.
		Vector2 lookDirection = new();

		// we store the mouse movements.
		lookDirection.x = Input.GetAxisRaw("Mouse X");
		lookDirection.y = Input.GetAxisRaw("Mouse Y");

		// we invert because screens start are top left, so positive means down.
		_yRotation -= lookDirection.y;

		// we clamp the rotation. obvious why we do this.
		_yRotation = Mathf.Clamp(_yRotation, -80f, 90f);

		// we rotate the body.
		_playerBody.Rotate(Vector3.up * lookDirection.x * _sensitivity);

		// we rotate the cam holder. (child of the player)
		CameraHolder.localRotation = Quaternion.Euler(_yRotation * _sensitivity, 0, 0);
	}

	// I refuse to explain the math behind this.
	private void MainHeadBobbing()
	{
		// we use the character controller because if we are running into a wall, we wont bob.
		Vector3 MovementVel = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
		if (MovementVel.magnitude > 0 && movementController.isGrounded) _walkingTime += Time.deltaTime;
		else _walkingTime = 0f;

		_headBobFrequency = (movementController.speed > MaxBobFreq) ? 1f * movementController.speed : MaxBobFreq;

		targetCameraPosition = CameraHolder.position + CalculateHeadBobOffset(_walkingTime);

		HeadBobObject.position = Vector3.Lerp(HeadBobObject.transform.position, targetCameraPosition, HeadBobSmoothing);

		if ((HeadBobObject.position - targetCameraPosition).magnitude <= 0.001) HeadBobObject.position = targetCameraPosition;
	}

	private Vector3 CalculateHeadBobOffset(float t)
	{
		float horOffset = 0f;
		float vertOffset = 0f;
		Vector3 Offset = Vector3.zero;

		if (t > 0)
		{
			horOffset = Mathf.Cos(t * _headBobFrequency) * HeadBobHorizontalAmplitude * characterController.height / 2;
			vertOffset = Mathf.Sin(t * _headBobFrequency * 2f) * HeadBobVerticalAmplitude * characterController.height / 2;

			Offset = CameraHolder.right * horOffset + CameraHolder.up * vertOffset;
		}

		return Offset;
	}
}
