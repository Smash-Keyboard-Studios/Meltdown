using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour, IInteractable
{
	public string ObjectName = "Note";

	string IInteractable.ObjectName { get => ObjectName; set => ObjectName = value; }

	public UnityEvent OnInteract;

	public void Interact()
	{
		OnInteract.Invoke();
	}
}
