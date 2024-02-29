using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointDoorTrigger : MonoBehaviour
{

    public UnityEvent checkpointTrigger;
    public doorController checkpointDoor;
    public bool DoorTriggered = false;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("My ancestors are smiling upon me imperial, can you say the same?");
        if (collision.gameObject.name == "Player" && DoorTriggered == false)
        {
            DoorTriggered = true;
            checkpointTrigger.Invoke();
            Debug.Log("God is dead and I killed him with my own hands.");
        }    
    }
}
