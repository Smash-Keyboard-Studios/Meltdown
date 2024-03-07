using UnityEngine;

public class IceCubeSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;  // object that spawns



    public int spawnedObjCount = 0;         // amount of objects in the scene
    public int spawnedObjMax = 1;             // max number of objects to spawn
    //private static float time = 0.0f;
    public void SpawnCube() 
    {
        if (spawnedObjCount < spawnedObjMax)

        {
            GameObject spawnedObj = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity); // spawns object you set
            spawnedObj.GetComponent<IceBlockDestroyer>().iceCubeSpawner = this;
            spawnedObjCount++; // adds count to object limit

        }
        else
        {

        }
    }




    public void ActivateIceCubeSpawner(int CubeCount) // activated by button script
    {
        for (int i = 1; i <= CubeCount; i++)  // cube count can be changed in the button script get interact()
        {
            if (spawnedObjCount < spawnedObjMax)

            {
                GameObject spawnedObj = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity); // spawns object you set
                spawnedObj.GetComponent<IceBlockDestroyer>().iceCubeSpawner = this;
                spawnedObjCount++;
                Debug.Log("object count plus plus" + spawnedObjCount);
            }
            else
            {
                Debug.Log("object count is maxxed out" + spawnedObjCount);
            }
        }
    }
    public void RemoveCube()
    {
        Debug.Log("minus minus");
        spawnedObjCount--;
    }


}
