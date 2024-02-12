// THIS SCRIPT CONTROLS THE MOVEMENT OF THE PIN (HAND) OF THE GAUGE AND SHOULD BE ATTACHED IT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeIndicator: MonoBehaviour
{
    // [Non-Editor Variables]

    private float[] _rotationPoint = {0.0f, 50.0f, 100.0f, 150.0f, 200.0f, 250.0f};

    private float _rotationIncrement, _nextRotationPoint;
    private int _rotationIndex = 1;

    // [Editor Variables]
    
    [Header("<b>Rotation Parameters</b>")]
    [Space]
    [SerializeField] private float _rotationSpeed = 40.0f;

    public bool MoveToNextPoint = false;

    // [Events]

    private void Awake () 
    {
        _nextRotationPoint = _rotationPoint[_rotationIndex];
    }

    private void Update () 
    {
        if (MoveToNextPoint) // Determines if rotation is enabled.
        {
            IncrementGauge();
        }
    }

    // [Custom Methods]

    public void IncrementGauge ()
    {
            if (_rotationIndex < _rotationPoint.Length) // Determines if there is a valid point to rotate to.
            {
            
                // Active rotation towards the next point.
            
                if (_rotationIncrement < _nextRotationPoint)
                {
                    transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
                    _rotationIncrement += _rotationSpeed * Time.deltaTime; 
                }

                // When the point is reached, rotation is disabled, and the next point (to rotate to) is assigned.
            
                else if (_rotationIncrement >= _nextRotationPoint) // Greater than symbol is necessary because of imprecision.
                {
                    MoveToNextPoint = false;

                    if (_rotationIndex < _rotationPoint.Length-1) // Determines if the current point is not the last one.
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
