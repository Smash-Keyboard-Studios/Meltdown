using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour, IInteractable
{
	public doorController drController;

	private bool doorOpen = false;

	private bool buttonPressed = false; // Used for single use buttons

	[SerializeField] private bool toggleButton = false;
	[SerializeField] private bool timedButton = false;  // Serialised access fields for  designers to decide what button type

	[SerializeField] private int waitTimer = 1; // Length of time between debounces
	[SerializeField] private bool pauseInteraction = false;

	private void Start()
	{
		drController = GetComponent<doorController>();
	}


	public void Interact()
	{

	}
	public void OpenDoor()
	{
		if (toggleButton == true)
		{
			drController.ToggleDoorOpen();
			doorOpen = !doorOpen;
		}

		else if (buttonPressed == false) // Ensures the button cannot be pressed twice
		{
			drController.ToggleDoorOpen();
			buttonPressed = true;
		}
	}
}
