using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static InputActions;

public class InputManager : MonoBehaviour
{
	// the singleton.
	public static InputManager Instance;

	public struct KeyData
	{
		public KeyCode KeyCode;
		public KeyType KeyType;
		public String DisplayText;
		public GameObject UIElement;

		public KeyData(KeyCode keyBind, KeyType type, String displayText, GameObject uiElement)
		{
			this.KeyCode = keyBind;
			this.KeyType = type;
			this.DisplayText = displayText;
			this.UIElement = uiElement;
		}
	}

	public Dictionary<KeyType, KeyData> keyValuePairs = new();


	void Awake()
	{
		// sets the singalton.
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public KeyCode GetKey(KeyType type)
	{
		KeyData data = new();

		keyValuePairs.TryGetValue(type, out data);

		return data.KeyCode;
	}


	public void ChangeKeyBind(KeyCode newKey, KeyType keyType)
	{
		KeyData data = new();

		data = keyValuePairs[keyType];

		data.KeyCode = newKey;

		// should move to BetterInputUI.
		data.UIElement.GetComponent<InputKey>().Text.text = $"{data.KeyType} [{data.KeyCode}]";

		keyValuePairs[keyType] = data;
	}
}
