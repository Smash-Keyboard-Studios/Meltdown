using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingWater : MonoBehaviour
{
    public GameObject IceCube;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ice>() != null)
        {
            //create instance of ice cube at hit point
            Instantiate(IceCube, other.transform.position, Quaternion.identity);
        }
    }
}
