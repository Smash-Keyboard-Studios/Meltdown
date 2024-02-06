using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractiveObject : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        Debug.Log("Object interacted with"); // Just testing debug - shows that the object has been interacted with.
    }
}
