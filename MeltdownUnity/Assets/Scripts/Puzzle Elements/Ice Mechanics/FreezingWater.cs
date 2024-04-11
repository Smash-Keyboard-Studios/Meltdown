using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingWater : MonoBehaviour
{
	public GameObject IceCube;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Ice>() != null)
		{

			GameObject go = Instantiate(IceCube, transform.localPosition, Quaternion.identity, transform);
			//call Audio Manager (SFX)

			go.transform.localScale = new Vector3(10f, 0.1f, 10f);
		}
	}
}
