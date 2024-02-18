using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
	[HideInInspector] // no need to touch this
	public bool Locked = false;


	public float Speed = 5f;
	public float SprintSpeed = 10f;

	public float Gravity = -9.81f;

	public float JumpHeight = 1.4f;


	private CharacterController _characterContoller;

	private Vector3 velocity;

	private bool isGrounded = false;


	// Start is called before the first frame update
	void Start()
	{
		_characterContoller = GetComponent<CharacterController>();

		Locked = false;
	}

	// Update is called once per frame
	void Update()
	{
		HandleGravity();

		if (Locked) return; // Stops movement
		HandleMovement();
		HandleJumping();
		HandleGroundCheck();


	}

	private void HandleMovement()
	{
		Vector3 moveDirection = Vector3.zero;

		moveDirection.x = Input.GetAxisRaw("Horizontal");
		moveDirection.z = Input.GetAxisRaw("Vertical");

		moveDirection = transform.right * moveDirection.x + transform.forward * moveDirection.z;

		moveDirection.Normalize();

		if (Input.GetKey(KeyCode.LeftShift))
		{
			_characterContoller.Move(moveDirection * SprintSpeed * Time.deltaTime);
		}
		else
		{
			_characterContoller.Move(moveDirection * Speed * Time.deltaTime);
		}
	}

	private void HandleGravity()
	{
		if (isGrounded && velocity.y <= 0)
		{
			velocity.y = -2f;
		}
		else
		{
			velocity.y += Gravity * Time.deltaTime;
		}

		_characterContoller.Move(velocity * Time.deltaTime);

		if (_characterContoller.velocity.y <= 0)
		{
			// may change later
			velocity.y = _characterContoller.velocity.y;
		}
	}

	private void HandleJumping()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			if (isGrounded)
			{
				velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
			}
		}
	}

	private void HandleGroundCheck()
	{
		if (_characterContoller.isGrounded || Physics.Raycast(transform.position, -transform.up, 1.1f, ~(1 << 3)))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}

	}
}
