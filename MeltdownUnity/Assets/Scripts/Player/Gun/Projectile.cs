using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// Base class for all projectiles.
/// </summary>
public class Projectile : MonoBehaviour
{
	[Header("Projectile settings")]

	[Tooltip("How long the projectile")]
	public float lifeTime = 5;

	[Tooltip("How fast the projectile is")]
	public float Speed = 50f;

	[Tooltip("How long decals last")]
	public float decalLifeTime = 3f;

	[Tooltip("The defualt start scale")]
	public float StartSize = 0.05f;

	[Tooltip("The end scale")]
	public float EndSize = 0.5f;

	[Tooltip("The speed that the projectile scales up")]
	public float ScaleSpeed = 0.5f;


	[Header("Ignored layers and tags")]

	[Tooltip("Ignored tags")]
	public string[] IgnoredTags;

	[Tooltip("Ingored layers")]
	public LayerMask IgnoredLayers;

	[Header("Light Settings")]
	public Light attachedLight;

	public float EndRadiusCollision = 10f;

	public float DurationOfCollisionLight = 0.1f;


	[Header("Variables")]

	[Tooltip("The decal prefab to spawn when this collides into somthing")]
	public GameObject DecalPrefab;


	[Header("Active state")]

	[Tooltip("Weathere this objet is active or not")]
	public bool Active = false;

	// used for scaling
	private float _localTime;
	private float _Scale = 0.05f;


	void Awake()
	{
		// we want to set the scale to the start scale.
		transform.localScale = new Vector3(StartSize, StartSize, StartSize);
	}

	void Update()
	{
		// if we are not active then do nothing.
		if (!Active) return;

		// increment time.
		_localTime += Time.deltaTime * ScaleSpeed;

		// can change for stange shaped objects.
		// get the scale ammount form current time.
		_Scale = Mathf.Lerp(StartSize, EndSize, _localTime);

		// Scale the object.
		transform.localScale = new Vector3(_Scale, _Scale, _Scale);
	}

	// A funtion to activate this object.
	public void Activate()
	{
		Destroy(gameObject, lifeTime);
		Active = true;
		transform.parent = null;
	}

	// overidable function
	public virtual void OnCollisionEnter(Collision other)
	{
		// dont do anything if we are disabled.
		if (!Active) return;

		// if there is a decal prefab and the other tag is not ignored and the layer is not ignored,
		// summon the decal.
		if (DecalPrefab != null && !CompareTag(other) && (IgnoredLayers != (IgnoredLayers | (1 << other.gameObject.layer))))
		{
			GameObject decal = Instantiate(DecalPrefab, other.contacts[0].point, Quaternion.FromToRotation(Vector3.up, other.GetContact(0).normal) * DecalPrefab.transform.rotation);
			decal.transform.parent = other.transform;
			Destroy(decal, decalLifeTime);
		}

		// destroy us.
		StartCoroutine(EndProjectile());
	}

	// Function to compare tag of a collision.
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

	private IEnumerator EndProjectile()
	{
		attachedLight.intensity = EndRadiusCollision;
		attachedLight.range = EndRadiusCollision;

		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = Vector3.zero;

		yield return new WaitForSeconds(DurationOfCollisionLight);
		Destroy(this.gameObject);
	}
}
