using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
	public GameObject ConsoleWindow;

	public GameObject Content;

	public GameObject TextPrefab;

	public TMP_InputField InputField;

	public Scrollbar VertScrollBar;

	bool consoleOpen = false;


	// Start is called before the first frame update
	void Start()
	{
		ConsoleWindow.SetActive(consoleOpen);
		Cursor.lockState = consoleOpen ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = consoleOpen ? true : false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			consoleOpen = !consoleOpen;
			ConsoleWindow.SetActive(consoleOpen);
			Cursor.lockState = consoleOpen ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = consoleOpen ? true : false;
		}


		if (consoleOpen)
		{

		}
	}

	public void TextToConsole(string text)
	{
		GameObject textObj = Instantiate(TextPrefab, Content.transform, false);

		textObj.GetComponent<TMP_Text>().text = text;

		VertScrollBar.value = 0;
	}

	public void EndEdit()
	{
		string command = InputField.text;

		string[] strings = command.Split(" ");

		if (string.IsNullOrEmpty(strings[0]))
		{
			TextToConsole("<color=red>Unkown command!</color>");
		}

		if (strings[0] != null)
		{
			TextToConsole(strings[0]);
		}


	}
}
