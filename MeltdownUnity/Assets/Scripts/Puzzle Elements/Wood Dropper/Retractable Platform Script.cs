using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class RetractablePlatformScript : MonoBehaviour
{
    private Vector3 endPosition;
    private float elapsedTime;
    private float retractingDuration = 1;
    private float percentageComplete;

    //Monitors whether the player has met the condition for the platform to be retracted
    private bool retractPlatformConditionMet = false;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        //Assigns the end position of the platform opening based on its current position
        endPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (retractPlatformConditionMet)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / retractingDuration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPosition, percentageComplete);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void RetractPlatform()
    {
        retractPlatformConditionMet = true;
        GetComponent<Collider>().enabled = false; //I DONT UNDERSTAND WHY THIS IS NEEDED BUT THE OBJECTS WONT DROP WITHOUT IT DESPITE THE COLLIDER NOT BEING IN THE WAY
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
