using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class masterDoorController : MonoBehaviour
{
	public float DoorSpeed = 1f;
	public bool isSlidingDoor = false;
	public bool Closed = false;
	public bool isPlayerInteractable = false;
	public Transform LeftDoor = null;
	public Vector3 leftDoorClosePos;
	public Vector3 leftDoorOpenPos;
	public Vector3 leftDoorCloseRotation;
	public Vector3 leftDoorOpenRotation;


	public Transform RightDoor;
	public Vector3 rightDoorClosePos;
	public Vector3 rightDoorOpenPos;
	public Vector3 rightDoorCloseRotation;
	public Vector3 rightDoorOpenRotation;

	private float _timeCounter;
	void Start()
	{
		if (isPlayerInteractable == true)
		{
			if (LeftDoor != null) LeftDoor.tag = "InteractableObject";
			if (RightDoor != null) RightDoor.tag = "InteractableObject";
		}
		else
		{
			if (LeftDoor != null) LeftDoor.tag = "Untagged";
			if (RightDoor != null) RightDoor.tag = "Untagged";
		}

		if (Closed)
		{
			_timeCounter = 1.1f;
		}
		else
		{
			_timeCounter = 0f;
		}
	}
	// Update is called once per frame
	void Update()
	{
		// closed
		if (_timeCounter < 1 && Closed)
		{
			_timeCounter += Time.deltaTime * DoorSpeed;
		}
		// open
		else if (_timeCounter > 0 && !Closed)
		{
			_timeCounter -= Time.deltaTime * DoorSpeed;
		}

		// move doors.
		if (LeftDoor != null)
		{
			LeftDoor.localEulerAngles = Vector3.Lerp(leftDoorOpenRotation, leftDoorCloseRotation, _timeCounter);
			if (isSlidingDoor == true)
			{
				LeftDoor.localPosition = Vector3.Lerp(leftDoorOpenPos, leftDoorClosePos, _timeCounter);
			}
		}
		RightDoor.localEulerAngles = Vector3.Lerp(rightDoorOpenRotation, rightDoorCloseRotation, _timeCounter);
		if (isSlidingDoor == true)
		{
			RightDoor.localPosition = Vector3.Lerp(rightDoorOpenPos, rightDoorClosePos, _timeCounter);
		}
	}

	public void ToggleDoor()
	{
		Closed = !Closed;
	}

	public void SetClosed(bool closed)
	{
		Closed = closed;
	}
}
