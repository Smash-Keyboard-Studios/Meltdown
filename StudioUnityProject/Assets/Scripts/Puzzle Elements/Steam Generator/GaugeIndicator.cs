// THIS SCRIPT CONTROLS THE MOVEMENT OF THE PIN (HAND) OF THE GAUGE AND SHOULD BE ATTACHED IT.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GaugeIndicator : MonoBehaviour
{
    // [Non-Editor Variables]

    private float _firstRotationPoint, _nextRotationPoint, _prevRotationPoint, _finalRotationPoint;
    private int _rotationIndex, _startRotationIndex, _finalRotationIndex;
    private float _rotationIncrement;
    private Quaternion _defaultRotation, _finalRotation;
    private bool _didForwardRot = false, _didBackRot = false;

    // [Editor Variables]

    [Header("<b>Rotation Parameters</b>")]
    [Space]
    [SerializeField] private float[] _rotationPoint = { 0.0f, 35.0f, 70.0f, 105.0f, 140.0f, 175.0f };
    [SerializeField][Range(0.0f, 100.0f)] private float _heatingSpeed = 40.0f;
    [SerializeField][Range(0.0f, 100.0f)] private float _coolingSpeed = 5.0f;
    [SerializeField][Range(0.0f, 100.0f)] private float _autoCoolDelay = 2.0f;
    public bool AutoCoolOn = false;
    public bool AutoCoolTriggerOn = false;
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
        else if ((AutoCoolOn || MoveToPrevPoint) && transform.rotation != _defaultRotation) // Cools down (reverse rotation) to origin if not.
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

                    // corrects rotation to end point.
                    if (_rotationIncrement > _finalRotationPoint - 0.2f)
                    {
                        Invoke("MaxGauge", 0.1f);
                    }
                }
                else
                {
                    JumpToNextRotationPoint();

                    // corrects rotation to end point.
                    if (_nextRotationPoint == _finalRotationPoint)
                    {
                        Invoke("MaxGauge", 0.1f);
                    }
                }
                _didForwardRot = true;
            }

            // When the point is reached, rotation is disabled, and the next point (to rotate to) is assigned.

            else if (_rotationIncrement >= _nextRotationPoint) // Greater than symbol is necessary because of imprecision.
            {
                JumpToNextRotationPoint();

                // Previous backward rotation check.
                if (!_didBackRot)
                {
                    MoveToNextPoint = false;
                }
                else
                {
                    _didBackRot = false;
                }

                if (_rotationIndex < _finalRotationIndex) // Determines if the current point is not the last one.
                {
                    _rotationIndex++;
                    SetRotationPoints(_rotationIndex);

                    AutoCoolOn = false;

                    if (AutoCoolTriggerOn)
                    {
                        Invoke("AwakenCoolDown", _autoCoolDelay);
                    }
                }
                else
                {
                    AutoCoolOn = false;

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

                // corrects rotation to default.
                if (_rotationIncrement < _firstRotationPoint + 0.2f)
                {
                   Invoke("ResetGauge",0.1f);
                }
            }
            else
            {
                JumpToPrevRotationPoint();

                // corrects rotation to default.
                if (_prevRotationPoint == _firstRotationPoint)
                {
                    Invoke("ResetGauge", 0.1f);
                }
            }

            _didBackRot = true;
        }

        // If the cooling takes it back below a previous rotation point, that will then become the next point to reach.

        else if (_rotationIncrement <= _prevRotationPoint)
        {
            JumpToPrevRotationPoint();

            // Previous forward rotation check.
            if (!_didForwardRot)
            {
                MoveToPrevPoint = false;
            }
            else
            {
                _didForwardRot = false;
            }

            if (_rotationIndex > _startRotationIndex)
            { 
                _rotationIndex--;
                SetRotationPoints(_rotationIndex);

                if (AutoCoolTriggerOn)
                {
                    Invoke("AwakenCoolDown", _autoCoolDelay);
                }
            }
        }
    }

    public void AwakenCoolDown()
    {
        if (!MoveToNextPoint && transform.rotation != _finalRotation) // if rotation is disabled and isn't last point.
        {
            AutoCoolOn = true;
        }
    }

    public void ResetGauge()
    {
        // Resets rotation.
        transform.rotation = _defaultRotation;

        // Resets tracking-related variables
        _rotationIncrement = _firstRotationPoint;
        _rotationIndex = _startRotationIndex;
        SetRotationPoints(_rotationIndex);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
    }

    public void MaxGauge()
    {
        // Completes rotation.
        transform.rotation = _finalRotation;

        // Sets tracking-related variables to final destinations.
        _rotationIncrement = _finalRotationPoint;
        _rotationIndex = _finalRotationIndex;
        SetRotationPoints(_rotationIndex);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        AutoCoolOn = false;
    }

    private void SetRotationPoints(int i)
    {
        _nextRotationPoint = _rotationPoint[i];
        _prevRotationPoint = _rotationPoint[i - 1];
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