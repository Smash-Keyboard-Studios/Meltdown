// THIS SCRIPT CONTROLS THE MOVEMENT OF THE PIN (HAND) OF THE GAUGE AND SHOULD BE ATTACHED IT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using CustomAttributes;

// [Struct Variables for a Serialized Array]

[System.Serializable]
public struct Destination
{
    [Tooltip("The name of the range.")] public string name;
    [Tooltip("The minimum of the range.")] public float minPosition;
    [Tooltip("The maximum of the range.")] public float maxPosition;
    [Tooltip("The midpoint of the range.")][ShowOnly] public float centralPosition;
    [Tooltip("The size of the range.")] [ShowOnly] public float tolerance;
    [Tooltip("Whether the event has been triggered.")] public bool hasBeenReached;
    [Tooltip("The event to trigger.")] public UnityEvent onReached;

    public Destination(string name, float minPosition, float maxPosition)
    {
        this.name = name;
        this.minPosition = minPosition;
        this.maxPosition = maxPosition;
        this.centralPosition = (minPosition + maxPosition) / 2f;
        this.tolerance = Mathf.Abs(minPosition - maxPosition);
        this.hasBeenReached = false;
        this.onReached = new UnityEvent();
    }

    public void Recalculate(float minPosition, float maxPosition)
    {
        this.minPosition = minPosition;
        this.maxPosition = maxPosition;
        this.centralPosition = (minPosition + maxPosition) / 2f;
        this.tolerance = Mathf.Abs(minPosition - maxPosition);
    }
}

public class GaugeIndicator : MonoBehaviour
{
    // [Non-Editor Variables]

    private int _heatIndex, _firstHeatIndex, _secondHeatIndex, _initialHeatIndex, _startHeatIndex, _finalHeatIndex;
    private int _firstCoolIndex, _startCoolIndex, _coolIndex, _finalCoolIndex;
    private float _firstHeatPoint, _initialHeatPoint, _finalHeatPoint, _firstCoolPoint, _finalCoolPoint, _fixedCurrentRotationPoint;
    private float _smallIncrement, _smallDelay;
    private Quaternion _zeroRotation, _defaultRotation, _finalRotation, _descendRotation;
    private bool _didForwardRot = false, _didBackRot = false, _runTimeRepositionStandby = false;
    private float _maxDegreesCopy, _equalHeatCopy, _equalCoolCopy;

    // [Editor Variables]

    [Header("<size=15>Rotation Parameters</size>")]
    [Space]
    [Tooltip("The axis of rotation in x, y, z.")][SerializeField] private Vector3 _rotationAxis = Vector3.back;
    [Space]
    [Tooltip("The degrees offset of the minimum point below the start.")][SerializeField][Range(0f, 360f)] private float _minDegreesBelowStart = 90.0f;
    [Space]
    [Tooltip("The progression a fire hit does in the dual fire-ice scale.")][Range(0, 100)] public int _firePercentage = 40;
    [Tooltip("The regression an ice hit does in the dual fire-ice scale.")][Range(0, 100)] public int _icePercentage = 20;
    [Tooltip("The size of a dual fire-ice scale.")][SerializeField][Range(20f, 360f)] private float _maxDegrees = 180f;
    [Space]
    [Tooltip("The speed of a fire hit.")][SerializeField][Range(0.0f, 100.0f)] private float _heatingSpeed = 40.0f;
    [Tooltip("The speed of an ice hit.")][SerializeField][Range(0.0f, 100.0f)] private float _coolingSpeed = 40.0f;

    [Header("<size=15>Location Parameters</size>")]
    [Space]
    [Tooltip("The range of points where events occur at.")]
    [SerializeField]
    public Destination[] _destinations =
    {
        new Destination("On", 160.0f, 180.0f)
    };
    [Space]
    [Tooltip("Moves according to one fire hit.")] public bool MoveToNextPoint = false;
    [Tooltip("Moves according to one ice hit.")] public bool MoveToPrevPoint = false;
    [Tooltip("Resets to the start position.")] public bool ResetPinLocation = false;
    [Tooltip("Moves to the final position.")] public bool FinalPinLocation = false;
    [Tooltip("Moves to the minimum position.")] public bool MinLocation = false;
    [Tooltip("Last movement used.")][SerializeField][ShowOnly] private string _lastMovement = "";

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

    // [Events]

    private void OnValidate()
    {
        const int zeroLength = 0, tenLength = 10, hundredLength = 100 + 1;
        const float zeroPoint = 0.0f;

        RecalcDestinations(); // Recalculates Central Position and Tolerance.

        if ((!Application.isPlaying || RunTimeReposition)) // Adjusts the min degrees based on the max.
        {
            Invoke("MinDegreesAutoAdjustment", _smallDelay);
        }

        if (_lastMovement == "") // Sets last movement to default point instead of empty.
        {
            _lastMovement = "Default Point";
        }

        // ### Prevents less than 1 rotation point and more than 101 (starting point + 100).
        if (_heatRotationPoints.Length == zeroLength)
        {
            _heatRotationPoints = new float[] { zeroPoint };
        }
        if (_heatRotationPoints.Length > hundredLength)
        {
            Array.Resize(ref _heatRotationPoints, hundredLength);
        }
        if (_coolRotationPoints.Length == zeroLength)
        {
            _coolRotationPoints = new float[] { zeroPoint };
        }
        if (_coolRotationPoints.Length > hundredLength)
        {
            Array.Resize(ref _coolRotationPoints, hundredLength);
        }

        // ### Prevents more than 10 destinations.
        if (_destinations.Length > tenLength)
        {
            Array.Resize(ref _destinations, tenLength);
        }
    }

    private void Awake()
    {
        // ##### Sets Equal Points to rotate to.
        SetEqualHeatPoints();
        SetEqualCoolPoints();

        // ##### Recalculates Central Position and Tolerance.
        RecalcDestinations();

        // ##### Sets Local Start Co-ordinates to override.
        if (_setStartPosition)
        {
            transform.localPosition = _startPosition;
        }
        if (_setStartRotation)
        {
            transform.localRotation = Quaternion.Euler(_startRotation);
        }

        // ##### Sets Indexes of Arrays.
        _firstHeatIndex = 0;
        _firstCoolIndex = 0;
        _secondHeatIndex = _firstHeatIndex + 1;
        _initialHeatIndex = GetInitialIndex(); // Gets nearest index to 0 degrees.
        _finalHeatIndex = _heatRotationPoints.Length - 1;
        _finalCoolIndex = _coolRotationPoints.Length - 1;
        _startHeatIndex = _initialHeatIndex != _finalHeatIndex ? _initialHeatIndex + 1 : _initialHeatIndex;
        _heatIndex = _startHeatIndex;

        // ##### Sets Elements of Array
        _firstHeatPoint = _heatRotationPoints[_firstHeatIndex];
        _firstCoolPoint = _coolRotationPoints[_firstCoolIndex];
        _initialHeatPoint = _heatRotationPoints[_initialHeatIndex];
        _finalHeatPoint = _heatRotationPoints[_finalHeatIndex];
        _finalCoolPoint = _coolRotationPoints[_finalCoolIndex];
        _nextRotationPoint = _heatRotationPoints[_heatIndex];
        _prevRotationPoint = _firstCoolPoint; // Value for the purpose of FindCoolIndex.
        _currentRotationPoint = _heatRotationPoints[_initialHeatIndex];
        _fixedCurrentRotationPoint = _currentRotationPoint;
        _currentStop = _currentRotationPoint;

        // ##### Sets these Indexes using Prev/Current Rotation points.
        _startCoolIndex = FindCoolIndex(_initialHeatIndex);
        _coolIndex = _startCoolIndex;
        SetRotationPoints(_heatIndex, _coolIndex);

        // ##### Sets Number of Calls to Repeat.
        SetFireCalls();
        SetIceCalls();

        // ##### Sets Rotation Transforms.
        _zeroRotation = transform.rotation; // 0 degrees on the scales.
        _defaultRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _initialHeatPoint);
        _finalRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _finalHeatPoint);
        if (HeatOnlyScale)
        {
            _descendRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _firstHeatPoint);
        }
        else
        {
            _descendRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _firstCoolPoint);
        }

        transform.rotation = _defaultRotation; // Goes to starting point.
        _lastMovement = "Default Point";

        // ##### Sets Corrective Variables
        _smallIncrement = 0.2f;
        _smallDelay = 0.1f;
    }

    private void Update()
    {
        const int noCalls = 0;
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
        else if (MoveToNextPoint && FireCalls != noCalls) // Determines if forward rotation is enabled.
        {
            // ##### Cancels if at the end.
            if (transform.rotation == _finalRotation && _nextRotationPoint == _finalHeatPoint)
            {
                MoveToNextPoint = false;
                return;
            }
            MoveToPrevPoint = false;
            IncrementGauge();

        }
        else if (AutoCoolOn || (MoveToPrevPoint && (HeatOnlyScale && FireCalls != noCalls || !HeatOnlyScale && IceCalls != noCalls))) // Cools down (reverse rotation) to origin if not.
        {
            // ##### Cancels if at the start.
            if (transform.rotation == _descendRotation && ((HeatOnlyScale && _prevRotationPoint == _firstHeatPoint) || (_prevRotationPoint == _firstCoolPoint)))
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

    // ########## Functions related to movement.
    public void IncrementGauge()
    {
        if (_heatIndex <= _finalHeatIndex) // Determines if there is a valid point to rotate to.
        {
            float sign = Mathf.Sign(_nextRotationPoint - _fixedCurrentRotationPoint);
            const float pos = 1.0f, neg = -1.0f;
            const int noCalls = 0;

            bool incPosRotation = sign == pos && _currentRotationPoint < _nextRotationPoint;
            bool incNegRotation = sign == neg && _currentRotationPoint > _nextRotationPoint;

            // ##### Greater than symbol is necessary because of imprecision.
            bool completePosRotation = sign == pos && _currentRotationPoint >= _nextRotationPoint;
            bool completeNegRotation = sign == neg && _currentRotationPoint <= _nextRotationPoint;

            if (incPosRotation || incNegRotation)
            {
                if (RunTimeReposition && !_runTimeRepositionStandby)
                {
                    UpdateRotationPoints(IncrementStatus.Transitioned);
                    _runTimeRepositionStandby = true;
                }

                if (!InstantRotation)
                {
                    transform.Rotate(_rotationAxis, _heatingSpeed * Time.deltaTime * sign);
                    _currentRotationPoint += _heatingSpeed * Time.deltaTime * sign;
                    _currentStop = _currentRotationPoint;

                    // ### Corrects rotation to default/end point. Needs to be this long because it won't get assigned quick enough in a local variable.
                    if (_nextRotationPoint == _finalHeatPoint && ((sign == pos && _currentRotationPoint > _finalHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _finalHeatPoint + _smallIncrement)))
                    {
                        CorrectPoint("MaxGauge", _heatIndex, _finalHeatIndex, "Next Point");
                    }
                    else if (_nextRotationPoint == _initialHeatPoint && _remainingFireCalls == noCalls && ((sign == pos && _currentRotationPoint > _initialHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _initialHeatPoint + _smallIncrement)))
                    {
                        CorrectPoint("ResetGauge", _heatIndex, _firstHeatIndex, "Next Point");
                    }
                }
                else
                {
                    JumpToNextRotationPoint();
                    _lastMovement = "Next Point";

                    // ##### Corrects rotation to default/end point.
                    if (_nextRotationPoint == _finalHeatPoint)
                    {
                        CorrectPoint("MaxGauge", _heatIndex, _finalHeatIndex, "Next Point");
                    }
                    else if (_nextRotationPoint == _initialHeatPoint && _remainingFireCalls == noCalls)
                    {
                        CorrectPoint("ResetGauge", _heatIndex, _firstHeatIndex, "Next Point");
                    }
                }
                _didForwardRot = true;
            }
            else if (completePosRotation || completeNegRotation)  // When the point is reached, rotation is disabled, and the next points (to rotate to) are assigned.
            {
                JumpToNextRotationPoint(); // corrects the imprecision
                _lastMovement = "Next Point";

                // ##### Previous backward rotation check.
                if (!_didBackRot || !HeatOnlyScale)
                {
                    MoveToNextPoint = false;
                }
                else
                {
                    _didBackRot = false;
                    _remainingFireCalls++;
                }

                _runTimeRepositionStandby = !RunTimeReposition; // Allows the RunTimeReposition to happen again if enabled.

                _heatIndex += (_heatIndex < _finalHeatIndex) ? 1 : 0; // Increases _heatIndex unless end.
                SetRotationPoints(_heatIndex, _coolIndex);

                AutoCoolOn = false;

                // ##### Repeated Increment calls.
                if (_remainingFireCalls > noCalls)
                {
                    _remainingFireCalls--;
                    MoveToNextPoint = true;
                }
                else
                {
                    CheckForDestination();
                    Invoke("SetFireCalls", _smallDelay);
                    if (!HeatOnlyScale)
                    {
                        Invoke("SetIceCalls", _smallDelay);
                    }
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
        float sign = Mathf.Sign(_prevRotationPoint - _fixedCurrentRotationPoint);
        const float pos = 1.0f, neg = -1.0f;
        const int noCalls = 0;

        bool decPosRotation = sign == pos && _currentRotationPoint < _prevRotationPoint;
        bool decNegRotation = sign == neg && _currentRotationPoint > _prevRotationPoint;

        bool completePosRotation = sign == pos && _currentRotationPoint >= _prevRotationPoint;
        bool completeNegRotation = sign == neg && _currentRotationPoint <= _prevRotationPoint;

        if (decPosRotation || decNegRotation) // Active rotation towards the previous point.
        {
            if (RunTimeReposition && !_runTimeRepositionStandby)
            {
                UpdateRotationPoints(IncrementStatus.Transitioned);
                _runTimeRepositionStandby = true;
            }

            if (!InstantRotation)
            {
                transform.Rotate(_rotationAxis, _coolingSpeed * Time.deltaTime * sign);
                _currentRotationPoint += _coolingSpeed * Time.deltaTime * sign;
                _currentStop = _currentRotationPoint;

                // ##### Corrects rotation to default/minimum. They need to be this long because it won't get assigned quick enough in a local variable.
                if (HeatOnlyScale)
                {
                    if ((_prevRotationPoint == _firstHeatPoint) && ((sign == pos && _currentRotationPoint > _firstHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _firstHeatPoint + _smallIncrement)))
                    {
                        CorrectPoint("MinGauge", _heatIndex, _secondHeatIndex, "Previous Point");
                    }
                    else if ((_prevRotationPoint == _initialHeatPoint) && _remainingFireCalls == noCalls && ((sign == pos && _currentRotationPoint > _initialHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _initialHeatPoint + _smallIncrement)))
                    {
                        CorrectPoint("ResetGauge", _heatIndex, _startHeatIndex, "Previous Point");
                    }
                }
                else
                {
                    if ((_prevRotationPoint == _firstCoolPoint) && ((sign == pos && _currentRotationPoint > _firstCoolPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _firstCoolPoint + _smallIncrement)))
                    {
                        CorrectPoint("MinGauge", _coolIndex, _firstCoolIndex, "Previous Point");
                    }
                    else if ((_prevRotationPoint == _initialHeatPoint) && _remainingIceCalls == noCalls && ((sign == pos && _currentRotationPoint > _initialHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _initialHeatPoint + _smallIncrement)))
                    {
                        CorrectPoint("ResetGauge", _coolIndex - 1, _startCoolIndex, "Previous Point");
                    }
                }
            }
            else
            {
                JumpToPrevRotationPoint();
                _lastMovement = "Previous Point";

                // ##### Corrects rotation to default/minimum.
                if (HeatOnlyScale)
                {
                    if (_prevRotationPoint == _firstHeatPoint)
                    {
                        CorrectPoint("MinGauge", _heatIndex, _secondHeatIndex, "Previous Point");
                    }
                    else if (_prevRotationPoint == _initialHeatPoint && _remainingFireCalls == noCalls)
                    {
                        CorrectPoint("ResetGauge", _heatIndex, _startHeatIndex, "Previous Point");
                    }
                }
                else
                {
                    if (_prevRotationPoint == _firstCoolPoint)
                    {
                        CorrectPoint("MinGauge", _coolIndex, _firstCoolIndex, "Previous Point");
                    }
                    else if (_prevRotationPoint == _initialHeatPoint && _remainingIceCalls == noCalls)
                    {
                        CorrectPoint("ResetGauge", _coolIndex - 1, _startCoolIndex, "Previous Point");
                    }
                }
            }

            _didBackRot = true;
        }
        else if (completePosRotation || completeNegRotation) // When the point is reached, rotation is disabled, and the next points (to rotate to) are assigned.
        {
            JumpToPrevRotationPoint(); // Corrects the imprecision.
            _lastMovement = "Previous Point";

            // ##### Previous forward rotation check.
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
                _remainingFireCalls++;
            }

            _runTimeRepositionStandby = !RunTimeReposition; // Allows the RunTimeReposition to happen again if enabled.

            if (HeatOnlyScale)
            {
                _heatIndex -= (_heatIndex > _secondHeatIndex) ? 1 : 0; // Decreases _heatIndex unless earliest.
            }
            _coolIndex -= (_coolIndex > _firstCoolIndex) ? 1 : 0; // Decreases _coolIndex unless start.
            SetRotationPoints(_heatIndex, _coolIndex);

            // ##### Repeated Decrement calls.
            if (HeatOnlyScale)
            {
                if (_remainingFireCalls > noCalls)
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

                if (_remainingIceCalls > noCalls)
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
    public void HeatGauge()
    {
        MoveToNextPoint = true;
    }
    public void CoolGauge()
    {
        MoveToPrevPoint = true;
    }
    public void AwakenCoolDown()
    {
        if (!MoveToNextPoint) // Sets the cooldown on if not moving.
        {
            AutoCoolOn = true;
        }
    }
    public void ResetGauge()
    {
        if (RunTimeReposition)
        {
            UpdateRotationPoints(IncrementStatus.Set);
        }

        // ##### Resets rotation.
        transform.rotation = _defaultRotation;

        // ##### Resets tracking-related variables
        _currentRotationPoint = _initialHeatPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
        _currentStop = _currentRotationPoint;
        _heatIndex = _startHeatIndex;
        _coolIndex = _startCoolIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        if (!HeatOnlyScale)
        {
            Invoke("SetIceCalls", _smallDelay);
        }
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        CheckForDestination();
        _lastMovement = "Default Point";
    }
    public void MinGauge()
    {
        if (RunTimeReposition)
        {
            UpdateRotationPoints(IncrementStatus.Set);
        }

        // ##### Completes rotation.
        transform.rotation = _descendRotation;

        // ##### Sets tracking-related variables to minimum point.
        _currentRotationPoint = HeatOnlyScale ? _firstHeatPoint : _firstCoolPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
        _currentStop = _currentRotationPoint;
        _coolIndex = _firstCoolIndex;
        _heatIndex = (_firstCoolPoint == _firstHeatPoint || HeatOnlyScale) ? _secondHeatIndex : _firstHeatIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        if (!HeatOnlyScale)
        {
            Invoke("SetIceCalls", _smallDelay);
        }
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        AutoCoolOn = false;
        CheckForDestination();
        _lastMovement = "Min Point";
    }
    public void MaxGauge()
    {
        if (RunTimeReposition)
        {
            UpdateRotationPoints(IncrementStatus.Set);
        }

        // ##### Completes rotation.
        transform.rotation = _finalRotation;

        // ##### Sets tracking-related variables to final point.
        _currentRotationPoint = _finalHeatPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
        _currentStop = _currentRotationPoint;
        _nextRotationPoint = _currentRotationPoint;
        _heatIndex = _finalHeatIndex;
        _coolIndex = FindCoolIndex(_heatIndex);
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        if (!HeatOnlyScale)
        {
            Invoke("SetIceCalls", _smallDelay);
        }
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        AutoCoolOn = false;
        if (AutoCoolTriggerOn)
        {
            Invoke("AwakenCoolDown", _autoCoolDelay);
        }
        CheckForDestination();
        _lastMovement = "Final Point";
    }

    // ########## Functions that set the number of Fire/Ice Calls and calculates the next stop after successive calls.
    private void SetFireCalls()
    {
        FireCalls = _customScale ? FireCalls : _firePercentage;
        _remainingFireCalls = FireCalls - 1;
        _nextStop = (_heatIndex + _remainingFireCalls) > _finalHeatIndex ? _finalHeatPoint : _heatRotationPoints[_heatIndex + _remainingFireCalls];
        if (HeatOnlyScale)
        {
            _prevStop = (_heatIndex - 1 - _remainingFireCalls) < _firstHeatIndex ? _firstHeatPoint : _heatRotationPoints[_heatIndex - 1 - _remainingFireCalls];

            // ##### Back/Forward Rotation Adjustments
            _nextStop = (_nextStop != _finalHeatPoint && _didBackRot) ? _heatRotationPoints[_heatIndex + _remainingFireCalls + 1] : _nextStop;
            _prevStop = (_prevStop != _firstHeatPoint && _didForwardRot) ? _heatRotationPoints[_heatIndex - 1 - _remainingFireCalls - 1] : _prevStop;
        }
    }
    private void SetIceCalls()
    {
        IceCalls = _customScale ? IceCalls : _icePercentage;
        _remainingIceCalls = IceCalls - 1;
        _prevStop = (_coolIndex - _remainingIceCalls) < _firstCoolIndex ? _firstCoolPoint : _coolRotationPoints[_coolIndex - _remainingIceCalls];
    }

    //  ########## Function for Run-Time Reposition.
    public enum IncrementStatus { Set, Transitioned };

    private void UpdateRotationPoints(IncrementStatus status)
    {
        // ##### Sets Equal Points to rotate to.
        SetEqualHeatPoints();
        SetEqualCoolPoints();

        // ##### Sets Indexes of Array
        _initialHeatIndex = GetInitialIndex();
        _finalHeatIndex = _heatRotationPoints.Length - 1;
        _finalCoolIndex = _coolRotationPoints.Length - 1;
        _startHeatIndex = _initialHeatIndex != _finalHeatIndex ? _initialHeatIndex + 1 : _initialHeatIndex;

        // ##### Sets Elements of Array
        _firstHeatPoint = _heatRotationPoints[_firstHeatIndex];
        _firstCoolPoint = _coolRotationPoints[_firstCoolIndex];
        _initialHeatPoint = _heatRotationPoints[_initialHeatIndex];
        _finalHeatPoint = _heatRotationPoints[_finalHeatIndex];
        _finalCoolPoint = _coolRotationPoints[_finalCoolIndex];

        // ##### Sets this Index using Prev/Current Rotation points.
        _startCoolIndex = FindCoolIndex(_initialHeatIndex);

        // ##### If out of bounds, sets to the last point. Minus one if it will be incremented afterwards.
        if (_heatIndex > _finalHeatIndex)
        {
            bool isEndPoint = status == IncrementStatus.Set;
            _heatIndex = isEndPoint ? _finalHeatIndex : _finalHeatIndex - 1;
            _coolIndex = FindCoolIndex(_heatIndex);
        }

        // ##### Rotation Transforms.
        _defaultRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _initialHeatPoint);
        _finalRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _finalHeatPoint);
        if (HeatOnlyScale)
        {
            _descendRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _firstHeatPoint);
        }
        else
        {
            _descendRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _firstCoolPoint);
        }
    }

    // ########## Function that updates the points to rotate to.
    private void SetRotationPoints(int i, int j)
    {
        // ##### Check for invalid indexes.
        int newFinalHeatIndex = _heatRotationPoints.Length - 1, newFinalCoolIndex = _coolRotationPoints.Length - 1;

        if (i > newFinalHeatIndex || i < _firstHeatIndex)
        {
            ResetGauge();
            return;
        }

        if (!HeatOnlyScale && (j > newFinalCoolIndex || j < _firstCoolIndex))
        {
            MinGauge();
            return;
        }

        // ##### Permutations to set next rotation point.
        if (HeatOnlyScale || _didForwardRot || _currentRotationPoint == _firstCoolPoint || _currentRotationPoint == _initialHeatPoint || _currentRotationPoint == _finalHeatPoint)
        {
            _nextRotationPoint = _heatRotationPoints[i];
        }
        else if (_didBackRot)
        {
            _heatIndex = FindHeatIndex(j + 1);
            _nextRotationPoint = _heatRotationPoints[_heatIndex];
        }

        // ##### Permutations to set previous rotation point.
        if (HeatOnlyScale)
        {
            if (i > 0)
            {
                _prevRotationPoint = _heatRotationPoints[i - 1];
            }
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

    // ########## Functions to find points (initial, and next and previous points with the cool scale).
    private int GetInitialIndex()
    {
        float approxStartingPoint = 0f;

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
        return 0;
    }
    private int FindHeatIndex(int j)
    {
        float sign = Mathf.Sign(_nextRotationPoint - _fixedCurrentRotationPoint);
        const float pos = 1.0f, neg = -1.0f;

        int nextIndex = _heatIndex;
        float minDifference = sign == pos ? _finalCoolPoint - _firstHeatPoint : _firstHeatPoint - _finalCoolPoint;

        // ##### Finds a heat rotation point nearest the previous cool point to rotate to)
        for (int i = _firstHeatIndex; i < _heatRotationPoints.Length; i++)
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
    private int FindCoolIndex(int i)
    {
        float sign = Mathf.Sign(_nextRotationPoint - _fixedCurrentRotationPoint);
        const float pos = 1.0f, neg = -1.0f;

        int nextIndex = _coolIndex;
        float minDifference = sign == pos ? _finalHeatPoint - _firstCoolPoint : _firstCoolPoint - _finalHeatPoint;

        // ##### Finds a cool rotation point nearest the previous heat point to rotate to.
        for (int j = _firstCoolIndex; j < _coolRotationPoints.Length; j++)
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

    // ########## Functions to correct rotation points.
    private void JumpToNextRotationPoint()
    {

        transform.Rotate(_rotationAxis, _nextRotationPoint - _currentRotationPoint);
        transform.localRotation = RoundRotationPoint(transform.localRotation);
        _currentRotationPoint = _nextRotationPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
        _currentStop = _currentRotationPoint;
    }
    private void JumpToPrevRotationPoint()
    {
        transform.Rotate(_rotationAxis, _prevRotationPoint - _currentRotationPoint);
        transform.localRotation = RoundRotationPoint(transform.localRotation);
        _currentRotationPoint = _prevRotationPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
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
    private void CorrectPoint(string setFunction, int rotationIndex, int positionIndex, string point)
    {
        // ##### Invokes Min/Max/Reset Gauge if right index, else jumps to the correct point without invoking.
        if (rotationIndex == positionIndex)
        {
                Invoke(setFunction, _smallDelay);
        }
        else
        {
            if (point == "Next Point")
            {
                JumpToNextRotationPoint();
            }
            else if (point == "Previous Point")
            {
                JumpToPrevRotationPoint();
            }
            _lastMovement = point;
        }
    }

    // ########## Functions to handle the destination triggers.
    private void RecalcDestinations()
    {
        for (int i = 0; i < _destinations.Length; i++)
        {
            _destinations[i].Recalculate(_destinations[i].minPosition, _destinations[i].maxPosition);
        }
    }
    private void CheckForDestination()
    {
        for (int i = 0; i < _destinations.Length; i++)
        {
            if (!_destinations[i].hasBeenReached && _currentRotationPoint >= _destinations[i].minPosition && _currentRotationPoint <= _destinations[i].maxPosition)
            {
                _destinations[i].hasBeenReached = true;
                _destinations[i].onReached.Invoke();
            }
        }
    }

    // ########## Functions to set equidistant points.
    private void SetEqualHeatPoints()
    {
        SetPoints(ref _heatRotationPoints, ref _firstHeatIndex, ref _equalHeatPoints, ref _equalHeatEndPoint);
    }
    private void SetEqualCoolPoints()
    {
        SetPoints(ref _coolRotationPoints, ref _firstCoolIndex, ref _equalCoolPoints, ref _equalCoolEndPoint);
    }
    private void SetPoints(ref float[] rotationPoints, ref int firstIndex, ref int equalPoints, ref float equalEndPoint)
    {
        const int oneHundredPercent = 100, oneStartPoint = 1, nothing = 0;

        // ##### Uses a unified percentage scale by default and calls x number of times for the 100 + 1 points.
        if (!_customScale) 
        {
            equalPoints = oneHundredPercent;
            equalEndPoint = _maxDegrees;
        }

        // ##### Sets equidistant rotation points.
        int numberOfPoints = equalPoints + oneStartPoint;

        if (equalPoints > nothing)
        {
            rotationPoints = new float[numberOfPoints];
            for (int i = firstIndex; i < numberOfPoints; i++)
            {
                rotationPoints[i] = i * (equalEndPoint / equalPoints) - _minDegreesBelowStart;
            }
        }
    }

    // ########## Function to prevent Min Degrees Below Start from reaching Max Degrees (or EndPoints) and adapts to Max Degrees changing.
    private void MinDegreesAutoAdjustment()
    {
        const float zeroDegrees = 0f, oneDegree = 1f, threeSixtyDegrees = 360f;

        // ##### Gives initial values.
        _maxDegreesCopy = _maxDegreesCopy == zeroDegrees ? _maxDegrees : _maxDegreesCopy;
        _equalHeatCopy = _equalHeatCopy == zeroDegrees ? _equalHeatEndPoint : _equalHeatCopy;
        _equalCoolCopy = _equalCoolCopy == zeroDegrees ? _equalCoolEndPoint : _equalCoolCopy;

        // ##### Applies changes if the Max degrees is exceeded or the Max degrees changes.
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

        // ##### Clamps to the 360 degree range.
        _minDegreesBelowStart = _minDegreesBelowStart > threeSixtyDegrees ? (threeSixtyDegrees - oneDegree) : _minDegreesBelowStart;
        _minDegreesBelowStart = _minDegreesBelowStart < zeroDegrees ? zeroDegrees : _minDegreesBelowStart;
    }
}