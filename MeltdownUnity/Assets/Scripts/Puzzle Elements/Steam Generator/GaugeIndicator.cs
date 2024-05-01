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
    [Tooltip("The minimum of the range.")][Range(-360.0f, 360.0f)] public float minPosition;
    [Tooltip("The maximum of the range.")][Range(-360.0f, 360.0f)] public float maxPosition;
    [Tooltip("Whether the event has been triggered.")] public bool hasBeenReached;
    [Tooltip("The event to trigger.")] public UnityEvent onReached;

    public Destination(string name, float minPosition, float maxPosition)
    {
        this.name = name;
        this.minPosition = minPosition;
        this.maxPosition = maxPosition;
        this.hasBeenReached = false;
        this.onReached = new UnityEvent();
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
    private bool _didForwardRot = false, _didBackRot = false, _correctedPoint = false, customScale;
    private float _prevRotationPoint, _currentRotationPoint, _nextRotationPoint;
    private float[] _heatRotationPoints = { 0.0f }, _coolRotationPoints = { 0.0f };
    private float _equalHeatEndPoint, _equalCoolEndPoint, _maxDegreesCopy;
    private int _equalHeatPoints, _equalCoolPoints, _remainingFireCalls, _remainingIceCalls;

    // [Editor Variables]

    [Header("<size=15>Rotation Parameters</size>")]
    [Space]
    [Tooltip("The axis of rotation in x, y, z.")][SerializeField] private Vector3 _rotationAxis = Vector3.back;
    [Space]
    [Tooltip("The degrees offset of the minimum point below the start.")][SerializeField][Range(0f, 360f)] private float _minDegreesBelowStart = 90f;
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

    // [Events]

    private void OnValidate()
    {
        const int zeroLength = 0, tenLength = 10, hundredLength = 100 + 1;
        const float zeroPoint = 0.0f;

        // ##### Prevents less than 1 rotation point and more than 101 (starting point + 100).
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

        // ##### Prevents more than 10 destinations.
        if (_destinations.Length > tenLength)
        {
            Array.Resize(ref _destinations, tenLength);
        }

        // ##### Rounding when scrubbing.
        _minDegreesBelowStart = Mathf.Round(_minDegreesBelowStart);
        _maxDegrees = Mathf.Round(_maxDegrees);
        _heatingSpeed = RoundToOneDP(_heatingSpeed);
        _coolingSpeed = RoundToOneDP(_coolingSpeed);
    }

    private void Awake()
    {
        // ##### Sets Equal Points to rotate to.
        SetEqualHeatPoints();
        SetEqualCoolPoints();

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
        _descendRotation = _zeroRotation * Quaternion.Euler(_rotationAxis * _firstCoolPoint);

        transform.rotation = _defaultRotation; // Goes to starting point.

        // ##### Sets Corrective Variables
        _smallIncrement = 0.2f;
        _smallDelay = 0.1f;
    }

    private void Update()
    {
        const int zero = 0;
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
            MinGauge();
        }
        else if (MoveToNextPoint && _firePercentage != zero && !_correctedPoint) // Determines if forward rotation is enabled.
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
        else if (MoveToPrevPoint && _icePercentage != zero && !_correctedPoint) // Cools down (reverse rotation) to origin if not.
        {
            // ##### Cancels if at the start.
            if (transform.rotation == _descendRotation && _prevRotationPoint == _firstCoolPoint)
            {
                MoveToPrevPoint = false;
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
            bool completePosRotation = sign == pos && !_correctedPoint && _currentRotationPoint >= _nextRotationPoint;
            bool completeNegRotation = sign == neg && !_correctedPoint && _currentRotationPoint <= _nextRotationPoint;

            if (incPosRotation || incNegRotation)
            {
                transform.Rotate(_rotationAxis, _heatingSpeed * Time.deltaTime * sign);
                _currentRotationPoint += _heatingSpeed * Time.deltaTime * sign;

                // ### Corrects rotation to default/end point. Needs to be this long because it won't get assigned quick enough in a local variable.
                if (_nextRotationPoint == _finalHeatPoint && ((sign == pos && _currentRotationPoint > _finalHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _finalHeatPoint + _smallIncrement)))
                {
                    CorrectPoint("MaxGauge", _heatIndex, _finalHeatIndex, "Next Point");
                }
                else if (_nextRotationPoint == _initialHeatPoint && _remainingFireCalls == noCalls && ((sign == pos && _currentRotationPoint > _initialHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _initialHeatPoint + _smallIncrement)))
                {
                    CorrectPoint("ResetGauge", _heatIndex, _firstHeatIndex, "Next Point");
                }
                _didForwardRot = true;
            }
            else if (completePosRotation || completeNegRotation)  // When the point is reached, rotation is disabled, and the next points (to rotate to) are assigned.
            {
                JumpToNextRotationPoint(); // corrects the imprecision

                MoveToNextPoint = false;

                _heatIndex += (_heatIndex < _finalHeatIndex) ? 1 : 0; // Increases _heatIndex unless end.
                SetRotationPoints(_heatIndex, _coolIndex);

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
                    Invoke("SetIceCalls", _smallDelay);
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

        bool completePosRotation = sign == pos && !_correctedPoint && _currentRotationPoint >= _prevRotationPoint;
        bool completeNegRotation = sign == neg && !_correctedPoint && _currentRotationPoint <= _prevRotationPoint;

        if (decPosRotation || decNegRotation) // Active rotation towards the previous point.
        {
            transform.Rotate(_rotationAxis, _coolingSpeed * Time.deltaTime * sign);
            _currentRotationPoint += _coolingSpeed * Time.deltaTime * sign;

            // ##### Corrects rotation to default/minimum. They need to be this long because it won't get assigned quick enough in a local variable.
            if ((_prevRotationPoint == _firstCoolPoint) && ((sign == pos && _currentRotationPoint > _firstCoolPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _firstCoolPoint + _smallIncrement)))
            {
                CorrectPoint("MinGauge", _coolIndex, _firstCoolIndex, "Previous Point");
            }
            else if ((_prevRotationPoint == _initialHeatPoint) && _remainingIceCalls == noCalls && ((sign == pos && _currentRotationPoint > _initialHeatPoint - _smallIncrement) || (sign == neg && _currentRotationPoint < _initialHeatPoint + _smallIncrement)))
            {
                CorrectPoint("ResetGauge", _coolIndex - 1, _startCoolIndex, "Previous Point");
            }
            _didBackRot = true;
        }
        else if (completePosRotation || completeNegRotation) // When the point is reached, rotation is disabled, and the next points (to rotate to) are assigned.
        {
            JumpToPrevRotationPoint(); // Corrects the imprecision.

            MoveToPrevPoint = false;
            _didForwardRot = false;

            _coolIndex -= (_coolIndex > _firstCoolIndex) ? 1 : 0; // Decreases _coolIndex unless start.
            SetRotationPoints(_heatIndex, _coolIndex);

            // ##### Repeated Decrement calls.

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
    public void HeatGauge()
    {
        MoveToNextPoint = true;
    }
    public void CoolGauge()
    {
        MoveToPrevPoint = true;
    }
    public void ResetGauge()
    {
        // ##### Resets rotation.
        transform.rotation = _defaultRotation;

        // ##### Resets tracking-related variables
        _currentRotationPoint = _initialHeatPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
        _heatIndex = _startHeatIndex;
        _coolIndex = _startCoolIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        Invoke("SetIceCalls", _smallDelay);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        CheckForDestination();
        _correctedPoint = false;
    }
    public void MinGauge()
    {
        // ##### Completes rotation.
        transform.rotation = _descendRotation;

        // ##### Sets tracking-related variables to minimum point.
        _currentRotationPoint = _firstCoolPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
        _coolIndex = _firstCoolIndex;
        _heatIndex = _firstCoolPoint == _firstHeatPoint ? _secondHeatIndex : _firstHeatIndex;
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        Invoke("SetIceCalls", _smallDelay);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        CheckForDestination();
        _correctedPoint = false;
    }
    public void MaxGauge()
    {
        // ##### Completes rotation.
        transform.rotation = _finalRotation;

        // ##### Sets tracking-related variables to final point.
        _currentRotationPoint = _finalHeatPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
        _nextRotationPoint = _currentRotationPoint;
        _heatIndex = _finalHeatIndex;
        _coolIndex = FindCoolIndex(_heatIndex);
        SetRotationPoints(_heatIndex, _coolIndex);
        Invoke("SetFireCalls", _smallDelay);
        Invoke("SetIceCalls", _smallDelay);
        MoveToNextPoint = false;
        MoveToPrevPoint = false;
        _didForwardRot = false;
        _didBackRot = false;
        CheckForDestination();
        _correctedPoint = false;
    }

    // ########## Functions that set the number of Fire/Ice Calls and calculates the next stop after successive calls.
    private void SetFireCalls()
    {
        _remainingFireCalls = _firePercentage - 1;
    }
    private void SetIceCalls()
    {
        _remainingIceCalls = _icePercentage - 1;
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

        if (j > newFinalCoolIndex || j < _firstCoolIndex)
        {
            MinGauge();
            return;
        }

        // ##### Permutations to set next rotation point.
        if (_didForwardRot || _currentRotationPoint == _firstCoolPoint || _currentRotationPoint == _initialHeatPoint || _currentRotationPoint == _finalHeatPoint)
        {
            _nextRotationPoint = _heatRotationPoints[i];
        }
        else if (_didBackRot)
        {
            _heatIndex = FindHeatIndex(j + 1);
            _nextRotationPoint = _heatRotationPoints[_heatIndex];
        }

        // ##### Permutations to set previous rotation point.
        if ((_didBackRot && !_didForwardRot) || _currentRotationPoint == _firstCoolPoint || _currentRotationPoint == _initialHeatPoint || _currentRotationPoint == _finalHeatPoint)
        {
            _prevRotationPoint = _coolRotationPoints[j];
        }
        else if (_didForwardRot && i > 0)
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
    }
    private void JumpToPrevRotationPoint()
    {
        transform.Rotate(_rotationAxis, _prevRotationPoint - _currentRotationPoint);
        transform.localRotation = RoundRotationPoint(transform.localRotation);
        _currentRotationPoint = _prevRotationPoint;
        _fixedCurrentRotationPoint = _currentRotationPoint;
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
            _correctedPoint = true;
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
        }
    }

    // ########## Function to handle the destination triggers.
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

        equalPoints = oneHundredPercent;
        equalEndPoint = _maxDegrees;

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
}
