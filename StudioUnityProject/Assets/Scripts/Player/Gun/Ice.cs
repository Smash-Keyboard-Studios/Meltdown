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
			GameObject decal = Instantiate(DecalPrefab, other.contacts[0].point, Quaternion.identity);
			Destroy(decal, decalLifeTime);
		}

		Destroy(gameObject);
	}



}
