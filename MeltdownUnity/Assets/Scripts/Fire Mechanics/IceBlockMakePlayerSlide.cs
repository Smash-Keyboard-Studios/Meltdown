using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockMakePlayerSlide : MonoBehaviour
{
    private float OriginalWalkSpeed;
    private float OriginalSprintSpeed;

    private float SlideyWalkSpeed;
    private float SlideySprintSpeed;

    public GameObject PlayerObj;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.FindGameObjectWithTag("Player");

        OriginalWalkSpeed = PlayerObj.GetComponent<PlayerMovementController>().WalkSpeed;
        OriginalSprintSpeed = PlayerObj.GetComponent<PlayerMovementController>().SprintSpeed;

        SlideyWalkSpeed = OriginalWalkSpeed * 2;
        SlideySprintSpeed = OriginalSprintSpeed * 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerObj.GetComponent<PlayerMovementController>().WalkSpeed = SlideyWalkSpeed;
            PlayerObj.GetComponent<PlayerMovementController>().SprintSpeed = SlideySprintSpeed;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerObj.GetComponent<PlayerMovementController>().WalkSpeed = OriginalWalkSpeed;
            PlayerObj.GetComponent<PlayerMovementController>().SprintSpeed = OriginalSprintSpeed;
        }
    }
}
