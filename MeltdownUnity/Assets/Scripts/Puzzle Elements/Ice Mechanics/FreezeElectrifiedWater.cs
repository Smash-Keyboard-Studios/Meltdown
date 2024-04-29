using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeElectrifiedWater : MonoBehaviour
{
    const int TimerResetTime = 20;

    [SerializeField] public bool CurrentFrozenState;

    public GameObject UnfrozenWater;
    public GameObject FrozenWater;

    private int Timer;

    private void Start()
    {
        CurrentFrozenState = false;
        Timer = TimerResetTime;
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

            //reset timer and run melt timer
            Timer = TimerResetTime;
            StartCoroutine("MeltTimer");
        }

        if (other.gameObject.GetComponent<Fire>() != null)
        {
            //change frozen state to active
            CurrentFrozenState = false;
        }
    }

    IEnumerator MeltTimer()
    {
        while (Timer > 0)
        {
            yield return new WaitForSeconds(1);
            Timer--;
        }
        CurrentFrozenState = false;
    }
}
