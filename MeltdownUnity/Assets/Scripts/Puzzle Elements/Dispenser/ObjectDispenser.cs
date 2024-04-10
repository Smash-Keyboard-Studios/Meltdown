using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDispenser : MonoBehaviour
{
    public GameObject ObjectToSpawn; // What object the dispenser will spawn

    public bool isTimed = false; // True/False for whether or not the dispenser is activated by timer
    public float dropDelay = 2; // Delay between each object drop
    public bool startActive = true; // Provides bool value for variable below
    private bool togglableTimerState = true; // Whether or not the timed dispenser starts dispenser or needs to be activated first

    private int curCount = 0; // Current amount of dispensed objects in the world
    public int spawnMax = 5;

    private void Start()
    {
        if (isTimed) // Begin timer version of dispenser
        {
            togglableTimerState = startActive;
            StartCoroutine(TimedSpawning()); // Starts timer
        }
        
    }

    private void SpawnObject() // Spawning function
    {
        if (curCount+1 <= spawnMax) // Checks if there is a free slot for another object
        {
            GameObject newObject = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
            newObject.AddComponent<DispenserItem>(); // Set the owner for the newly dispensed object
            newObject.GetComponent<DispenserItem>().owner = this;

            curCount++;
        }
    }

    public void ActivateDispenser() // Called by other activator scripts to trigger the dispenser
    {
        if (!isTimed) 
        {
            SpawnObject();
        }
        else
        {
            togglableTimerState = !togglableTimerState; // Flips active state when called
        }
    }

    public void FreeUpSpace() // Call this function to signalfy an object getting destroyed
    {
        curCount--;
    }

    IEnumerator TimedSpawning() // Continous timer for spawning objects every set amount of time
    {
        while (true)
        {
            if (togglableTimerState)
            {
                SpawnObject();
            }

            yield return new WaitForSeconds(dropDelay);
        }
    }

}
