using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class NoteUtil
{
	public enum NoteType
	{
		Page = 0,
		Folder = 1,
		Postitnote = 2,
		Mail = 3
	}
}

[CreateAssetMenu(menuName = "MeltdownUnity/Notes/Note")]
public class NoteObject : ScriptableObject
{

	public NoteUtil.NoteType noteType = NoteUtil.NoteType.Page;

	public string Title = "<size=25><b>Title</b></size>";

	[TextArea(10, 99999)]
	public string Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed consectetur arcu quis lacus tincidunt, at interdum enim euismod. In pellentesque vestibulum nibh, ut tempus ante eleifend non. Vivamus ac orci sapien. Donec nec erat hendrerit, sodales massa vulputate, porttitor nulla. Donec congue aliquet tempor.";
}