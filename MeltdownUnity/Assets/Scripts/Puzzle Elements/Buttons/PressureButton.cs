using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private bool buttonActive;


    public UnityEvent OnButtonPress;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null || collision.gameObject.GetComponent<PlayerMovementController>() != null)
        {
            OnButtonPress.Invoke();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null || collision.gameObject.GetComponent<PlayerMovementController>() != null)
        {
            buttonActive = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        buttonActive = false;
        OnButtonPress.Invoke();
    }
}
