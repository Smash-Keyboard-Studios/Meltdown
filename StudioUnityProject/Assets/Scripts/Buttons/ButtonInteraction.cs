using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public doorController drController;

    private bool doorOpen = false;
    [SerializeField] private bool toggleButton = false;
    [SerializeField] private bool timedButton = false;
    [SerializeField] private bool onceButton = false;

    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;

    private void Start()
    {
        drController = GetComponent<doorController>();
    }

    private IEnumerator PauseDoorInteraction ()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }

    public void OpenDoor()
    {
        if (toggleButton == true)
        {
            if (!doorOpen && !pauseInteraction)
            {
                drController.ToggleDoorOpen();
                doorOpen = true;
                StartCoroutine(PauseDoorInteraction());
            }
            else if (doorOpen && !pauseInteraction)
            {
                drController.ToggleDoorOpen();
                doorOpen = false;
                StartCoroutine(PauseDoorInteraction());
            }
        }

        if (onceButton == true)
        {
            drController.ToggleDoorOpen();
        }
    }
}
