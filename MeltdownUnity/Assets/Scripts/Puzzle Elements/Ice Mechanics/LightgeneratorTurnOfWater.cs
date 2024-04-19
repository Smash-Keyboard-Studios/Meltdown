using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightgeneratorTurnOfWater : MonoBehaviour
{
    [Header("select the electrified water that will trigger the light to go off")] public FreezeElectrifiedWater FreezeElectrifiedWater;

    Light Light;

    // Start is called before the first frame update
    void Start()
    {
        Light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FreezeElectrifiedWater.CurrentFrozenState)
        {
            Light.gameObject.SetActive(false);
        }
        else
        {
            Light.gameObject.SetActive(true);
        }
    }
}
