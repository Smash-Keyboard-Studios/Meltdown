using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
	public float PlatformSpeed = 1f;

	public bool Activated = false;
	public bool isConstant = false;
	public bool isFrozen = false;

	public Transform PlatformObject;
	public Vector3 platformStartPos;
	public Vector3 platformEndPos;

	private float _timeCounter;

	private void Start()
	{
		if (isConstant) { Activated = true; }
	}
	// Update is called once per frame
	void Update()
	{
		// closed
		if (_timeCounter < 1 && Activated)
		{
			_timeCounter += Time.deltaTime * PlatformSpeed;
		}
		// open
		else if (_timeCounter > 0 && !Activated)
		{
			_timeCounter -= Time.deltaTime * PlatformSpeed;		
		}
		if (_timeCounter <= 1 && isConstant) { ToggleDoor(); }
		if (_timeCounter >= 0 && isConstant) { ToggleDoor(); }
		// move platform.
		PlatformObject.localPosition = Vector3.Lerp(platformStartPos, platformEndPos, _timeCounter);
	}
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Fire>() != null )
		{
			if (isFrozen)
			{
				PlatformSpeed = PlatformSpeed * 2.5f;
				isFrozen = false; 
			}
		}
		if (other.gameObject.GetComponent<Ice>() != null)
		{
			if (!isFrozen)
			{
				PlatformSpeed = PlatformSpeed * 0.4f;
				isFrozen = true;
			}
		}
	}
	public void ToggleDoor()
	{
		Activated = !Activated;
	}

	public void SetActivated(bool activated)
	{
		Activated = activated;
	}
}
