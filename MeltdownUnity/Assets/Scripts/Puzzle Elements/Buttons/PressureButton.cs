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
        print("i want to be free");
        if(collision.gameObject.GetComponent<CharacterController>() != null || collision.gameObject.GetComponent<Rigidbody>() || collision.gameObject.GetComponent<FirstPersonController>() != null)
        {
            OnButtonPress.Invoke();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        buttonActive = false;
        OnButtonPress.Invoke();
    }
}
