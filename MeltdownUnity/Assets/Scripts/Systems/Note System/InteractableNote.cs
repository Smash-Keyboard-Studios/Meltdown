using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNote : MonoBehaviour, IInteractable
{
	[Header("BROKEN")]
	public NoteObject note;

	[Header("name")]
	public string ObjectName = "Note";

	string IInteractable.ObjectName { get => ObjectName; set => ObjectName = value; }

	[Header("USE THIS")]
	public NoteUtil.NoteType noteType = NoteUtil.NoteType.Page;

	public string Title = "<size=25><b>Title</b></size>";

	[TextArea(10, 99999)]
	public string Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed consectetur arcu quis lacus tincidunt, at interdum enim euismod. In pellentesque vestibulum nibh, ut tempus ante eleifend non. Vivamus ac orci sapien. Donec nec erat hendrerit, sodales massa vulputate, porttitor nulla. Donec congue aliquet tempor.";


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

		
		NoteMenu.Current.OpenNote(noteType, Title, Content);
	}
}
