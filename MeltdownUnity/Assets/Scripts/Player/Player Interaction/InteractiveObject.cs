using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour, IInteractable
{
	public UnityEvent OnInteract;

	[Header("Object Name when Looking at it")] public string ObjectName = "Please Change the Object Name in the InteractiveObject script";

	public void Interact()
	{
		OnInteract.Invoke();
	}
}
