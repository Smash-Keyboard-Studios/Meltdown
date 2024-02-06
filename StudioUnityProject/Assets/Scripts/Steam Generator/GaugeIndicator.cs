using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeIndicator: MonoBehaviour
{
    // Non-Editable Values
    private float[] _rotationPoint = {0.0f, 50.0f, 100.0f, 150.0f, 200.0f, 250.0f};

    private float _rotationIncrement, _nextRotationPoint;
    private int _rotationIndex = 1;

    // Editable Values
    [SerializeField]
    private float _rotationSpeed = 40.0f;

    // Public Values
    public bool MoveToNextPoint = false;

    private void Awake () 
    {
        _nextRotationPoint = _rotationPoint[_rotationIndex];
    }

    private void Update () 
    {
        if (MoveToNextPoint)
        {
            IncrementGauge();
        }
    }

    public void IncrementGauge ()
    {
            if (_rotationIndex < _rotationPoint.Length) // Check that is a valid point to rotate to.
            {
            // Rotation to the next point.
                if (_rotationIncrement < _nextRotationPoint)
                {
                    transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
                    _rotationIncrement += _rotationSpeed * Time.deltaTime;
                }

            // When the point is reached, permission to rotate is turned off and the next point to rotate to is set up.

                else if (_rotationIncrement >= _nextRotationPoint) // not equal to because of imprecision.
                {
                    MoveToNextPoint = false;

                    if (_rotationIndex < _rotationPoint.Length-1) // Check that it isn't the last point.
                    {
                        _rotationIndex++;
                        _nextRotationPoint = _rotationPoint[_rotationIndex];
                    }
                    /* 
                    else 
                    {
                        Now that the gauge is at 100%, call the other script here.
                    }
                     */           
                }
        }


    }

    public void ResetGauge ()
    {
        // Reset rotation
        transform.rotation = Quaternion.Euler(Vector3.zero);

        // Reset gauge-related variables
        _rotationIncrement = 0.0f;
        _rotationIndex = 1;
        _nextRotationPoint = _rotationPoint[_rotationIndex];
        MoveToNextPoint = false;
    }
}
