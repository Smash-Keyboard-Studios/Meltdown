using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class calls functions in the Moving Platform script depending of the projectile.
/// </summary>
public class MovingPlatformProjectileDetection : MonoBehaviour
{
	// the moving platform script controlling the moving platform.
	private MovingPlatform _mp;

	void Start()
	{
		// stores a referance to the moving platform manager.
		_mp = GetComponentInParent<MovingPlatform>();
	}

	// called when a object with a collider touches this collider. (read documentation)
	void OnCollisionEnter(Collision other)
	{
		// If the other collider / object has a ice script, then freeze it.
		if (other.collider.GetComponent<Ice>() != null)
		{
			_mp.Freez();
		}

		// If the other collider / object has a fire script, then unfreeze it.
		else if (other.collider.GetComponent<Fire>() != null)
		{
			_mp.UnFreez();
		}

	}
}
