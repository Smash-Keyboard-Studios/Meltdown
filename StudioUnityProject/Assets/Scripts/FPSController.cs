using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float mouseSensitivity = 2.0f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float verticalRotation = 0;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        // Lock cursor to center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse input for looking around
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

       
        transform.Rotate(0, mouseX, 0);

        // Rotate the camera vertically (limited to avoid flipping)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        if (controller.isGrounded)
        {
            // Get player input for movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate movement direction
            moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            // Jump
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        //gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the player
        controller.Move(moveDirection * Time.deltaTime);
    }
}