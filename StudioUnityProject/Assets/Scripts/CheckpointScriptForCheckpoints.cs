using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScriptForCheckpoints : MonoBehaviour
{
    private int OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            
        }
            return 0;
    }
}
