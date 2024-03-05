using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
	public float lifeTime = 5;
	public float Speed = 50f;
	public float decalLifeTime = 3f;
	public GameObject DecalPrefab;

	void Awake()
	{
		Destroy(gameObject, lifeTime);
	}

	void OnCollisionEnter(Collision other)
	{
		if (DecalPrefab != null)
		{
			GameObject decal = Instantiate(DecalPrefab, other.contacts[0].point, Quaternion.FromToRotation(Vector3.up, other.GetContact(0).normal) * DecalPrefab.transform.rotation);
			decal.transform.parent = other.transform;
			Destroy(decal, decalLifeTime);
		}

		Destroy(gameObject);
	}



}
