// THIS SCRIPT CONTROLS THE MOVEMENT OF THE PIN (HAND) OF THE GAUGE AND SHOULD BE ATTACHED IT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GaugeIndicator : MonoBehaviour
{
	// [Non-Editor Variables]

	private float[] _rotationPoint = { 0.0f, 35.0f, 70.0f, 105.0f, 140.0f, 175.0f };

	private float _rotationIncrement, _nextRotationPoint;
	private int _rotationIndex = 1;
	private Quaternion _defaultRotation;

	// [Editor Variables]

	[Header("<b>Rotation Parameters</b>")]
	[Space]
	[SerializeField][Range(0.0f, 100.0f)] private float _rotationSpeed = 40.0f;
	[SerializeField][Range(0.0f, 100.0f)] private float _coolOffSpeed = 5.0f;
	[SerializeField][Range(0.0f, 100.0f)] private float _coolOffDelay = 2.0f;

	public bool MoveToNextPoint = false;
	public bool DisableCoolOff = false;

	public UnityEvent OnComplete;

	// [Events]

	private void Awake()
	{
		_nextRotationPoint = _rotationPoint[_rotationIndex];
		_defaultRotation = transform.rotation;
	}

	private void Update()
	{
		if (MoveToNextPoint) // Determines if rotation is enabled.
		{
			IncrementGauge();
		}
		else if (!DisableCoolOff && transform.rotation != _defaultRotation) // Cools down (reverse rotation) to origin if not.
		{
			DecrementGauge();
        }
	}

	// [Custom Methods]

	public void IncrementGauge()
	{
		if (_rotationIndex < _rotationPoint.Length) // Determines if there is a valid point to rotate to.
		{
			// Active rotation towards the next point.

			if (_rotationIncrement < _nextRotationPoint)
			{
				transform.Rotate(-Vector3.forward, _rotationSpeed * Time.deltaTime);
				_rotationIncrement += _rotationSpeed * Time.deltaTime;
			}

			// When the point is reached, rotation is disabled, and the next point (to rotate to) is assigned.

			else if (_rotationIncrement >= _nextRotationPoint) // Greater than symbol is necessary because of imprecision.
			{
				MoveToNextPoint = false;

                if (_rotationIndex < _rotationPoint.Length - 1) // Determines if the current point is not the last one.
				{
					_rotationIndex++;
					_nextRotationPoint = _rotationPoint[_rotationIndex];
                    DisableCoolOff = true;
					Invoke("awakenCoolDown", _coolOffDelay);
                }
				else
				{
					DisableCoolOff = true;
                    OnComplete.Invoke();
				}

			}
		}
	}

	public void DecrementGauge()
	{
		transform.Rotate(-Vector3.forward, -_coolOffSpeed * Time.deltaTime);
		_rotationIncrement -= _coolOffSpeed * Time.deltaTime;

		// If the cooling takes it back below a previous rotation point, that will then become the next point to reach.

		if (_rotationIndex > 1 && _rotationIncrement < _rotationPoint[_rotationIndex - 1])
		{
			_rotationIndex--;
			_nextRotationPoint = _rotationPoint[_rotationIndex];
		}
	}

	public void awakenCoolDown()
	{
		if (!MoveToNextPoint && _rotationIndex < _rotationPoint.Length - 1) // if rotation is disabled and isn't last point.
		{
			DisableCoolOff = false;
		}
	}

	public void ResetGauge()
	{
		// Reset rotation
		transform.rotation = _defaultRotation;

		// Reset gauge-related variables
		_rotationIncrement = 0.0f;
		_rotationIndex = 1;
		_nextRotationPoint = _rotationPoint[_rotationIndex];
		MoveToNextPoint = false;
	}
}
