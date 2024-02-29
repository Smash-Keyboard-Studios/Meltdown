using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class buttonScript : MonoBehaviour, IInteractable
{
	public UnityEvent getInteract;

	[SerializeField] public bool ToggleButton;
	[SerializeField] public bool TimedButton;
	[SerializeField] public int Timer;


	private bool ButtonUsed = false;

	IEnumerator ActivateTimer()
	{
		yield return new WaitForSeconds(Timer);
		if (!ToggleButton)
		{
			ButtonUsed = false;
		}
		getInteract.Invoke();
	}

	//  public void Interact()
	void IInteractable.Interact()
	{
		if (!ToggleButton && ButtonUsed == false)
		{
			ButtonUsed = true;
			if (TimedButton == true)
			{
				StartCoroutine(ActivateTimer());
			}
			else
			{
				getInteract.Invoke();
			}
		}
		else if (ToggleButton)
		{

			if (TimedButton == true)
			{
				StartCoroutine(ActivateTimer());
			}
			else
			{
				getInteract.Invoke();
			}
		}
	}

}
