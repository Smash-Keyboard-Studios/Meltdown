using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDoor : MonoBehaviour
{
	public float DoorSpeed = 1f;

	public bool Closed = false;

	public Transform LeftDoor;
	public Vector3 leftDoorClosePos;
	public Vector3 leftDoorOpenPos;

	public Transform RightDoor;
	public Vector3 rightDoorClosePos;
	public Vector3 rightDoorOpenPos;

	private float _timeCounter;

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
		LeftDoor.localPosition = Vector3.Lerp(leftDoorOpenPos, leftDoorClosePos, _timeCounter);
		RightDoor.localPosition = Vector3.Lerp(rightDoorOpenPos, rightDoorClosePos, _timeCounter);
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
