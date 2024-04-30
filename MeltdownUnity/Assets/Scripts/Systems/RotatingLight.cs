using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLight : MonoBehaviour
{
	public Vector3 RotationAngle = Vector3.up;

	public float RotationSpeed = 5f;

	public Transform ObjectToRotate;

	public Light LightSource;

	public bool IsEnabled = true;


	void Start()
	{
		if (LightSource == null)
		{
			LightSource = ObjectToRotate.GetComponent<Light>();
		}
	}

	void Update()
	{
		if (!IsEnabled)
		{
			LightSource.enabled = false;
		}
		else
		{
			LightSource.enabled = true;
			ObjectToRotate.Rotate(RotationAngle * RotationSpeed * Time.deltaTime);
		}

	}
}
