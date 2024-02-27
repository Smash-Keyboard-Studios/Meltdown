using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; 
    public float spawnForce = 5f; 
    public float maxDistance = 2f;  //distance player has to be at in relation to the cube spawner
    public string playerTag = "Player"; 

    // void Update()
    // {
       
    //     if (Input.GetKeyDown(KeyCode.E) && IsPlayerClose())   //basically checking whether the player is close and the player has pressed the correct interact button
    //     {
    //         SpawnCube();
    //     }
    // }

    bool IsPlayerClose()  //checks whether the player is close enough
    {
        
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        foreach (GameObject player in players)
        {
            
            float distance = Vector3.Distance(player.transform.position, transform.position);

            
            if (distance <= maxDistance)
            {
                return true;
            }
        }

        return false;
    }

    public void SpawnCube()  
    {
        
        GameObject cube = Instantiate(cubePrefab, transform.position, Quaternion.identity);

        
        Vector3 spawnDirection = Vector3.forward;

       
        cube.GetComponent<Rigidbody>().AddForce(spawnDirection * spawnForce, ForceMode.Impulse);
    }
}
