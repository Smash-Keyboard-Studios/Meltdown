using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

	public float lifeTime = 5;
	public float Speed = 50f;
	public float decalLifeTime = 3f;
	public GameObject DecalPrefab;

	public float StartSize = 0.05f;
	public float EndSize = 0.5f;

	public float ScaleSpeed = 0.5f;

	public string[] IgnoredTags;
	public LayerMask IgnoredLayers;

	public bool Active = false;

	private float _localTime;
	private float _Scale = 0.05f;


	void Awake()
	{
		transform.localScale = new Vector3(StartSize, StartSize, StartSize);
	}

	void Update()
	{

		if (!Active) return;

		_localTime += Time.deltaTime * ScaleSpeed;

		// can change for stange shaped objects.
		_Scale = Mathf.Lerp(StartSize, EndSize, _localTime);

		transform.localScale = new Vector3(_Scale, _Scale, _Scale);
	}

	public void Activate()
	{
		Destroy(gameObject, lifeTime);
		Active = true;
		transform.parent = null;
	}

	public virtual void OnCollisionEnter(Collision other)
	{
		if (!Active) return;

		if (DecalPrefab != null && !CompareTag(other) && (IgnoredLayers != (IgnoredLayers | (1 << other.gameObject.layer))))
		{
			GameObject decal = Instantiate(DecalPrefab, other.contacts[0].point, Quaternion.FromToRotation(Vector3.up, other.GetContact(0).normal) * DecalPrefab.transform.rotation);
			decal.transform.parent = other.transform;
			Destroy(decal, decalLifeTime);
		}

		Destroy(gameObject);
	}

	private bool CompareTag(Collision other)
	{
		if (IgnoredTags.Length > 0)
		{
			foreach (var tag in IgnoredTags)
			{
				if (other.gameObject.CompareTag(tag)) return true;
			}
		}

		return false;
	}
}
