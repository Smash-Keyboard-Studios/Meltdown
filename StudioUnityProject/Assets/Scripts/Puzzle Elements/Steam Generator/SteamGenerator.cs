using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamGenerator : MonoBehaviour
{
	public GaugeIndicator gaugeIndicator;

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Fire>() != null)
		{
			gaugeIndicator.MoveToNextPoint = true;
		}
	}
}
