using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIceBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if( collision.transform.CompareTag("IceBlock"))
        {
            Debug.Log("IceBlock Hit");
            Destroy(collision.gameObject);
        }
    }
}
