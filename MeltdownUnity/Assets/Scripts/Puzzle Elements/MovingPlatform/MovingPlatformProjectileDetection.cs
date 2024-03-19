using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformProjectileDetection : MonoBehaviour
{
	MovingPlatform _mp;

	// Start is called before the first frame update
	void Start()
	{
		_mp = GetComponentInParent<MovingPlatform>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.GetComponent<Ice>() != null)
		{
			_mp.Freez();
		}

		else if (other.collider.GetComponent<Fire>() != null)
		{
			_mp.UnFreez();
		}

	}
}
