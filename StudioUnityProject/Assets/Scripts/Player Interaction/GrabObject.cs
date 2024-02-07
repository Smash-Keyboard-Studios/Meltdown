using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{


    private Rigidbody rb;
    private Transform objectGrabPoint;

    private void Awake()
    {
        rb  = GetComponent<Rigidbody>();
    }
    public void Grab(Transform objectGrabPoint)
    {
        Debug.Log("AAAAAAAAAAAADDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
        this.objectGrabPoint = objectGrabPoint;
    }

    private void FixedUpdate()
    {
      if(objectGrabPoint != null)
        {
            rb.MovePosition(objectGrabPoint.position);
        }
    }
}
