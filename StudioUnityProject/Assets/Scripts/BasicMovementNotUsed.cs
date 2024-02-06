using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BasicMovementNotUsed : MonoBehaviour
{
    public CharacterController characterController;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;

    Vector3 Velocity;
    bool isGrounded;


    public float crouchSpeed = 6f;
    public float crouchYScale;
    public float crouchYPosition;
    public float OriginalYScale;
    public float OriginalYPosition;
    bool isCrouched;

    private void Start()
    {
        OriginalYScale = transform.localScale.y; // Original player size
        crouchYScale = transform.localScale.y - 0.8f; // Crouched player size
    }

    void Update()
    {


        if (characterController.isGrounded) // Checks if player is grounded using the CharacterController then sets velocity to 0
        {
            Velocity.y = 0f;
        }


        float xCoord = Input.GetAxis("Horizontal"); // Checks whether or not the player is pressing W or S then transforms that into a value (1, -1) on the horizontal axis (forward, backward in this case)
        float zCoord = Input.GetAxis("Vertical"); // Checks whether or not the player is pressing A or D then transforms that into a value (1, -1) on the vertical axis (left, right in this case)

        Vector3 move = transform.right * xCoord + transform.forward * zCoord; // Creates the new movement coordinates

        characterController.Move(move * speed * Time.deltaTime); // Moves the player in line with the amount of time since last frame, ensuring that movement is not frame-rate dependent



        if (Input.GetKeyDown(KeyCode.Space))
        {
            Velocity.y = Mathf.Sqrt(6 * -2 * gravity); // Jump height formulae
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x, crouchYScale, transform.localPosition.z); // Just changes the height of the player then moves them to that height to avoid any like weird stuff with it

        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            transform.localPosition = new Vector3(transform.localPosition.x, OriginalYScale, transform.localPosition.z);
            transform.localScale = new Vector3(transform.localScale.x, OriginalYScale, transform.localScale.z); // Same as above but opposite
        }
        Velocity.y += gravity * 2 * Time.deltaTime; // Increases the downwards velcoity of the player at a steady rate till they actually hit the ground

        characterController.Move(Velocity * Time.deltaTime);
    }
}
