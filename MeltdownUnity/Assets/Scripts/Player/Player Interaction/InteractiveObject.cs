using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour, IInteractable
{
	public UnityEvent OnInteract;

	[Header("Object Name when Looking at it")] public string ObjectName = "Object";

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
