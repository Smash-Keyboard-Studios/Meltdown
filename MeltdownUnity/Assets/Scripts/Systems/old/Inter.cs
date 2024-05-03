using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter : MonoBehaviour, IInteractable
{
	private string Name = "Hat";

	string IInteractable.ObjectName { get => Name; set => Name = value; }

	void IInteractable.Interact()
	{
		if (BookManager.Current != null)
		{
			BookManager.Current._isOpen = true;
		}
	}


	// Start is called before the first frame update
	void Start()
	{
		transform.tag = "InteractableObject";
	}

	// Update is called once per frame
	void Update()
	{

	}
}
