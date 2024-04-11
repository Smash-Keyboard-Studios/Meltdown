using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlammableObject : MonoBehaviour
{
	public ParticleSystem FireParticles;

	// public float BurnTime = 3f;

	private float _scaleSpeed = 1f;

	private bool _isBurning = false;
	private bool _isBurningOver = false;

	private float _currentBurnTime = 0f;

	private Vector3 _startScale;

	void Start()
	{
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
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		//if hit by fire
		if (other.gameObject.GetComponent<Fire>() != null)
		{

			StartCoroutine(StartBurn());
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
