using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public Transform bulletSpawnPoint;
	public GameObject bulletPrefableftclick;
	public GameObject bulletPrefabrightclick;
	public float bulletSpeed = 10;

	public bool hasFire = false;
	public bool hasIce = false;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0) && hasFire)
		{
			// Instantiate the bullet GameObject
			GameObject bulletObject = Instantiate(bulletPrefableftclick, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

			// Check if the bullet GameObject is not null
			if (bulletObject != null)
			{
				// Get the Rigidbody component of the bullet GameObject
				Rigidbody bulletRigidbody = bulletObject.GetComponent<Rigidbody>();

				// Check if the bullet Rigidbody component is not null
				if (bulletRigidbody != null)
				{
					// Apply velocity to the bullet Rigidbody
					bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletSpeed;
				}

			}

		}

		if (Input.GetKeyDown(KeyCode.Mouse1) && hasIce)
		{
			// Instantiate the bullet GameObject
			GameObject bulletObject = Instantiate(bulletPrefabrightclick, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

			// Check if the bullet GameObject is not null
			if (bulletObject != null)
			{
				// Get the Rigidbody component of the bullet GameObject
				Rigidbody bulletRigidbody = bulletObject.GetComponent<Rigidbody>();

				// Check if the bullet Rigidbody component is not null
				if (bulletRigidbody != null)
				{
					// Apply velocity to the bullet Rigidbody
					bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletSpeed;
				}

			}

		}
	}
}
