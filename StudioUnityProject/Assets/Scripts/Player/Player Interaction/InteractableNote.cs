using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableNote : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteract;

    public void Interact()
    {
        Debug.Log("I hate this");
        OnInteract.Invoke();
    }
}
