using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorFunctions : MonoBehaviour
{
    [SerializeField] public GameObject doorObject; // Put the door object in here. 
	public string doorOpenAnimName, doorCloseAnimName; // Put the names of this door's animations in their slots on Inspector.
    void Update()
    {		
		if (Input.GetKeyDown(KeyCode.E)) //Replace this to be triggered by the interaction system.
		{
			if (doorObject == null) { doorObject = gameObject; } //If the door object isn't put into inspector, script grabs the object its on.
			Animator doorAnimation = doorObject.GetComponent<Animator>(); //Locates the door Animator Component.
			if (doorAnimation.GetCurrentAnimatorStateInfo(0).IsName(doorOpenAnimName))// Checks if the current animation is the Opening Animation.
			{
				doorAnimation.ResetTrigger("doorOpenTrigger"); 
				doorAnimation.SetTrigger("doorCloseTrigger");
			}
			else if (doorAnimation.GetCurrentAnimatorStateInfo(0).IsName(doorCloseAnimName)) // Checks if the current animation is the Closing Animation.
			{
				doorAnimation.ResetTrigger("doorCloseTrigger");
				doorAnimation.SetTrigger("doorOpenTrigger");
			}
			else //On initalisation, to make sure the door doesn't stay in Idle State.
			{
				doorAnimation.ResetTrigger("doorCloseTrigger");
				doorAnimation.SetTrigger("doorOpenTrigger");
			}
		}
    }

}
