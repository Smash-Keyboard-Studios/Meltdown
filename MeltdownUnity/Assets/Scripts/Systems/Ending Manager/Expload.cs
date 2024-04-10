using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expload : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.GetComponent<Rigidbody>() != null)
		{
			Vector3 dir = other.transform.position - transform.position;
			other.transform.GetComponent<Rigidbody>().AddForce(dir * 10f, ForceMode.Impulse);
		}
	}
}
