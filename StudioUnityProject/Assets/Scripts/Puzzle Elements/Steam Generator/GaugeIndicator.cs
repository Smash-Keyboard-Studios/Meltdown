// THIS SCRIPT CONTROLS THE MOVEMENT OF THE PIN (HAND) OF THE GAUGE AND SHOULD BE ATTACHED IT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class GaugeIndicator : MonoBehaviour
{
    // [Non-Editor Variables]

    private int _heatIndex, _startHeatIndex, _finalHeatIndex, _startCoolIndex, _coolIndex, _finalCoolIndex;
    private float _firstHeatPoint, _finalCoolPoint, _finalHeatPoint, _firstCoolPoint;
    private float _nextRotationPoint, _prevRotationPoint;
    private float _rotationIncrement, smallIncrement, smallDelay;
    private Quaternion _defaultRotation, _finalRotation;
    private bool _didForwardRot = false, _didBackRot = false;

    // [Editor Variables]

    [Header("<size=15>Rotation Parameters</size>")]
    [Space]
    [SerializeField] private float[] _heatRotationPoints = { 0.0f, 70f, 140.0f, 210.0f };
    [SerializeField] private float[] _coolRotationPoints = { 0.0f, 35.0f, 70.0f, 105.0f, 140.0f, 175.0f, 210.0f };
    [SerializeField][Range(0.0f, 100.0f)] private float _heatingSpeed = 40.0f;
    [SerializeField][Range(0.0f, 100.0f)] private float _coolingSpeed = 5.0f;

    [Header("<size=15>Location Parameters</size>")]
    [Space]
    [SerializeField][Range(0.0f, 360.0f)] private float _destinationDegrees = 175.0f;
    public bool MoveToNextPoint = false;
    public bool MoveToPrevPoint = false;
    public bool ResetPinLocation = false;
    public bool FinalPinLocation = false;

    [Header("<size=15>Test-Only Parameters</size>")]
    [Space]
    [SerializeField][Range(0.0f, 100.0f)] private float _autoCoolDelay = 2.0f;
    public bool AutoCoolOn = false;
    public bool AutoCoolTriggerOn = false;
    public bool InstantRotation = false;
    public bool RunTimeReposition = false;
    public bool HeatOnlyScale = false;

    public UnityEvent OnComplete;

    // [Events]

    private void Awake()
    {
        // Indexes of Arrays.
        _startHeatIndex = 1;
        _finalHeatIndex = _heatRotationPoints.Length - 1;
        _finalCoolIndex = _coolRotationPoints.Length - 1;
        _heatIndex = _startHeatIndex;
        _coolIndex = _finalCoolIndex;

        // Elements of Array
        _firstHeatPoint = _heatRotationPoints[0];
        _finalHeatPoint = _heatRotationPoints[_finalHeatIndex];
        _nextRotationPoint = _heatRotationPoints[_heatIndex];
        _prevRotationPoint = !HeatOnlyScale ? _coolRotationPoints[_coolIndex] : _firstHeatPoint;

        const float noXRotation = 0, noYRotation = 0, negZRotation = -1;

        // Rotations
        _defaultRotation = transform.rotation;
        _finalRotation = _defaultRotation * Quaternion.Euler(noXrotation, noYrotation, negZRotation * _finalHeatPoint); // -z rotation

        // Corrective Variables
        smallIncrement = 0.2f;
        smallDelay = 0.1f;
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
        else if (AutoCoolOn || MoveToPrevPoint) // Cools down (reverse rotation) to origin if not.
        {
            if (transform.rotation == _defaultRotation) // Cancels if at the start.
            {
                MoveToPrevPoint = false;
                AutoCoolOn = false;
                return;
            }

            MoveToNextPoint = false;
            DecrementGauge();
        }
    }

    // [Custom Methods]

    public void IncrementGauge()
    {
        if (_heatIndex <= _finalHeatIndex) // Determines if there is a valid point to rotate to.
        {
            // Active rotation towards the next point.

            if (_rotationIncrement < _nextRotationPoint)
            {
                if (!InstantRotation)
                {
                    transform.Rotate(-Vector3.forward, _heatingSpeed * Time.deltaTime);
                    _rotationIncrement += _heatingSpeed * Time.deltaTime;

                    // corrects rotation to end point.
                    if (_rotationIncrement > _finalHeatPoint - smallIncrement)
                    {
                        Invoke("MaxGauge", smallDelay);
                    }
                }
                else
                {
                    JumpToNextRotationPoint();

                    // corrects rotation to end point.
                    if (_nextRotationPoint == _finalHeatPoint)
                    {
                        Invoke("MaxGauge", smallDelay);
                    }
                }
                _didForwardRot = true;
            }

            // When the point is reached, rotation is disabled, and the next point (to rotate to) is assigned.

            else if (_rotationIncrement >= _nextRotationPoint) // Greater than symbol is necessary because of imprecision.
            {
                JumpToNextRotationPoint(); // corrects the imprecision

                // Previous backward rotation check.
                if (!_didBackRot)
                {
                    MoveToNextPoint = false;
                }
                else
                {
                    _didBackRot = false;
                }

                if (RunTimeReposition)
                {
                    UpdateRotationPoints(IncrementStatus.Forward);
                }

                if (_heatIndex < _finalHeatIndex) // Increments if the current point is not the last one.
                {
                    _heatIndex++;
                    SetRotationPoints(_heatIndex, _coolIndex);
                }

                AutoCoolOn = false;

                if (_rotationIncrement == _destinationDegrees)
                {
                    OnComplete.Invoke();
                }
                else if (AutoCoolTriggerOn)
                {
                    Invoke("AwakenCoolDown", _autoCoolDelay);
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
                if (_rotationIncrement < _firstHeatPoint + smallIncrement)
                {
                    Invoke("ResetGauge", smallDelay);
                }
            }
            else
            {
                JumpToPrevRotationPoint();

                // corrects rotation to default.
                if (_prevRotationPoint == _firstHeatPoint)
                {
                    Invoke("ResetGauge", smallDelay);
                }
            }

            _didBackRot = true;
        }

        // If the cooling takes it back below a previous rotation point, that will then become the next point to reach.

        else if (_rotationIncrement <= _prevRotationPoint)
        {
            JumpToPrevRotationPoint(); // corrects the imprecision.

            // Previous forward rotation check.
            if (!_didForwardRot)
            {
                MoveToPrevPoint = false;
            }
            else
            {
                _didForwardRot = false;
            }

            if (RunTimeReposition)
            {
                UpdateRotationPoints(IncrementStatus.Back);
            }

            _heatIndex -= (_heatIndex > _startHeatIndex) ? 1 : 0; // decreases _heatIndex unless start.
            _coolIndex += (_coolIndex < _finalCoolIndex) ? 1 : 0; // increases _coolIndex unless end.
            SetRotationPoints(_heatIndex, _coolIndex);

            if (_rotationIncrement == _destinationDegrees)
            {
                OnComplete.Invoke();
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
        if (RunTimeReposition)
        {
            UpdateRotationPoints(IncrementStatus.Reset);
        }

        // Resets rotation.
        transform.rotation = _defaultRotation;

        // Resets tracking-related variables
        _rotationIncrement = _firstHeatPoint;
        _heatIndex = _startHeatIndex;
        _coolIndex = _finalCoolIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
    }

    public void MaxGauge()
    {
        if (RunTimeReposition)
        {
            UpdateRotationPoints(IncrementStatus.Max);
        }

        // Completes rotation.
        transform.rotation = _finalRotation;

        // Sets tracking-related variables to final destinations.
        _rotationIncrement = _finalHeatPoint;
        _heatIndex = _finalHeatIndex;
        _coolIndex = _startCoolIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        AutoCoolOn = false;
    }

    public enum IncrementStatus { Reset, Max, Forward, Back };

    private void UpdateRotationPoints(IncrementStatus status)
    {
        // Indexes of Array
        _startHeatIndex = 1;
        _finalHeatIndex = _heatRotationPoints.Length - 1;

        // Elements of Array
        _firstHeatPoint = _heatRotationPoints[0];
        _finalHeatPoint = _heatRotationPoints[_finalHeatIndex];

        // if out of bounds, set to the last point. Minus one if it will be incremented afterwards.
        if (_heatIndex > _finalHeatIndex)
        {
            bool isEndPoint = (status == IncrementStatus.Reset || status == IncrementStatus.Max);
            _heatIndex = isEndPoint ? _finalHeatIndex : _finalHeatIndex - 1;
        }

        SetRotationPoints(_heatIndex, _coolIndex);

        // Rotation to default point adjusted depending on previous forwards or backwards motion.
        if (status == IncrementStatus.Forward)
        {
            _defaultRotation = transform.rotation * Quaternion.Euler(0, 0, _nextRotationPoint - _firstHeatPoint);
        }
        else if (status == IncrementStatus.Back)
        {
            _defaultRotation = transform.rotation * Quaternion.Euler(0, 0, _prevRotationPoint - _firstHeatPoint);
        }
        else if (status == IncrementStatus.Reset)
        {
            _defaultRotation = _finalRotation * Quaternion.Euler(0, 0, _finalHeatPoint - _firstHeatPoint);
        }

        // Rotation to final point.
        _finalRotation = _defaultRotation * Quaternion.Euler(0, 0, _firstHeatPoint - _finalHeatPoint);
    }

    private void SetRotationPoints(int i, int j)
    {

        _nextRotationPoint = _heatRotationPoints[i];

        if (HeatOnlyScale)
        {
            _prevRotationPoint = _heatRotationPoints[i - 1];
        }
        else if (_didBackRot)
        {
            _prevRotationPoint = _coolRotationPoints[j];
        }
        else
        {
            _prevRotationPoint = _coolRotationPoints[FindCoolIndex(j)];
        }
    }

    // Function to find a cool rotation point nearest the previous heat point to rotate to.

    private int FindCoolIndex(int heatIndex)
    {
        int nextIndex = 0;
        float minDifference = float.MaxValue;

        for (int i = 0; i < _finalCoolIndex; i++)
        {
            float heatRotation = _finalHeatPoint - _prevRotationPoint;

            if (_coolRotationPoints[i] > heatRotation)
            {
                float difference = _coolRotationPoints[i] - heatRotation;

                if (difference < minDifference)
                {
                    minDifference = difference;
                    nextIndex = i;
                }
            }
        }

        return nextIndex;
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