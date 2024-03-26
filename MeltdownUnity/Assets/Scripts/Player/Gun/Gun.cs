using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public Transform bulletSpawnPoint;
	public GameObject FirePrefab;
	public GameObject IcePrefab;
	//public float bulletSpeed = 10;

	public float FireRate = 0.3f;

	public bool hasFire = true;
	public bool hasIce = true;

	public AmmoController AmmoController;

	private GameObject currentBullet;

	private float _LocalTime;
	// private float _WaitTime;

	private Camera cam;

	void Start()
	{
		AmmoController = gameObject.GetComponent<AmmoController>();
		cam = Camera.main;
	}

	void Update()
	{
		if (PauseMenu.Paused) return;

		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
		{
			bulletSpawnPoint.LookAt(hit.point);
		}
		else
		{
			bulletSpawnPoint.localRotation = Quaternion.identity;
		}


		if (_LocalTime < FireRate + 1f) _LocalTime += Time.deltaTime;


		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.ShootFire)) && AmmoController.FireAmmo > 0 && _LocalTime > FireRate && currentBullet == null)
		{

			//-1 fire ammo
			AmmoController.FireAmmo -= 1;

			// Instantiate the bullet GameObject
			currentBullet = Instantiate(FirePrefab, Vector3.zero, bulletSpawnPoint.rotation, bulletSpawnPoint);

			currentBullet.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			currentBullet.GetComponent<Rigidbody>().isKinematic = true;

			currentBullet.transform.localPosition = Vector3.zero;
		}
		else if (Input.GetKeyUp(InputManager.GetKey(InputActions.KeyAction.ShootFire)) && currentBullet != null)
		{
			_LocalTime = 0;


			// Check if the bullet GameObject is not null
			if (currentBullet != null && currentBullet.GetComponent<Fire>() != null)
			{

				currentBullet.GetComponent<Fire>().Activate();

				currentBullet.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
				currentBullet.GetComponent<Rigidbody>().isKinematic = false;

				// Check if the bullet Rigidbody component is not null
				if (currentBullet.GetComponent<Rigidbody>() != null)
				{
					// Apply velocity to the bullet Rigidbody
					currentBullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * currentBullet.GetComponent<Fire>().Speed;
				}

			}

			currentBullet = null;
		}

		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.ShootIce)) && AmmoController.IceAmmo > 0 && _LocalTime > FireRate && currentBullet == null)
		{
			//-1 ice ammo
			AmmoController.IceAmmo -= 1;

			// Instantiate the bullet GameObject
			currentBullet = Instantiate(IcePrefab, Vector3.zero, bulletSpawnPoint.rotation, bulletSpawnPoint);

			currentBullet.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			currentBullet.GetComponent<Rigidbody>().isKinematic = true;

			currentBullet.transform.localPosition = Vector3.zero;



		}
		else if (Input.GetKeyUp(InputManager.GetKey(InputActions.KeyAction.ShootIce)) && currentBullet != null)
		{
			_LocalTime = 0;


			// Check if the bullet GameObject is not null
			if (currentBullet != null && currentBullet.GetComponent<Ice>() != null)
			{
				currentBullet.GetComponent<Ice>().Activate();

				currentBullet.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
				currentBullet.GetComponent<Rigidbody>().isKinematic = false;

				// Check if the bullet Rigidbody component is not null
				if (currentBullet.GetComponent<Rigidbody>() != null)
				{
					// Apply velocity to the bullet Rigidbody
					currentBullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * currentBullet.GetComponent<Ice>().Speed;
				}

			}

			currentBullet = null;
		}
	}
}
