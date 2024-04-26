using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableObject : MonoBehaviour
{
	public Vector3 FireStartLocation;

	private ParticleSystem FireParticles;
	private GameObject FireParticlesObject;

	// public float BurnTime = 3f;

	private float _scaleSpeed = 1f;

	private bool _isBurning = false;
	private bool _isBurningOver = false;

	private float _currentBurnTime = 0f;

	private Vector3 _startScale;

	public event Action DestoryedObject;
	public delegate void FlammableObjectDestroyedDelegate();

	public AudioSource ObjectSource;

	void Start()
	{
		FireParticlesObject = Instantiate(Resources.Load<GameObject>("Particles/Fire"), FireStartLocation, Quaternion.identity, transform);
		FireParticlesObject.transform.localPosition = FireStartLocation;
		FireParticles = FireParticlesObject.GetComponent<ParticleSystem>();

		_scaleSpeed = 1f / FireParticles.main.duration;
		_startScale = transform.localScale;
	}

	void Update()
	{
		if (_isBurning)
		{
			_currentBurnTime += Time.deltaTime * _scaleSpeed;
			if (transform.localScale.x >= 0 && transform.localScale.y >= 0 && transform.localScale.z >= 0)
			{
				transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, _currentBurnTime);
			}
			else
			{
				transform.localScale = Vector3.zero;
			}
		}

		if (_isBurningOver)
		{
			Destroy(this.gameObject);
            ObjectSource.Stop();
        }
	}

	private void OnCollisionEnter(Collision other)
	{
		//if hit by fire
		if (other.gameObject.GetComponent<Fire>() != null)
		{

			StartCoroutine(StartBurn());
			ObjectSource.Play();
		}
	}

	void OnDestroy()
	{
		if (DestoryedObject != null)
		{
			DestoryedObject.Invoke();
		}
	}

	private IEnumerator StartBurn()
	{
		FireParticles.Play();
		_isBurning = true;
		yield return new WaitForSeconds(FireParticles.main.duration + 2f);
		_isBurningOver = true;
	}
}
