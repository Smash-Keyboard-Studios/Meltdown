using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireElementalButton : MonoBehaviour
{
    public UnityEvent OnActivate;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Fire>() != null)
        {
            Debug.Log("Collider has hit Fire");
            OnActivate.Invoke();
            
        }
    }
}
