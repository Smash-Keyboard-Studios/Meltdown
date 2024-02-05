using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static InputActions;

/// <summary>
/// This is the better input system, You use this instad of the Keycode provided by unity
/// so the player can change their keybinds.
/// 
/// To use this, use the standard Input.[key action]](InputManager.Instance.GetKey(InputActions.KeyType.[Key]))
/// 
/// This is very long, I will work on making it shorter.
/// </summary>
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

	// TODO statics may be better. shortens the call by 1.
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
