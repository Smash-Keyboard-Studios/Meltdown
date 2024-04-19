using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeElectrifiedWater : MonoBehaviour
{
    [SerializeField] public bool CurrentFrozenState;

    public GameObject UnfrozenWater;
    public GameObject FrozenWater;

    private void Start()
    {
        CurrentFrozenState = false;
    }

    private void Update()
    {
        if (CurrentFrozenState)
        {
            UnfrozenWater.SetActive(false);
            FrozenWater.SetActive(true);
        }
        else
        {
            UnfrozenWater.SetActive(true);
            FrozenWater.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ice>() != null)
        {
            //change frozen state to active
            CurrentFrozenState = true;            
        }
    }
}
