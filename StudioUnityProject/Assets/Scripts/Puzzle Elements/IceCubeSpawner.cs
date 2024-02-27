using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using UnityEngine;

public class IceCubeSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;
  
    public void ActivateIceCubeSpawner(int CubeCount) // activated by button script
    {
        for (int i = 0; i < CubeCount; i++)  // cube count can be changed in the button script get interact()
        {
            Instantiate(ObjectToSpawn, transform.position, Quaternion.identity); // spawns object you set
        }
        
    }

        
    



}
