using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    private Vector3 crouchScale = new Vector3(0.5f, 0.425f, 0.5f);
    private Vector3 playerScale = new Vector3(0.5f, 0.85f, 0.5f);

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

    [Header("Inputs Customisation")]
    [SerializeField] private string horizontalMoveInput = "Horizontal";
    [SerializeField] private string verticalMoveInput = "Vertical";
    [SerializeField] private string MouseXInput = "Mouse X";
    [SerializeField] private string MouseYInput = "Mouse Y";
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    private Camera mainCamera;
    private float verticalRotation;
    private Vector3 currentMovement = Vector3.zero;
    private CharacterController characterController;

    private bool isCrouched;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.425f, transform.position.z);

            isCrouched = true;
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.425f, transform.position.z);

            isCrouched = false;
        }
    }

    void HandleMovement()
    {
        float speedMultiplier = Input.GetKey(sprintKey) ? sprintMultiplier : 1f;
        float speedDivider = Input.GetKey(crouchKey) ? crouchDivider : 1f;

        float verticalSpeed = Input.GetAxis(verticalMoveInput) * walkSpeed * speedMultiplier / speedDivider;
        float horizontalSpeed = Input.GetAxis(horizontalMoveInput) * walkSpeed * speedMultiplier / speedDivider;

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;

        HandleGravityAndJumping();

        currentMovement.x = horizontalMovement.x;
        currentMovement.z = horizontalMovement.z;

        characterController.Move(currentMovement * Time.deltaTime);
    }

    void HandleGravityAndJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (Input.GetKeyDown(jumpKey))
            {
                currentMovement.y = jumpForce;

                if (isCrouched == true)
                {
                    transform.localScale = playerScale;
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
        float mouseXRotation = Input.GetAxis(MouseXInput) * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= Input.GetAxis(MouseYInput) * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -downRange, upRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
