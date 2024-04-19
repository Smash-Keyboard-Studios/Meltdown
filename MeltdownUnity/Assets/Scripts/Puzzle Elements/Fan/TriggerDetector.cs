using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
	FanSpin parentFanSpin;

	// Start is called before the first frame update
	void Start()
	{
		parentFanSpin = GetComponentInParent<FanSpin>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		//if collide with ice then slow fan
		if (collision.gameObject.GetComponent<Ice>() != null)
		{
			parentFanSpin.StartCoroutine("SlowFan");
        }

		if (collision.gameObject.GetComponent<Fire>() != null)
		{
			parentFanSpin.WarmUpFan();
		}
	}
}
