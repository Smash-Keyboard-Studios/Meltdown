using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{
	public static Display Current;

	public TMP_Text display;

	public List<DisplayText> displayTexts = new List<DisplayText>();

	[Serializable]
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

	private bool displayingText = false;

	private float _localTime = 0;
	private DisplayText _currentDisplay;

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

		if (GetDisplayTextToDisplay() != null)
		{
			_currentDisplay = GetDisplayTextToDisplay().Value;
			displayingText = true;
		}
		else
		{
			displayingText = false;
		}

		if (displayingText)
		{
			display.gameObject.SetActive(true);

			_localTime += Time.deltaTime;

			if (_localTime > _currentDisplay.Duration)
			{
				displayTexts.Clear();
				// RemoveDisplayText(_currentDisplay);
				_localTime = 0;
			}
			else
			{
				display.text = _currentDisplay.Text;
			}

		}
		else
		{
			display.text = "";
			display.gameObject.SetActive(false);
		}

	}

	public DisplayText? GetDisplayTextToDisplay()
	{
		if (displayTexts.Count <= 0)
		{
			return null;
		}
		else if (displayTexts.Count == 1)
		{
			return displayTexts[0];
		}

		DisplayText dp = new DisplayText("", -1, -1, -1);

		foreach (DisplayText displayText in displayTexts)
		{
			if (displayText.PriorityLevel > dp.PriorityLevel)
			{
				dp = displayText;
			}
		}

		return dp;
	}

	public bool AddDisplayTextToList(DisplayText displayText)
	{
		if (displayTexts.Contains(displayText)) return false;

		if (displayText.ID < 0) displayText.ID = GetID();

		displayTexts.Add(displayText);
		return true;
	}

	public bool RemoveDisplayText(DisplayText displayText)
	{
		if (displayTexts.Contains(displayText))
		{
			displayTexts.Remove(displayText);
			return true;
		}

		return false;
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

	public DisplayText CreateDisplayTextForRef(string text, int priorityLevel = 0, float duration = 5f)
	{
		DisplayText displayText = new DisplayText(text, -1, priorityLevel, duration);
		return displayText;
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
