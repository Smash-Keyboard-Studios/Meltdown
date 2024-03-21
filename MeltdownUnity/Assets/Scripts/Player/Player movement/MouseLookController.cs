using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookController : MonoBehaviour
{
	[HideInInspector] // no need to touch this
	public bool Locked = false;


	public Transform CameraHolder;

	public float Sensitivity = 1f;


	private Transform _playerBody;

	private float _yRotation;


	// head bobing
	[Header("Head Bob")]
	public Transform HeadBob;
	public float headBobHorizontalAmplitude;
	public float headBobVerticalAmplitude;
	public float maxBobFreq = 4.5f;
	[Range(0, 1)] public float headBobSmoothing;

	private float xRotation;
	private float yRotation;
	private float headBobFrequency;
	private float walkingTime;
	private Vector3 targetCameraPosition;

	private PlayerMovementController movementController;
	private CharacterController characterController;



	// Start is called before the first frame update
	void Start()
	{
		// what is this? what is this bro? :face_vomiting:
		_playerBody = GetComponent<Transform>();

		movementController = GetComponent<PlayerMovementController>();
		characterController = GetComponent<CharacterController>();

		Locked = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Locked) return;

		Vector2 lookDirection = new();

		lookDirection.x = Input.GetAxisRaw("Mouse X");
		lookDirection.y = Input.GetAxisRaw("Mouse Y");

		_yRotation -= lookDirection.y;
		_yRotation = Mathf.Clamp(_yRotation, -80f, 90f);

		// could put sensitivity here, like ved2D * sens

		_playerBody.Rotate(Vector3.up * lookDirection.x * Sensitivity);


		CameraHolder.localRotation = Quaternion.Euler(_yRotation * Sensitivity, 0, 0);

		MainHeadBobbing();
	}




	private void MainHeadBobbing()
	{
		Vector3 MovementVel = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);
		if (MovementVel.magnitude > 0 && movementController.isGrounded) walkingTime += Time.deltaTime;
		else walkingTime = 0f;

		headBobFrequency = (movementController.speed > maxBobFreq) ? 1f * movementController.speed : maxBobFreq;


		targetCameraPosition = CameraHolder.position + CalculateHeadBobOffset(walkingTime);

		HeadBob.position = Vector3.Lerp(HeadBob.transform.position, targetCameraPosition, headBobSmoothing);

		if ((HeadBob.position - targetCameraPosition).magnitude <= 0.001) HeadBob.position = targetCameraPosition;
	}

	private Vector3 CalculateHeadBobOffset(float t)
	{
		float horOffset = 0f;
		float vertOffset = 0f;
		Vector3 Offset = Vector3.zero;

		if (t > 0)
		{
			horOffset = Mathf.Cos(t * headBobFrequency) * headBobHorizontalAmplitude * characterController.height / 2;
			vertOffset = Mathf.Sin(t * headBobFrequency * 2f) * headBobVerticalAmplitude * characterController.height / 2;

			Offset = CameraHolder.right * horOffset + CameraHolder.up * vertOffset;
		}

		return Offset;
	}
}
