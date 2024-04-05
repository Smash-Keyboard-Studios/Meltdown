using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNote : MonoBehaviour, IInteractable
{
	public NoteObject note;

	// Start is called before the first frame update
	void Start()
	{
		gameObject.tag = "InteractableObject";
	}

	// Update is called once per frame
	void Update()
	{

	}

	void IInteractable.Interact()
	{

		if (note == null)
		{
			Debug.LogError("Cannot open note UI with no note | NullRefernceExeption");
			return;
		}

		NoteMenu.Current.OpenNote(note);
	}
}
