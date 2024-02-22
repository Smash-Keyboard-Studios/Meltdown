using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CheckpointScript : MonoBehaviour
{
   [SerializeField] GameObject Player;
    //[SerializeField] List<GameObject> Checkpoints;
    //[SerializeField] Vector3 vectorPoint;
    //[SerializeField] float Death;

    //void Update()
    //{
    //    if (player.transform.position.y < -Death) 
    //    {
    //        player.transform.position = vectorPoint;
    //    }
    //}

    //private void OntriggerEnter(Collider other) 
    //{
    //    vectorPoint -= player.transform.position;
    //    Destroy(other.gameObject);

    //}
    public bool Death = false;
    public int  CheckpointNumbers;
    public int[] CheckpointNumbersArray = new int[5];
    public int CheckpointNumber;
    public Vector3 PlayerCheckpointPosition;

    //public Vector3 Update()
    //{
    //Vector3 PlayerCheckpointPosition = GameObject.Find($"Checkpoint{CheckpointNumber}").transform.position;
    //Debug.Log ($"{PlayerCheckpointPosition}");
    //return PlayerCheckpointPosition;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint") 
        {
               PlayerCheckpointPosition = other.transform.position;
               Debug.Log($"{PlayerCheckpointPosition}");
        }
    }
    public void Update()
    {
        if (this.transform.position.y < - 100)
        {
            Death = true;
            Debug.Log("Death is true");
            Debug.Log($"{PlayerCheckpointPosition}");
        }
        if (Death == true) 
        {
            //PlayerCheckpointPosition.x = Mathf.RoundToInt(PlayerCheckpointPosition.x);
            Player.transform.position = new Vector3(PlayerCheckpointPosition.x, PlayerCheckpointPosition.y, PlayerCheckpointPosition.z);
            Death = false;
            Debug.Log("You should have respawned LOL");
        }
    }
}
