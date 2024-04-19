using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stop changing my code dumbass, if u have a problem with it then tell me

public class FanSpin : MonoBehaviour
{
	const float FastSpinSpeed = 1;
	const float SlowSpinSpeed = 0.05f;

	public float SlowTime;
	public bool isSlow;

	[SerializeField] private float CurrentSpinSpeed;

	public bool isClockwise = false;

	public KillZone[] KillZones;
	public TrailRenderer[] TrailRenderers;

	// Start is called before the first frame update
	void Start()
	{
		//set initial values
		CurrentSpinSpeed = FastSpinSpeed;
	}

	private void Update()
	{
		if (PauseMenu.Paused) return;


		// somthing here is causing fan to stutter. try using time.deltatime.
		if (isClockwise)
		{
			//rotate clockwise
			transform.Rotate(Vector3.down * CurrentSpinSpeed);
		}
		else
		{
			//rotate anticlockwise
			transform.Rotate(Vector3.up * CurrentSpinSpeed);
		}
	}

	IEnumerator SlowFan()
	{
		//change spin speed to slower for slowtime number of seconds
		isSlow = true;

		foreach (KillZone killZone in KillZones)
		{
			killZone.IsEnabled = false;
		}

		foreach (TrailRenderer trilRenderer in TrailRenderers)
		{
			trilRenderer.emitting = false;
		}

		CurrentSpinSpeed = SlowSpinSpeed;
		yield return new WaitForSeconds(SlowTime);
		CurrentSpinSpeed = FastSpinSpeed;
		isSlow = false;

		foreach (TrailRenderer trilRenderer in TrailRenderers)
		{
			trilRenderer.emitting = true;
		}

		foreach (KillZone killZone in KillZones)
		{
			killZone.IsEnabled = true;
		}
	}

	public void WarmUpFan()
	{
		CurrentSpinSpeed = FastSpinSpeed;
		isSlow = false;

		foreach (TrailRenderer trilRenderer in TrailRenderers)
		{
			trilRenderer.emitting = true;
		}

		foreach (KillZone killZone in KillZones)
		{
			killZone.IsEnabled = true;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		//if collide with ice then slow fan
		if (collision.gameObject.GetComponent<Ice>() != null)
		{
            StartCoroutine("SlowFan");
        }

		if (collision.gameObject.GetComponent<Fire>() != null)
		{
			WarmUpFan();
		}
	}
}
