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

	public string GetDisplayTextToDisplay()
	{
		if (displayTexts.Count <= 0)
		{
			return "";
		}
		else if (displayTexts.Count == 1)
		{
			return displayTexts[0].Text;
		}

		DisplayText dp = new DisplayText("", -1, -1, -1);

		foreach (DisplayText displayText in displayTexts)
		{
			if (displayText.PriorityLevel > dp.PriorityLevel)
			{
				dp = displayText;
			}
		}

		return dp.Text;
	}

	public bool AddDisplayTextToList(DisplayText displayText)
	{
		if (displayTexts.Contains(displayText)) return false;

		displayTexts.Add(displayText);
		return true;
	}



	public int CreateDisplayText(string text, int priorityLevel = 0, float duration = 5f)
	{
		DisplayText displayText = new DisplayText(text, GetID(), priorityLevel, duration);

		bool b = AddDisplayTextToList(displayText);

		if (b)
		{
			return displayText.ID;
		}
		else
		{
			return -1;
		}
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
		int highestNum = 0;

		foreach (DisplayText displayText in displayTexts)
		{
			if (highestNum < displayText.ID) highestNum = displayText.ID + 1;
		}

		return highestNum;
	}
}
