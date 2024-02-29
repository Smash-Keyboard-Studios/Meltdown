using Codice.Client.Common;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IceCubeSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;  // object that spawns
    public GameObject SpawningPipe; // pipe that rotates and spawns from
    //private static float time = 0.0f;
    

    public void ActivateIceCubeSpawner(int CubeCount) // activated by button script
    {
        for (int i = 0; i < CubeCount; i++)  // cube count can be changed in the button script get interact()
        {
            Instantiate(ObjectToSpawn, transform.position, Quaternion.identity); // spawns object you set
            while (SpawningPipe.transform.rotation.eulerAngles.z <= 180)
            {
                //SpawningPipe.transform.Rotate(new Vector3(0f, 0f, -0.1f * Time.deltaTime));   // failed
                SpawningPipe.transform.Rotate(0f, 0f, 1f * UnityEngine.Time.deltaTime);


                //SpawningPipe.transform.eulerAngles = new Vector3(0, 0, 10);   // failed
                //SpawningPipe.transform.rotation = new Vector3(0f, 0f, Mathf.Lerp(90f, 180f, time));   // failed
                //time += 0.5f * Time.deltaTime;   // failed
            }
        }




    }



        
    



}
