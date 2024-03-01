using Codice.Client.Common;
using Codice.Client.Common.GameUI;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class IceCubeSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;  // object that spawns
    public GameObject spawnedObj;
    public GameObject SpawningPipe; // pipe that rotates and spawns from

    public int spawnedObjCount = 0;         // amount of objects in the scene
    public int spawnedObjMax = 1;             // max number of objects to spawn
    //private static float time = 0.0f;
    
    


    
    public void ActivateIceCubeSpawner(int CubeCount) // activated by button script
    {
        for (int i = 0; i < CubeCount; i++)  // cube count can be changed in the button script get interact()
        {
            if (spawnedObjCount < spawnedObjMax) 

            {
                spawnedObj = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity); // spawns object you set
                spawnedObj.GetComponent<IceBlockDestroyer>().iceCubeSpawner = this;
                spawnedObjCount ++;
                Debug.Log("object count plus plus" + spawnedObjCount);
            }
            else
            {
                Debug.Log("object count is maxxed out" + spawnedObjCount);
            }

            
                


            
            

            while (SpawningPipe.transform.rotation.eulerAngles.z <= 180)
            {
                //SpawningPipe.transform.Rotate(new Vector3(0f, 0f, -0.1f * Time.deltaTime));   // failed
                
                SpawningPipe.transform.Rotate(0f, 0f, 1f * UnityEngine.Time.deltaTime);


                //SpawningPipe.transform.eulerAngles = new Vector3(0, 0, 10);   // failed
                //SpawningPipe.transform.rotation = new Vector3(0f, 0f, Mathf.Lerp(90f, 180f, time));   // failed
                //time += 0.5f * Time.deltaTime;   // failed
            }
            
        }

        if (spawnedObj.activeInHierarchy == false)
        {
            Debug.Log("minus minus");
            spawnedObjCount--;
        }

    }

    public void RemoveCube()
    {
        Debug.Log("minus minus");
        spawnedObjCount--;
    }


}
