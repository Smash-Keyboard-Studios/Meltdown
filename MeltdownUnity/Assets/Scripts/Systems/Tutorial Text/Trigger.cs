using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
	[TextArea]
	public string Text;

	public int PriorityLevel = 0;

	public float Duration = 5f;

	private Display.DisplayText displayText;

	// Start is called before the first frame update
	void Start()
	{
		displayText = Display.Current.CreateDisplayTextForRef(Text, PriorityLevel, Duration);
	}

	public void DisplayText()
	{
		Display.Current.AddDisplayTextToList(displayText);
	}
}
