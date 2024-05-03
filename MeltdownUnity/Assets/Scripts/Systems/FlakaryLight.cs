using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakaryLight : MonoBehaviour
{
	public Light LightSource;

	public Renderer LightMaterial = null;

	public float MaxOnTime = 1;
	public float MaxOffTime = 1;

	private float _localTime = 0;
	private float _waitTime = 0;

	private Color _emissionColor;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		_localTime += Time.deltaTime;

		if (_localTime >= _waitTime)
		{
			LightSource.enabled = !LightSource.enabled;



			if (LightSource.enabled)
			{
				_waitTime = Random.Range(0, MaxOnTime);

				if (LightMaterial != null)
				{
					LightMaterial.material.SetColor("_EmissionColor", Color.white);
				}
			}
			else
			{
				_waitTime = Random.Range(0, MaxOffTime);

				if (LightMaterial != null)
				{
					LightMaterial.material.SetColor("_EmissionColor", Color.black);
				}
			}

			_localTime = 0;
		}
	}
}
