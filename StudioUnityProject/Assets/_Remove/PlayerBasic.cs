using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerBasic : MonoBehaviour
{
	private CharacterController cc;

	private Transform camHolder;

	public float walkspeed = 1f;
	public float runspeed = 1f;

	public float speed = 1f;

	public bool Grounded;

	public Vector3 velocity;
	public Vector3 mouse;

	public float gravity = 9.81f;

	public float Sens = 2f;

	public bool LockMouse = false;

	// Start is called before the first frame update
	void Start()
	{

		if (LockMouse)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		cc = GetComponent<CharacterController>();

		camHolder = GameObject.Find("cam holder").transform;




	}
	// this code is awful!
	// seperte later...

	// Update is called once per frame
	void Update()
	{
		if (LockMouse)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		Vector3 moveDir = Vector3.zero;

		float xLeftAxis = 0f;
		float xRightAxis = 0f;

		if (Input.GetKey(InputManager.Instance.GetKey(InputActions.KeyType.Left)))
		{
			xLeftAxis = -1f;
		}

		if (Input.GetKey(InputManager.Instance.GetKey(InputActions.KeyType.Right)))
		{
			xRightAxis = 1f;
		}

		float zFrwAxis = 0f;
		float zBakAxis = 0f;

		if (Input.GetKey(InputManager.Instance.GetKey(InputActions.KeyType.Forward)))
		{
			zFrwAxis = 1f;
		}

		if (Input.GetKey(InputManager.Instance.GetKey(InputActions.KeyType.Backward)))
		{
			zBakAxis = -1f;
		}

		moveDir.x = xRightAxis + xLeftAxis;
		moveDir.z = zFrwAxis + zBakAxis;

		//moveDir.z = Input.GetAxisRaw("Vertical");

		moveDir = transform.right * moveDir.x + transform.forward * moveDir.z;

		moveDir.Normalize();



		if (Input.GetKey(KeyCode.LeftShift))
		{
			speed = runspeed;
		}
		else speed = walkspeed;

		cc.Move(moveDir * 0.01f * speed);

		if (cc.isGrounded || Physics.Raycast(transform.position, Vector3.down, 1.1f))
		{
			Grounded = true;
		}
		else
		{
			Grounded = false;
		}

		HandleLook();

		HandleGravity();

	}

	void HandleGravity()
	{
		if (Grounded)
		{
			velocity.y = -2f;
		}
		else
		{
			velocity.y -= gravity * Time.deltaTime;
		}

		cc.Move(velocity * Time.deltaTime);
	}

	void HandleLook()
	{
		mouse.x += Input.GetAxisRaw("Mouse X") * Sens;
		mouse.y += Input.GetAxisRaw("Mouse Y") * Sens;

		transform.rotation = Quaternion.Euler(0f, mouse.x, 0f);

		camHolder.transform.localRotation = Quaternion.Euler(Mathf.Clamp(-mouse.y, -90f, 90f), 0f, 0f);
	}

}
