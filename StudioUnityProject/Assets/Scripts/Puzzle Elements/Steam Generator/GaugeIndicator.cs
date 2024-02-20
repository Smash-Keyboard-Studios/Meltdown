// THIS SCRIPT CONTROLS THE MOVEMENT OF THE PIN (HAND) OF THE GAUGE AND SHOULD BE ATTACHED IT.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GaugeIndicator : MonoBehaviour
{
	// [Non-Editor Variables]

	private float[] _rotationPoint = { 0.0f, 35.0f, 70.0f, 105.0f, 140.0f, 175.0f };

 	private float _firstRotationPoint, _nextRotationPoint, _prevRotationPoint, _finalRotationPoint;
	private int _rotationIndex, _startRotationIndex, _finalRotationIndex;
	private float _rotationIncrement;
	private Quaternion _defaultRotation, _finalRotation;

	// [Editor Variables]

	[Header("<b>Rotation Parameters</b>")]
	[Space]
	[SerializeField][Range(0.0f, 100.0f)] private float _heatingSpeed = 40.0f;
	[SerializeField][Range(0.0f, 100.0f)] private float _coolingSpeed = 5.0f;
	[SerializeField][Range(0.0f, 100.0f)] private float _coolOffDelay = 2.0f;
	public bool DisableCoolOff = false;
	public bool NoTriggerCoolOff = false;
	public bool InstantRotation = false;

	[Header("<b>Location Parameters</b>")]
	[Space]
	public bool MoveToNextPoint = false;
	public bool MoveToPrevPoint = false;
	public bool ResetPinLocation = false;
	public bool FinalPinLocation = false;

	public UnityEvent OnComplete;

	// [Events]

	private void Awake()
	{
		// Indexes of Array
		_startRotationIndex = 1;
		_rotationIndex = _startRotationIndex;
		_finalRotationIndex = _rotationPoint.Length - 1;

		// Elements of Array
		_firstRotationPoint = _rotationPoint[0];
		_nextRotationPoint = _rotationPoint[_rotationIndex];
		_finalRotationPoint = _rotationPoint[_finalRotationIndex];

		// Rotations
		_defaultRotation = transform.rotation;
		_finalRotation = _defaultRotation * Quaternion.Euler(0, 0, -_finalRotationPoint);
	}

	private void Update()
	{
		if (ResetPinLocation) 
		{
			ResetPinLocation = false;
			ResetGauge();
		}
		else if (FinalPinLocation)
		{
			FinalPinLocation = false;
			MaxGauge();
		}
		else if (MoveToNextPoint) // Determines if forward rotation is enabled.
		{
			MoveToPrevPoint = false;
			IncrementGauge();
		}
		else if ((!DisableCoolOff || MoveToPrevPoint)  && transform.rotation != _defaultRotation) // Cools down (reverse rotation) to origin if not.
		{
			MoveToNextPoint = false;
			DecrementGauge();
        }
	}

	// [Custom Methods]

	public void IncrementGauge()
	{
		if (_rotationIndex <= _finalRotationIndex) // Determines if there is a valid point to rotate to.
		{
			// Active rotation towards the next point.

			if (_rotationIncrement < _nextRotationPoint)
			{
				if (!InstantRotation)
				{
					transform.Rotate(-Vector3.forward, _heatingSpeed * Time.deltaTime);
					_rotationIncrement += _heatingSpeed * Time.deltaTime;
				} 
				else 
				{
					JumpToNextRotationPoint();
				}
			}

			// When the point is reached, rotation is disabled, and the next point (to rotate to) is assigned.

			else if (_rotationIncrement >= _nextRotationPoint) // Greater than symbol is necessary because of imprecision.
			{
				JumpToNextRotationPoint();
				MoveToNextPoint = false;

                if (_rotationIndex < _finalRotationIndex) // Determines if the current point is not the last one.
				{
					_rotationIndex++;
					SetRotationPoints(_rotationIndex);

                    DisableCoolOff = true;

					if (!NoTriggerCoolOff)
					{
						Invoke("AwakenCoolDown", _coolOffDelay);
					}
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
		if (_rotationIncrement > _prevRotationPoint) // active rotation towards the previous point.
		{
			if (!InstantRotation)
			{
				transform.Rotate(-Vector3.forward, -_coolingSpeed * Time.deltaTime);
				_rotationIncrement -= _coolingSpeed * Time.deltaTime;
			}
			else 
			{
				JumpToPrevRotationPoint();
			}
		}

		// If the cooling takes it back below a previous rotation point, that will then become the next point to reach.
		
		else if (_rotationIncrement <= _prevRotationPoint)
		{	
			JumpToPrevRotationPoint();
			MoveToPrevPoint = false;

			if (_rotationIndex > _startRotationIndex)
			{
				_rotationIndex--;
				SetRotationPoints(_rotationIndex);
			}
		}
	}

	public void AwakenCoolDown()
	{
		if (!MoveToNextPoint && _rotationIndex < _finalRotationIndex) // if rotation is disabled and isn't last point.
		{
			DisableCoolOff = false;
		}
	}

	public void ResetGauge()
	{
		// Resets rotation.
		transform.rotation = _defaultRotation;

		// Resets gauge-related variables
		_rotationIncrement = _firstRotationPoint;
		_rotationIndex = _startRotationIndex;
		SetRotationPoints(_rotationIndex);
		MoveToNextPoint = false;
		MoveToPrevPoint = false;
	}

	public void MaxGauge()
	{
		// Completes rotation.
		transform.rotation = _finalRotation;

		// Sets gauge-related variables to final destinations.
		_rotationIncrement = _finalRotationPoint;
		_rotationIndex = _finalRotationIndex;
		SetRotationPoints(_rotationIndex);
		MoveToNextPoint = false;
		MoveToPrevPoint = false;
		DisableCoolOff = true;
	}

	private void SetRotationPoints(int i)
	{
		_nextRotationPoint = _rotationPoint[i];
		_prevRotationPoint = _rotationPoint[i-1];
	}

	private void JumpToNextRotationPoint()
	{
		transform.Rotate(-Vector3.forward, _nextRotationPoint - _rotationIncrement);
		_rotationIncrement = _nextRotationPoint;
	}

	private void JumpToPrevRotationPoint()
	{
		transform.Rotate(-Vector3.forward, _prevRotationPoint - _rotationIncrement);
		_rotationIncrement = _prevRotationPoint;
	}
}
