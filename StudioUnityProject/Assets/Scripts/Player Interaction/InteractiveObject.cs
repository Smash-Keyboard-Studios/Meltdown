using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour, IInteractable
{
	public UnityEvent OnInteract;


	public void Interact()
	{
		OnInteract.Invoke();
	}
}
