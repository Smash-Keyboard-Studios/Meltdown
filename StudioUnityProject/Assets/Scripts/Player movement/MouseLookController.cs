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

	// Start is called before the first frame update
	void Start()
	{
		// what is this? what is this bro? :face_vomiting:
		_playerBody = GetComponent<Transform>();

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
		_yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

		// could put sensitivity here, like ved2D * sens

		_playerBody.Rotate(Vector3.up * lookDirection.x * Sensitivity);

		CameraHolder.localRotation = Quaternion.Euler(_yRotation * Sensitivity, 0, 0);


	}
}
