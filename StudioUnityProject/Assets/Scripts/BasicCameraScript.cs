using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraScript : MonoBehaviour
{

    public float MouseSensitivity = 100;

    public Transform playerBody;

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime; // Gets the X/Y movement components of the player

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents the player turning the camera all the way through their body

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * MouseX); // Moves the player's body in line with the mouse rotation
    }
}
