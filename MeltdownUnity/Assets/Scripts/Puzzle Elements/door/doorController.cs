using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
	public GameObject doorObject;
	public bool isSlidingDoor;
	public float openRotation, closeRotation, openX, openY, openZ, speed;
	private float closeX, closeY, closeZ; //Privated to prevent unintentional edits on Inspector
	public bool isOpening = false;
	private Vector3 startPos;
	void Start()
	{
		if (doorObject == null) { doorObject = gameObject; } //If the door object isn't put into inspector, script grabs the object its on.
		startPos = doorObject.transform.localPosition;
        float closeX = startPos.x; // Sets the current object's location as the inital close position.
        float closeY = startPos.y; //
		float closeZ = startPos.z; //
	}
	void Update()
	{
		// if (Input.GetKeyDown(KeyCode.E)) { isOpening = !isOpening; } //Replace this to be triggered by the interaction system.
		Vector3 currentRotation = transform.localEulerAngles;
		Vector3 currentPosition = doorObject.transform.localPosition;
		if (isSlidingDoor == true)
		{
			if (isOpening)
			{
				float distance = Vector3.Distance(transform.localPosition, new Vector3(openX, openY, openZ));
				if (distance > 0.1f) { doorObject.transform.localPosition = Vector3.Lerp(currentPosition, new Vector3(openX, openY, openZ), speed * Time.deltaTime); }
			}
			else
			{
				float distance = Vector3.Distance(transform.localPosition, new Vector3(closeX, closeY, closeZ));
				if (distance > 0.1f) { doorObject.transform.localPosition = Vector3.Lerp(currentPosition, new Vector3(closeX, closeY, closeZ), speed * Time.deltaTime); }
			}
		}
		else
		{
			if (isOpening)
			{
				if (currentRotation.y <= openRotation) //Rotates the door to the open rotation.
				{
					transform.localEulerAngles = Vector3.Lerp(currentRotation, new Vector3(currentRotation.x, openRotation, currentRotation.z), speed * Time.deltaTime);
				}
			}
			else
			{
				if (currentRotation.y >= closeRotation) //Rotates the door to the closed rotation.
				{
					transform.localEulerAngles = Vector3.Lerp(currentRotation, new Vector3(currentRotation.x, closeRotation, currentRotation.z), speed * Time.deltaTime);
				}
			}

		}
	}

	public void ToggleDoorOpen()
	{
		isOpening = !isOpening;
	}
}
