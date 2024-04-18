using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInteractionScript : MonoBehaviour
{

	public float InteractionDistance = 1.75f; // Set distance of where the player can interact with the objects
	public KeyCode interactKeycode = KeyCode.E; // The keycode interaction key
	public Sprite Crosshair;
	public Sprite Crosshair2; // Images for the crosshairs (to enlarge)
	public Image crosshairImage;
	public TMP_Text tutorialText; // Text that appears under the crosshair

	private Camera _camera;

	void Start()
	{
		_camera = Camera.main;
	}

	void Update()
	{
		if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, InteractionDistance)) // Creates an origin of the raycast from the camera, going forward from the camera, then outing the first hit object using the InteractionDistance as a limiter
		{
			if (hit.transform.CompareTag("InteractableObject")) // Checks the object has an "InteractableObject" tag
			{
				crosshairImage.sprite = Crosshair2;
				tutorialText.enabled = true;
				string temp = hit.transform.GetComponent<IInteractable>() != null ? hit.transform.GetComponent<IInteractable>().ObjectName : "Null";
				tutorialText.text = "Press '" + interactKeycode.ToString() + "' to interact with " + temp; // Brings up the tutorial text and changes it to both the keycode assigned and what has been hit
				if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Interact)))
				{
					hit.collider.gameObject.GetComponent<IInteractable>()?.Interact();
				}
			}
			else
			{
				crosshairImage.sprite = Crosshair;
				tutorialText.enabled = false;
			}
		}
		else
		{
			crosshairImage.sprite = Crosshair;
			tutorialText.enabled = false;
		}     // Just sets the crosshair sprite to the smaller one
	}
}
