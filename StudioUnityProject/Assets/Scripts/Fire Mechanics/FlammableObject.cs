using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //if hit by fire
        if (other.gameObject.GetComponent<Fire>() != null)
        {
            //set gameobject to inactive
            //this can then be set active again when the level restarts
            gameObject.SetActive(false);
        }
    }
}
