using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour, IInteractable
{
	public UnityEvent OnInteract;

	[Header("Object Name when Looking at it")] public string ObjectName = "Please Change the Object Name in the InteractiveObject script";

	string IInteractable.ObjectName { get => ObjectName; set => ObjectName = value; }

	void Start()
	{
		gameObject.tag = "InteractableObject";
	}

	public void Interact()
	{
		OnInteract.Invoke();
	}
}
