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
    private float _firstHeatPoint, _finalHeatPoint, _firstCoolPoint, _finalCoolPoint;
    private float _nextRotationPoint, _prevRotationPoint;
    private float _rotationIncrement, _smallIncrement, _smallDelay;
    private Quaternion _defaultRotation, _finalRotation, _descendRotation, _ascendRotation;
    private bool _didForwardRot = false, _didBackRot = false;

    // [Editor Variables]

    [Header("<size=15>Rotation Parameters</size>")]
    [Space]
    [SerializeField] private Vector3 _rotationAxis = Vector3.back;
    [SerializeField] private int _equalHeatPoints = 5;
    [SerializeField] private int _equalCoolPoints = 10;
    [SerializeField][Range(0.0f, 360.0f)] private float _equalEndPoint = 180f;
    [SerializeField] private float[] _heatRotationPoints = { 0.0f, 70f, 140.0f, 210.0f };
    [SerializeField] private float[] _coolRotationPoints = { 0.0f, 35.0f, 70.0f, 105.0f, 140.0f, 175.0f, 210.0f };
    [SerializeField][Range(0.0f, 100.0f)] private float _heatingSpeed = 40.0f;
    [SerializeField][Range(0.0f, 100.0f)] private float _coolingSpeed = 40.0f;

    [Header("<size=15>Location Parameters</size>")]
    [Space]
    [SerializeField][Range(0.0f, 360.0f)] private float _destinationDegrees = 175.0f;
    [SerializeField][Range(0.0f, 360.0f)] private float _destinationTolerance = 20f;
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
    [SerializeField] private bool _minCoolLocation = false;
    [SerializeField] private bool _maxCoolLocation = false;

    public UnityEvent OnComplete;

    // [Events]

    private void Awake()
    {
        // Indexes of Arrays.
        _startHeatIndex = 1;
        _startCoolIndex = 1;
        _finalHeatIndex = _heatRotationPoints.Length - 1;
        _finalCoolIndex = _coolRotationPoints.Length - 1;
        _heatIndex = _startHeatIndex;
        _coolIndex = _startCoolIndex;

        // Elements of Array
        _firstHeatPoint = _heatRotationPoints[0];
        _firstCoolPoint = _coolRotationPoints[0];
        _finalHeatPoint = _heatRotationPoints[_finalHeatIndex];
        _finalCoolPoint = _coolRotationPoints[_finalCoolIndex];
        _nextRotationPoint = _heatRotationPoints[_heatIndex];
        _prevRotationPoint = !HeatOnlyScale ? _firstCoolPoint : _firstHeatPoint;

        const float noXrotation = 0, noYrotation = 0, posZrotation = 1, negZrotation = -1;

        // Rotations
        _defaultRotation = transform.rotation;
        _finalRotation = _defaultRotation * Quaternion.Euler(noXrotation, noYrotation, negZrotation * _finalHeatPoint);
        _descendRotation = _defaultRotation * Quaternion.Euler(noXrotation, noYrotation, posZrotation * (_firstHeatPoint - _firstCoolPoint)); 
        _ascendRotation = _finalRotation * Quaternion.Euler(noXrotation, noYrotation, negZrotation * (_finalCoolPoint - _finalHeatPoint));

        // Corrective Variables
        _smallIncrement = 0.2f;
        _smallDelay = 0.1f;
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
        else if (_minCoolLocation)
        {
            _minCoolLocation = false;
            MinGauge();
        }
        else if (_maxCoolLocation)
        {
            _maxCoolLocation = false;
            AscendGauge();
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

            if (Mathf.Abs(_rotationIncrement) < Mathf.Abs(_nextRotationPoint))
            {
                if (!InstantRotation)
                {
                    transform.Rotate(_rotationAxis, _heatingSpeed * Time.deltaTime * Mathf.Sign(_nextRotationPoint - _prevRotationPoint));
                    _rotationIncrement += _heatingSpeed * Time.deltaTime * Mathf.Sign(_nextRotationPoint - _prevRotationPoint);

                    CheckForDestination();

                    // corrects rotation to end point.
                    if (Mathf.Abs(_rotationIncrement) > Mathf.Abs(_finalHeatPoint - _smallIncrement))
                    {
                        Invoke("MaxGauge", _smallDelay);
                    }
                }
                else
                {
                    JumpToNextRotationPoint();

                    // corrects rotation to end point.
                    if (_nextRotationPoint == _finalHeatPoint)
                    {
                        Invoke("MaxGauge", _smallDelay);
                    }
                }
                _didForwardRot = true;
            }

            // When the point is reached, rotation is disabled, and the next point (to rotate to) is assigned.

            else if (Mathf.Abs(_rotationIncrement) >= Mathf.Abs(_nextRotationPoint)) // Greater than symbol is necessary because of imprecision.
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

                _heatIndex += (_heatIndex < _finalHeatIndex) ? 1 : 0; // increases _heatIndex unless end.
                SetRotationPoints(_heatIndex, _coolIndex);

                AutoCoolOn = false;

                CheckForDestination();

                if (AutoCoolTriggerOn)
                {
                    Invoke("AwakenCoolDown", _autoCoolDelay);
                }
            }
        }
    }

    public void DecrementGauge()
    {
        if (Mathf.Abs(_rotationIncrement) > Mathf.Abs(_prevRotationPoint)) // active rotation towards the previous point.
        {
            if (!InstantRotation)
            {
                transform.Rotate(_rotationAxis, _coolingSpeed * Time.deltaTime * Mathf.Sign(_prevRotationPoint - _nextRotationPoint));
                _rotationIncrement += _coolingSpeed * Time.deltaTime * Mathf.Sign(_prevRotationPoint - _nextRotationPoint);

                CheckForDestination();

                // corrects rotation to default.
                if (Mathf.Abs(_rotationIncrement) < Mathf.Abs(_firstHeatPoint + _smallIncrement))
                {
                    Invoke("ResetGauge", _smallDelay);
                }
            }
            else
            {
                JumpToPrevRotationPoint();

                // corrects rotation to default.
                if (_prevRotationPoint == _firstHeatPoint)
                {
                    Invoke("ResetGauge", _smallDelay);
                }
            }

            _didBackRot = true;
        }

        // If the cooling takes it back below a previous rotation point, that will then become the next point to reach.

        else if (Mathf.Abs(_rotationIncrement) <= Mathf.Abs(_prevRotationPoint))
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

            if (HeatOnlyScale)
            {
                _heatIndex -= (_heatIndex > _startHeatIndex) ? 1 : 0; // decreases _heatIndex unless start.
            }
            _coolIndex -= (_coolIndex > _startCoolIndex) ? 1 : 0; // decreases _coolIndex unless start.
            SetRotationPoints(_heatIndex, _coolIndex);

            CheckForDestination();
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
        SetRotationPoints(_heatIndex, _coolIndex);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
    }

    public void AscendGauge()
    {
        if (RunTimeReposition)
        {
            UpdateRotationPoints(IncrementStatus.Reset);
        }

        // Resets rotation.
        transform.rotation = _ascendRotation;

        // Resets tracking-related variables
        _rotationIncrement = _finalCoolPoint;
        _coolIndex = _finalCoolIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        AutoCoolOn = false;
    }

    public void MinGauge()
    {
        if (RunTimeReposition)
        {
            UpdateRotationPoints(IncrementStatus.Max);
        }

        // Completes rotation.
        transform.rotation = _descendRotation;

        // Sets tracking-related variables to final destinations.
        _rotationIncrement = _firstCoolPoint;
        _coolIndex = _startCoolIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        AutoCoolOn = false;
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
        if (HeatOnlyScale)
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
    }

    private void SetRotationPoints(int i, int j)
    {
        if (HeatOnlyScale || _didForwardRot)
        {
            _nextRotationPoint = _heatRotationPoints[i];
        }
        else if (_didBackRot || _rotationIncrement == _finalHeatPoint)
        {
            _heatIndex = FindHeatIndex(j);
            _nextRotationPoint = _heatRotationPoints[_heatIndex];
        }

        if (HeatOnlyScale)
        {
            _prevRotationPoint = _heatRotationPoints[i - 1];
        }
        else if (_didForwardRot || _rotationIncrement == _finalHeatPoint)
        {
            _coolIndex = FindCoolIndex(i);
            _prevRotationPoint = _coolRotationPoints[_coolIndex];
        }
        else if (_didBackRot)
        {
            _prevRotationPoint = _coolRotationPoints[j];
        }

        Debug.Log("Cool Index: " + _coolIndex);
        Debug.Log("Prev Rotation Point: " + _prevRotationPoint);

        // Debug.Log("Next Rotation Point: " + _nextRotationPoint);
        // Debug.Log("HeatIndex: " + _heatIndex);

    }


    // Function to find a heat rotation point nearest the previous cool point to rotate to.

    private int FindHeatIndex(int j)
    {
        int nextIndex = _heatIndex;
        float minDifference = _finalCoolPoint - _firstHeatPoint;

        for (int i = _heatIndex; i < _finalHeatIndex; i++)
        {
            float heatRotation = _heatRotationPoints[i];
            float coolRotation = _coolRotationPoints[j];

            if (coolRotation > heatRotation)
            {
                float difference = coolRotation - heatRotation;

                if (difference < minDifference)
                {
                    minDifference = difference;
                    nextIndex = i;
                }
            }
        }

        return nextIndex;
    }

    // Function to find a cool rotation point nearest the previous heat point to rotate to.

    private int FindCoolIndex(int i)
    {
        int nextIndex = _coolIndex;
        float minDifference = _finalHeatPoint - _firstCoolPoint;
        bool noResult = true;

        for (int j = _coolIndex; j < _finalCoolIndex; j++)
        {
            float heatRotation = _heatRotationPoints[i];
            float coolRotation = _coolRotationPoints[j];
            float difference = heatRotation - coolRotation;

            if (heatRotation > coolRotation)
            {
                if (difference < minDifference)
                {
                    minDifference = difference;
                    nextIndex = j;
                }
            }

            noResult = false;
        }

        if (noResult)
        {
            nextIndex--;
        }

        return nextIndex;
    }

    private void JumpToNextRotationPoint()
    {
        transform.Rotate(_rotationAxis, _nextRotationPoint - _rotationIncrement);
        _rotationIncrement = _nextRotationPoint;
    }

    private void JumpToPrevRotationPoint()
    {
        transform.Rotate(_rotationAxis, _prevRotationPoint - _rotationIncrement);
        _rotationIncrement = _prevRotationPoint;
    }

    private void CheckForDestination()
    {
        if (_rotationIncrement >= (_destinationDegrees - _destinationTolerance) && _rotationIncrement <= (_destinationDegrees + _destinationTolerance))
        {
            OnComplete.Invoke();
            Debug.Log(_rotationIncrement);
            AutoCoolTriggerOn = false; ;
        }
    }
}