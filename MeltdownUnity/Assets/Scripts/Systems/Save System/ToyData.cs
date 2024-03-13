using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ToyType
{
    Car,
    Penguin,
    Pig
}

[System.Serializable]
public class ToyData
{
    public string id;

    public ToyType toyType;

    public Vector3 position;

    public Quaternion rotation;
}
