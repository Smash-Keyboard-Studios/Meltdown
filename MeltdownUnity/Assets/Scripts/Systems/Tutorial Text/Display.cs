using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{
	public static Display Current;

	public TMP_Text display;

	public List<DisplayText> displayTexts = new List<DisplayText>();

	public struct DisplayText
	{
		public int ID;
		public int PriorityLevel;
		public float Duration;
		public string Text;

		public DisplayText(string textToDisplay, int id, int priorityLevel, float duration)
		{
			Text = textToDisplay;
			ID = id;
			PriorityLevel = priorityLevel;
			Duration = duration;
		}
	}

	void Awake()
	{
		if (Current != null && Current != this)
		{
			Destroy(this);
		}
		else
		{
			Current = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public bool AddDisplayTextToList(DisplayText displayText)
	{
		if (displayTexts.Contains(displayText)) return false;

		displayTexts.Add(displayText);
		return true;
	}



	public int CreateDisplayText(string text, int id, int priorityLevel, float duration)
	{

	}

	public bool ContainsDisplayText(int id)
	{
		foreach (DisplayText displayText in displayTexts)
		{
			if (displayText.ID == id)
			{
				return true;
			}
		}

		return false;
	}

	public DisplayText? GetDisplayText(int id)
	{
		foreach (DisplayText displayText in displayTexts)
		{
			if (displayText.ID == id)
			{
				return displayText;
			}
		}

		return null;
	}

	public int GetID()
	{
		int highestNum = displayTexts.Count;

		foreach (DisplayText displayText in displayTexts)
		{
			if (highestNum < displayText.ID) highestNum = displayText.ID + 1;
		}

		return highestNum;
	}
}
