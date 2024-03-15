using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IceElementalButton : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent OnActivate;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Ice>() != null)
        {
            Debug.Log("Collider has hit Ice");
            OnActivate.Invoke();

        }
    }
}
