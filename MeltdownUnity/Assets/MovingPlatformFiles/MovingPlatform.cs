using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttach : MonoBehaviour
{
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
}

//ADDITIONAL NOTES
//The player will need to be dragged to be set as the player in the eyes of the platform.
//This script should be copiable to each other platform as well.

//The ANIMATION created will need to be redone individually for each moving platform.
//Equally after numerous attempts I could not personally implement any of the conditions. A better programmer could have but I am not currently one of those.
//Within the animation I've currently left it such that it should hold for 2 seconds, move to it's destination, hold for 2 seconds, then move back to start position.
//Since everything must be done by hand I recommend keeping a list of platforms so that you can mark off which ones have been done if something needs altering.