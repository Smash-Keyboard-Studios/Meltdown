using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private bool buttonActive;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody>() != null || collision.gameObject.GetComponent<CharacterController>() != null)
        {
            buttonActive = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null || collision.gameObject.GetComponent<CharacterController>() != null)
        {
            buttonActive = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        buttonActive = false;
    }
}
