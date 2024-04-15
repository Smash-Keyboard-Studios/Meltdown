// THIS SCRIPT CONTROLS THE MOVEMENT OF THE PIN (HAND) OF THE GAUGE AND SHOULD BE ATTACHED IT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using CustomAttributes;

[System.Serializable]
public struct Destination
{
    public string name;
    public float minPosition;
    public float maxPosition;
    [ShowOnly] public float centralPosition;
    [ShowOnly] public float tolerance;

    public Destination(string name, float minPosition, float maxPosition)
    {
        this.name = name;
        this.minPosition = minPosition;
        this.maxPosition = maxPosition;
        this.centralPosition = (minPosition + maxPosition) / 2f;
        this.tolerance = Mathf.Abs(this.minPosition - this.maxPosition);
    }
}

public class GaugeIndicator : MonoBehaviour
{
    // [Non-Editor Variables]

    private int _heatIndex, _firstHeatIndex, _secondHeatIndex, _initialHeatIndex, _startHeatIndex, _finalHeatIndex;
    private int _startCoolIndex, _coolIndex, _finalCoolIndex;
    private float _firstHeatPoint, _initialHeatPoint, _finalHeatPoint, _firstCoolPoint, _finalCoolPoint;
    private float _smallIncrement, _smallDelay;
    private Quaternion _defaultRotation, _finalRotation, _descendRotation;
    private bool _didForwardRot = false, _didBackRot = false, _runTimeRepositionStandby = false;
    private float _maxDegreesCopy, _equalHeatCopy, _equalCoolCopy;

    // [Editor Variables]

    [Header("<size=15>Rotation Parameters</size>")]
    [Space]
    [Tooltip("The axis of rotation in x, y, z.")][SerializeField] private Vector3 _rotationAxis = Vector3.back;
    [Space]
    [Tooltip("The degrees before the start where the minimum point is.")][SerializeField][Range(0f, 360f)] private float _minDegreesBelowStart = 90.0f;
    [Space]
    [Tooltip("The progression a fire hit does in the dual fire-ice scale.")][Range(0, 100)] public int _firePercentage = 40;
    [Tooltip("The regression an ice hit does in the dual fire-ice scale.")][Range(0, 100)] public int _icePercentage = 20;
    [Tooltip("The size of a dual fire-ice scale.")][SerializeField][Range(20f, 360f)] private float _maxDegrees = 180f;
    [Space]
    [Tooltip("The speed of a fire hit.")][SerializeField][Range(0.0f, 100.0f)] private float _heatingSpeed = 40.0f;
    [Tooltip("The speed of an ice hit.")][SerializeField][Range(0.0f, 100.0f)] private float _coolingSpeed = 40.0f;

    [Header("<size=15>Location Parameters</size>")]
    [Space]
    [Tooltip("The destination range at which events occur.")]
    [SerializeField]
    public Destination[] _destinations =
    {
        new Destination ("On", 160.0f, 180.0f)
    };
    [Space]
    [Tooltip("Moves according to one fire hit.")] public bool MoveToNextPoint = false;
    [Tooltip("Moves according to one ice hit.")] public bool MoveToPrevPoint = false;
    [Tooltip("Resets to the start position.")] public bool ResetPinLocation = false;
    [Tooltip("Moves to the final position.")] public bool FinalPinLocation = false;
    [Tooltip("Moves to the minimum position.")] public bool MinLocation = false;

    [Header("<size=15>Repetition Parameters</size>")]
    [Space]
    [Tooltip("The set number of rotation points to go through by fire.")][Range(0, 100)] public int FireCalls = 1;
    [Tooltip("The set number of rotation points to go through by ice.")][Range(0, 100)] public int IceCalls = 1;
    [Tooltip("The remaining number of rotation points to go through by fire.")][SerializeField][Range(0, 100)] private int _remainingFireCalls;
    [Tooltip("The remaining number of rotation points to go through by ice.")][SerializeField][Range(0, 100)] private int _remainingIceCalls;
    [Tooltip("The previous rotation point after cooling repetition is done.")][SerializeField][ShowOnly] private float _prevStop;
    [Tooltip("The current rotation value.")][SerializeField][ShowOnly] private float _currentStop;
    [Tooltip("The next rotation point after heating repetition is done.")][SerializeField][ShowOnly] private float _nextStop;

    [Header("<size=15>Scale Parameters</size>")]
    [Space]
    [Tooltip("Whether to override with custom scales.")][SerializeField] private bool _customScale = false;
    [Space]
    [Tooltip("The number of equidistant fire rotation points.")][SerializeField][Range(0, 100)] private int _equalHeatPoints = 3;
    [Tooltip("The size of the fire scale.")][SerializeField][Range(20f, 360f)] private float _equalHeatEndPoint = 210.0f;
    [Space]
    [Tooltip("The number of equidistant ice rotation points.")][SerializeField][Range(0, 100)] private int _equalCoolPoints = 7;
    [Tooltip("The size of the ice scale.")][SerializeField][Range(20f, 360f)] private float _equalCoolEndPoint = 210.0f;
    [Space]
    [Tooltip("The fire rotation points.")][SerializeField] private float[] _heatRotationPoints = { 0.0f, 70f, 140.0f, 210.0f };
    [Tooltip("The ice rotation points.")][SerializeField] private float[] _coolRotationPoints = { 0.0f, 35.0f, 70.0f, 105.0f, 140.0f, 175.0f, 210.0f };
    [Space]
    [Tooltip("The previous rotation point.")][SerializeField][ShowOnly] private float _prevRotationPoint;
    [Tooltip("The current rotation point.")][SerializeField][ShowOnly] private float _currentRotationPoint;
    [Tooltip("The next rotation point.")][SerializeField][ShowOnly] private float _nextRotationPoint;

    [Header("<size=14>Test-Only Rotation Parameters</size>")]
    [Space]
    [Tooltip("The delay at which auto-cooling begins.")][SerializeField][Range(0.0f, 100.0f)] private float _autoCoolDelay = 2.0f;
    [Tooltip("Whether auto-cooling is active.")] public bool AutoCoolOn = false;
    [Tooltip("Whether auto-cooling is triggered.")] public bool AutoCoolTriggerOn = false;
    [Space]
    [Tooltip("Whether instant movement is on.")] public bool InstantRotation = false;
    [Tooltip("Whether the rotation points adapat at run-time (currently heat-only).")] public bool RunTimeReposition = false;
    [Tooltip("Makes the ice hits the same as fire hits.")] public bool HeatOnlyScale = false;

    [Header("<size=14>Test-Only Location Parameters </size>")]
    [Space]
    [Tooltip("Whether setting the local start position is enabled.")][SerializeField] private bool _setStartPosition = false;
    [Tooltip("The co-ordinates to set the local start position.")][SerializeField] private Vector3 _startPosition = Vector3.zero;
    [Space]
    [Tooltip("Whether setting the local start rotation is enabled.")][SerializeField] private bool _setStartRotation = false;
    [Tooltip("The co-ordinates to set the local start rotation.")][SerializeField] private Vector3 _startRotation = Vector3.zero;
    [Space]
    public UnityEvent<string> OnComplete;

    // [Events]

    private void OnValidate()
    {
        // Allows Central Position and Tolerance to be recalculated.

        RebuildDestinations();

        if ((!Application.isPlaying || RunTimeReposition))
        {
            Invoke("MinDegreesAutoAdjustment", _smallDelay);
        }
    }

    private void Awake()
    {
        // Sets Equal Points to rotate to.
        SetEqualHeatPoints();
        SetEqualCoolPoints();

        for (int i = 0; i < _destinations.Length; i++)
        {
            _destinations[i] = new Destination(_destinations[i].name, _destinations[i].minPosition, _destinations[i].maxPosition);
        }

        // Local Start Co-ordinates override.
        if (_setStartPosition)
        {
            transform.localPosition = _startPosition;
        }
        if (_setStartRotation)
        {
            transform.localRotation = Quaternion.Euler(_startRotation);
        }

        // Indexes of Arrays.
        _firstHeatIndex = 0;
        _startCoolIndex = 0;
        _secondHeatIndex = _firstHeatIndex + 1;
        _initialHeatIndex = GetInitialIndex();
        _finalHeatIndex = _heatRotationPoints.Length - 1;
        _finalCoolIndex = _coolRotationPoints.Length - 1;
        _startHeatIndex = _initialHeatIndex != _finalHeatIndex ? _initialHeatIndex + 1 : _initialHeatIndex;
        _heatIndex = _startHeatIndex;

        // Elements of Array
        _firstHeatPoint = _heatRotationPoints[_firstHeatIndex];
        _firstCoolPoint = _coolRotationPoints[_startCoolIndex];
        _initialHeatPoint = _heatRotationPoints[_initialHeatIndex];
        _finalHeatPoint = _heatRotationPoints[_finalHeatIndex];
        _finalCoolPoint = _coolRotationPoints[_finalCoolIndex];
        _nextRotationPoint = _heatRotationPoints[_heatIndex];
        _prevRotationPoint = !HeatOnlyScale ? _firstCoolPoint : _firstHeatPoint;
        _currentRotationPoint = _heatRotationPoints[_initialHeatIndex];
        _currentStop = _currentRotationPoint;

        // Prev/Current Rotation Points need to be known.
        _coolIndex = FindCoolIndex(_initialHeatIndex);

        // Sets Number of Calls to Repeat.
        SetFireCalls();
        SetIceCalls();

        // Rotations
        _defaultRotation = transform.rotation;
        _finalRotation = _defaultRotation * Quaternion.Euler(_rotationAxis * _finalHeatPoint);
        _descendRotation = _defaultRotation * Quaternion.Euler(_rotationAxis * _firstCoolPoint);

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
        else if (MinLocation)
        {
            MinLocation = false;
            if (!HeatOnlyScale)
            {
                MinGauge();
            }
        }
        else if (MoveToNextPoint && FireCalls != 0) // Determines if forward rotation is enabled.
        {
            // Cancels if at the end.
            if (transform.rotation == _finalRotation && _nextRotationPoint == _finalHeatPoint)
            {
                MoveToNextPoint = false;
                return;
            }
            MoveToPrevPoint = false;
            IncrementGauge();

        }
        else if (AutoCoolOn || (MoveToPrevPoint && IceCalls != 0)) // Cools down (reverse rotation) to origin if not.
        {
            // Cancels if at the start.
            if ((HeatOnlyScale && transform.rotation == _defaultRotation && _prevRotationPoint == _firstHeatPoint) || (transform.rotation == _descendRotation && _prevRotationPoint == _firstCoolPoint))
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
            float sign = Mathf.Sign(_nextRotationPoint - _prevRotationPoint);
            const float pos = 1.0f, neg = -1.0f;

            bool incPosRotation = sign == pos && _currentRotationPoint < _nextRotationPoint;
            bool incNegRotation = sign == neg && _currentRotationPoint > _nextRotationPoint;

            // Greater than symbol is necessary because of imprecision.
            bool completePosRotation = sign == pos && _currentRotationPoint >= _nextRotationPoint;
            bool completeNegRotation = sign == neg && _currentRotationPoint <= _nextRotationPoint;
            if (incPosRotation || incNegRotation)
            {
                if (!InstantRotation)
                {
                    transform.Rotate(_rotationAxis, _heatingSpeed * Time.deltaTime * sign);
                    _currentRotationPoint += _heatingSpeed * Time.deltaTime * sign;
                    _currentStop = _currentRotationPoint;

                    // corrects rotation to end point. Needs to be this long because it won't get assigned quick enough in a local variable.
                    if (_nextRotationPoint == _finalHeatPoint && ((sign == pos && _currentRotationPoint > _finalHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _finalHeatPoint + _smallIncrement)))
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

                if (RunTimeReposition && !_runTimeRepositionStandby)
                {
                    UpdateRotationPoints(IncrementStatus.Changed);
                    _runTimeRepositionStandby = true;
                }
            }

            // When the point is reached, rotation is disabled, and the next point (to rotate to) is assigned.

            else if (completePosRotation || completeNegRotation)
            {
                JumpToNextRotationPoint(); // corrects the imprecision
                
                // Previous backward rotation check.
                if (!_didBackRot || !HeatOnlyScale)
                {
                    MoveToNextPoint = false;
                }
                else
                {
                    _didBackRot = false;
                }

                _runTimeRepositionStandby = !RunTimeReposition; // Allows the RunTimeReposition to happen again if enabled.

                _heatIndex += (_heatIndex < _finalHeatIndex) ? 1 : 0; // increases _heatIndex unless end.
                SetRotationPoints(_heatIndex, _coolIndex);

                AutoCoolOn = false;

                // Repeated Increment calls.

                if (_remainingFireCalls > 0)
                {
                    _remainingFireCalls--;
                    MoveToNextPoint = true;
                }
                else
                {
                    CheckForDestination();
                    Invoke("SetFireCalls", _smallDelay);
                    Invoke("SetIceCalls", _smallDelay);
                    if (AutoCoolTriggerOn)
                    {
                        Invoke("AwakenCoolDown", _autoCoolDelay);
                    }
                }
            }
        }
    }

    public void DecrementGauge()
    {
        float sign = Mathf.Sign(_prevRotationPoint - _nextRotationPoint);
        const float pos = 1.0f, neg = -1.0f;

        bool decPosRotation = sign == pos && _currentRotationPoint < _prevRotationPoint;
        bool decNegRotation = sign == neg && _currentRotationPoint > _prevRotationPoint;

        bool completePosRotation = sign == pos && _currentRotationPoint >= _prevRotationPoint;
        bool completeNegRotation = sign == neg && _currentRotationPoint <= _prevRotationPoint;

        if (decPosRotation || decNegRotation) // active rotation towards the previous point.
        {
            if (!InstantRotation)
            {
                transform.Rotate(_rotationAxis, _coolingSpeed * Time.deltaTime * sign);
                _currentRotationPoint += _coolingSpeed * Time.deltaTime * sign;
                _currentStop = _currentRotationPoint;

                // corrects rotation to default. Needs to be this long because it won't get assigned quick enough in a local variable.
                if (HeatOnlyScale && (_prevRotationPoint == _firstHeatPoint) && ((sign == pos && _currentRotationPoint > _firstHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _firstHeatPoint + _smallIncrement)))
                {
                    Invoke("ResetGauge", _smallDelay);
                }
                else if (!HeatOnlyScale && (_prevRotationPoint == _firstCoolPoint) && ((sign == pos && _currentRotationPoint > _firstCoolPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _firstCoolPoint + _smallIncrement)))
                {
                    Invoke("MinGauge", _smallDelay);
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
                else if (_prevRotationPoint == _firstCoolPoint)
                {
                    Invoke("MinGauge", _smallDelay);
                }
            }

            _didBackRot = true;

            if (RunTimeReposition && !_runTimeRepositionStandby)
            {
                UpdateRotationPoints(IncrementStatus.Changed);
                _runTimeRepositionStandby = true;
            }
        }

        // If the cooling takes it back below a previous rotation point, that will then become the next point to reach.

        else if (completePosRotation || completeNegRotation)
        {
            JumpToPrevRotationPoint(); // corrects the imprecision.

            // Previous forward rotation check.
            if (_nextRotationPoint == _finalHeatPoint || !HeatOnlyScale)
            {
                MoveToPrevPoint = false;
                _didForwardRot = false;
            }
            else if (!_didForwardRot)
            {
                MoveToPrevPoint = false;
            }
            else
            {
                _didForwardRot = false;
            }

            _runTimeRepositionStandby = !RunTimeReposition; // Allows the RunTimeReposition to happen again if enabled.

            if (HeatOnlyScale)
            {
                _heatIndex -= (_heatIndex > _startHeatIndex) ? 1 : 0; // decreases _heatIndex unless start.
            }
            _coolIndex -= (_coolIndex > _startCoolIndex) ? 1 : 0; // decreases _coolIndex unless start.
            SetRotationPoints(_heatIndex, _coolIndex);

            // Repeated Decrement calls.

            if (HeatOnlyScale)
            {
                if (_remainingFireCalls > 0)
                {
                    _remainingFireCalls--;
                    MoveToPrevPoint = true;
                }
                else
                {
                    Invoke("SetFireCalls", _smallDelay);
                }
            }
            else
            {

                if (_remainingIceCalls > 0)
                {
                    _remainingIceCalls--;
                    MoveToPrevPoint = true;
                }
                else
                {
                    CheckForDestination();
                    Invoke("SetFireCalls", _smallDelay);
                    Invoke("SetIceCalls", _smallDelay);
                }
            }
        }
    }

    public void AwakenCoolDown()
    {
        // if rotation is disabled and isn't last point.
        if (!MoveToNextPoint)
        {
            AutoCoolOn = true;
        }
    }

    public void SetFireCalls()
    {
        FireCalls = _customScale ? FireCalls : _firePercentage;
        _remainingFireCalls = FireCalls - 1;
        _nextStop = (_heatIndex + _remainingFireCalls) > _finalHeatIndex ? _finalHeatPoint : _heatRotationPoints[_heatIndex + _remainingFireCalls];
        if (HeatOnlyScale)
        {
            _prevStop = (_heatIndex - _remainingFireCalls) < _firstHeatIndex ? _firstHeatPoint : _heatRotationPoints[_heatIndex - _remainingFireCalls];
        }
    }

    public void SetIceCalls()
    {
        IceCalls = _customScale ? IceCalls : _icePercentage;
        _remainingIceCalls = IceCalls - 1;
        _prevStop = (_coolIndex - _remainingIceCalls) < _startCoolIndex ? _firstCoolPoint : _coolRotationPoints[_coolIndex - _remainingIceCalls];
    }

    private int GetInitialIndex()
    {
        float approxStartingPoint = _heatRotationPoints[_firstHeatIndex] + _minDegreesBelowStart;

        for (int i = _firstHeatIndex; i < _heatRotationPoints.Length; i++)
        {
            // Finds a rotation point near zero.
            if (_heatRotationPoints[i] >= approxStartingPoint)
            {
                // checks which point is closer to zero if there's a previous point.
                if (i > _firstHeatIndex)
                {
                    float prevDif = Mathf.Abs(_heatRotationPoints[i - 1] - approxStartingPoint);
                    float curDif = Mathf.Abs(_heatRotationPoints[i] - approxStartingPoint);
                    if (prevDif < curDif)
                    {
                        return i - 1;
                    }
                }
                return i;
            }
        }
        return 1;
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
        _currentRotationPoint = _initialHeatPoint;
        _currentStop = _currentRotationPoint;
        _heatIndex = _startHeatIndex;
        _coolIndex = FindCoolIndex(_initialHeatIndex);
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        Invoke("SetIceCalls", _smallDelay);
        CheckForDestination();
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
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
        _currentRotationPoint = _firstCoolPoint;
        _currentStop = _currentRotationPoint;
        _coolIndex = _startCoolIndex;
        _heatIndex = _firstCoolPoint == _firstHeatPoint ? _secondHeatIndex : _firstHeatIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        Invoke("SetIceCalls", _smallDelay);
        CheckForDestination();
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
        _currentRotationPoint = _finalHeatPoint;
        _currentStop = _currentRotationPoint;
        _heatIndex = _finalHeatIndex;
        _coolIndex = FindCoolIndex(_heatIndex);
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        Invoke("SetIceCalls", _smallDelay);
        CheckForDestination();
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        AutoCoolOn = false;
        if (AutoCoolTriggerOn)
        {
            Invoke("AwakenCoolDown", _autoCoolDelay);
        }
    }

    public enum IncrementStatus { Reset, Max, Changed};

    private void UpdateRotationPoints(IncrementStatus status)
    {
        SetEqualHeatPoints();
        SetEqualCoolPoints();

        // Indexes of Array
        _initialHeatIndex = GetInitialIndex();
        _startHeatIndex = _initialHeatIndex + 1;
        _finalHeatIndex = _heatRotationPoints.Length - 1;
        _finalCoolIndex = _coolRotationPoints.Length - 1;

        // Elements of Array
        _firstHeatPoint = _heatRotationPoints[_firstHeatIndex];
        _firstCoolPoint = _coolRotationPoints[_startCoolIndex];
        _initialHeatPoint = _heatRotationPoints[_initialHeatIndex];
        _finalHeatPoint = _heatRotationPoints[_finalHeatIndex];
        _finalCoolPoint = _coolRotationPoints[_finalCoolIndex];

        // if out of bounds, set to the last point. Minus one if it will be incremented afterwards.
        if (_heatIndex > _finalHeatIndex)
        {
            bool isEndPoint = (status == IncrementStatus.Reset || status == IncrementStatus.Max);
            _heatIndex = isEndPoint ? _finalHeatIndex : _finalHeatIndex - 1;
            _coolIndex = FindCoolIndex(_heatIndex);
        }

        // Rotation to default point adjusted depending on current rotation point.

        if (status == IncrementStatus.Changed)
        {
            _defaultRotation = transform.rotation * Quaternion.Euler(_rotationAxis * (_initialHeatPoint - _currentRotationPoint));
        }
        else if (status == IncrementStatus.Reset)
        {
            _defaultRotation = _finalRotation * Quaternion.Euler(_rotationAxis * (_finalHeatPoint - _initialHeatPoint));
        }

        // Rotation to final point.
        _finalRotation = _defaultRotation * Quaternion.Euler(_rotationAxis * (_initialHeatPoint - _finalHeatPoint));

        // Rotation to minimum point.
        _descendRotation = _defaultRotation * Quaternion.Euler(_rotationAxis * (_initialHeatPoint - _firstCoolPoint));
    }

    private void SetRotationPoints(int i, int j)
    {
        // Check for invalid indexes.
        int newFinalHeatIndex = _heatRotationPoints.Length - 1, newFinalCoolIndex = _coolRotationPoints.Length - 1;

        if (i > newFinalHeatIndex || i < _firstHeatIndex)
        {
            ResetGauge();
            return;
        }

        if (!HeatOnlyScale && (j > newFinalCoolIndex || j < _startCoolIndex))
        {
            MinGauge();
            return;
        }

        // Permutations to set next rotation point.
        if (HeatOnlyScale || _didForwardRot || _currentRotationPoint == _firstCoolPoint || _currentRotationPoint == _initialHeatPoint || _currentRotationPoint == _finalHeatPoint)
        {
            _nextRotationPoint = _heatRotationPoints[i];
        }
        else if (_didBackRot)
        {
            _heatIndex = FindHeatIndex(j + 1);
            _nextRotationPoint = _heatRotationPoints[_heatIndex];
        }

        // Permutations to set previous rotation point.
        if (HeatOnlyScale)
        {
            _prevRotationPoint = _heatRotationPoints[i - 1];
        }
        else if (_currentRotationPoint == _firstCoolPoint && _coolIndex != _startCoolIndex)
        {
            MinGauge();
            return;
        }
        else if (_didBackRot || _currentRotationPoint == _firstCoolPoint || _currentRotationPoint == _initialHeatPoint || _currentRotationPoint == _finalHeatPoint)
        {
            _prevRotationPoint = _coolRotationPoints[j];
        }
        else if (_didForwardRot)
        {
            _coolIndex = FindCoolIndex(i - 1);
            _prevRotationPoint = _coolRotationPoints[_coolIndex];
        }
    }

    // Function to find a heat rotation point nearest the previous cool point to rotate to.

    private int FindHeatIndex(int j)
    {
        float sign = Mathf.Sign(_nextRotationPoint - _prevRotationPoint);
        const float pos = 1.0f, neg = -1.0f;

        int nextIndex = _heatIndex;
        float minDifference = sign == pos ? _finalCoolPoint - _firstHeatPoint : _firstHeatPoint - _finalCoolPoint;

        for (int i = _startHeatIndex; i < _heatRotationPoints.Length; i++)
        {
            float heatRotation = _heatRotationPoints[i];
            float coolRotation = _coolRotationPoints[j];

            if (sign == pos && heatRotation > coolRotation || sign == neg && coolRotation > heatRotation)
            {
                float difference = sign == pos ? heatRotation - coolRotation : coolRotation - heatRotation;

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
        float sign = Mathf.Sign(_nextRotationPoint - _prevRotationPoint);
        const float pos = 1.0f, neg = -1.0f;

        int nextIndex = _coolIndex;
        float minDifference = sign == pos ? _finalHeatPoint - _firstCoolPoint : _firstCoolPoint - _finalHeatPoint;

        for (int j = _startCoolIndex; j < _coolRotationPoints.Length; j++)
        {
            float heatRotation = _heatRotationPoints[i];
            float coolRotation = _coolRotationPoints[j];

            if (sign == pos && heatRotation > coolRotation || sign == neg && coolRotation > heatRotation)
            {
                float difference = sign == pos ? heatRotation - coolRotation : coolRotation - heatRotation;

                if (difference < minDifference)
                {
                    minDifference = difference;
                    nextIndex = j;
                }
            }
        }
        return nextIndex;
    }

    private void JumpToNextRotationPoint()
    {

        transform.Rotate(_rotationAxis, _nextRotationPoint - _currentRotationPoint);
        transform.localRotation = RoundRotationPoint(transform.localRotation);
        _currentRotationPoint = _nextRotationPoint;
        _currentStop = _currentRotationPoint;
    }

    private void JumpToPrevRotationPoint()
    {
        transform.Rotate(_rotationAxis, _prevRotationPoint - _currentRotationPoint);
        transform.localRotation = RoundRotationPoint(transform.localRotation);
        _currentRotationPoint = _prevRotationPoint;
        _currentStop = _currentRotationPoint;
    }

    private Quaternion RoundRotationPoint(Quaternion RotationPoint)
    {
        Vector3 eulerAngles = RotationPoint.eulerAngles;
        eulerAngles.x = RoundToOneDP(eulerAngles.x);
        eulerAngles.y = RoundToOneDP(eulerAngles.y);
        eulerAngles.z = RoundToOneDP(eulerAngles.z);

        return Quaternion.Euler(eulerAngles);
    }

    private float RoundToOneDP(float valueToRound)
    {
        return Mathf.Round(valueToRound * 10) / 10;
    }

    private void RebuildDestinations()
    {
        for (int i = 0; i < _destinations.Length; i++)
        {
            _destinations[i] = new Destination(_destinations[i].name, _destinations[i].minPosition, _destinations[i].maxPosition);
        }
    }

    private void CheckForDestination()
    {
        foreach (Destination destination in _destinations)
        {
            if (_currentRotationPoint >= destination.minPosition && _currentRotationPoint <= destination.maxPosition)
            {
                OnComplete.Invoke(destination.name);
            }
        }
    }

    private void SetEqualHeatPoints()
    {
        const int oneHundredPercent = 100, oneStartPoint = 1, nothing = 0;

        // When the end points are equal and by percent.

        if (!_customScale)
        {
            _equalHeatPoints = oneHundredPercent;
            _equalHeatEndPoint = _maxDegrees;
        }

        // Sets equidistant heat scale rotation points.

        int numberOfPoints = _equalHeatPoints + oneStartPoint;

        if (_equalHeatPoints > nothing)
        {
            _heatRotationPoints = new float[numberOfPoints];
            for (int i = _firstHeatIndex; i < numberOfPoints; i++)
            {
                _heatRotationPoints[i] = i * (_equalHeatEndPoint / _equalHeatPoints) - _minDegreesBelowStart;
            }
        }
    }

    private void SetEqualCoolPoints()
    {
        const int oneHundredPercent = 100, oneStartPoint = 1, nothing = 0;

        // When the end points are equal and by percent.

        if (!_customScale)
        {
            _equalCoolPoints = oneHundredPercent;
            _equalCoolEndPoint = _maxDegrees;
        }

        // Sets equidistant ice scale rotation points.

        int numberOfPoints = _equalCoolPoints + oneStartPoint;

        if (_equalCoolPoints > nothing)
        {
            _coolRotationPoints = new float[numberOfPoints];
            for (int i = _startCoolIndex; i < numberOfPoints; i++)
            {
                _coolRotationPoints[i] = i * (_equalCoolEndPoint / _equalCoolPoints) - _minDegreesBelowStart;
            }
        }
    }

    private void MinDegreesAutoAdjustment()
    {
        // Prevents _minDegreesBelowStart from reaching maxDegrees (or EndPoints) and adjusts accordingly when maxDegrees changes.

        const float zeroDegrees = 0f, oneDegree = 1f, threeSixtyDegrees = 360f;

        // Gives them initial values.
        _maxDegreesCopy = _maxDegreesCopy == zeroDegrees ? _maxDegrees : _maxDegreesCopy;
        _equalHeatCopy = _equalHeatCopy == zeroDegrees ? _equalHeatEndPoint : _equalHeatCopy;
        _equalCoolCopy = _equalCoolCopy == zeroDegrees ? _equalCoolEndPoint : _equalCoolCopy;

        if (!_customScale)
        {
            if (_minDegreesBelowStart >= _maxDegrees)
            {
                _minDegreesBelowStart = _maxDegrees - oneDegree;
            }
            else if (_maxDegrees != _maxDegreesCopy && _maxDegreesCopy != zeroDegrees)
            {
                _minDegreesBelowStart += (_maxDegrees - _maxDegreesCopy);
                _maxDegreesCopy = _maxDegrees;
            }
        }
        else if (_customScale)
        {
            if (_minDegreesBelowStart >= _equalHeatEndPoint)
            {
                _minDegreesBelowStart = _equalHeatEndPoint - oneDegree;
            }
            else if (_equalHeatEndPoint != _equalHeatCopy && _equalHeatCopy != zeroDegrees)
            {
                _minDegreesBelowStart += (_equalHeatEndPoint - _equalHeatCopy);

                _equalHeatCopy = _equalHeatEndPoint;

            }
            else if (_minDegreesBelowStart >= _equalCoolEndPoint)
            {
                _minDegreesBelowStart = _equalCoolEndPoint - oneDegree;
            }
            else if (_equalCoolEndPoint != _equalCoolCopy && _equalCoolCopy != zeroDegrees)
            {
                _minDegreesBelowStart += (_equalCoolEndPoint - _equalCoolCopy);
                _equalCoolCopy = _equalCoolEndPoint;
            }
        }
        // Clamping to the 360 degree range.
        _minDegreesBelowStart = _minDegreesBelowStart > threeSixtyDegrees ? (threeSixtyDegrees - oneDegree) : _minDegreesBelowStart;
        _minDegreesBelowStart = _minDegreesBelowStart < zeroDegrees ? zeroDegrees : _minDegreesBelowStart;
    }
}