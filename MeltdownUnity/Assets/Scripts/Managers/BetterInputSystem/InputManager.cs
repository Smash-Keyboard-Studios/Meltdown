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
/// To use this, use the standard Input.[key action]](InputManager.GetKey(InputActions.KeyType.[Key]))
/// 
/// This is very long, I will work on making it shorter.
/// </summary>
public class InputManager : MonoBehaviour
{

	public static bool Existing = false;

	/// <summary>
	/// This is the data for the key.
	/// THe KeyData is used to save a keybind / load the keybind.
	/// </summary>
	public struct KeyData
	{
		public KeyCode KeyCode;
		public KeyAction KeyAction;
		public String DisplayText;
		public GameObject UIElement;

		public KeyData(KeyCode keyBind, KeyAction type, String displayText, GameObject uiElement)
		{
			this.KeyCode = keyBind;
			this.KeyAction = type;
			this.DisplayText = displayText;
			this.UIElement = uiElement;
		}
	}

	// This is where all the key actions with the key codes.
	public static Dictionary<KeyAction, KeyData> keyValuePairs = new();

	void Awake()
	{
		if (!Existing)
		{
			keyValuePairs.Clear();
			Existing = true;
		}
	}

	void Start()
	{
		foreach (KeyAction action in Enum.GetValues(typeof(KeyAction)))
		{
			try
			{
				InputManager.KeyData newKeyData = new();

				newKeyData.KeyAction = action;

				// TODO load from save
				if (PlayerPrefs.HasKey(action.ToString()))
				{
					newKeyData.KeyCode = (KeyCode)PlayerPrefs.GetInt(action.ToString());
				}
				else
				{
					newKeyData.KeyCode = GetDefultValues(newKeyData.KeyAction);
					PlayerPrefs.SetInt(action.ToString(), (int)newKeyData.KeyCode);
				}

				newKeyData.DisplayText = $"{newKeyData.KeyAction} [{newKeyData.KeyCode}]";

				// Put the UI elements here
				newKeyData.UIElement = null;

				InputManager.keyValuePairs.Add(newKeyData.KeyAction, newKeyData);

				// keyValuePairs.Add()
			}
			catch
			{
				print("Oopsies");
			}
		}


	}


	public static void ResetKeyBinds()
	{
		foreach (KeyAction action in Enum.GetValues(typeof(KeyAction)))
		{
			ChangeKeyBind(GetDefultValues(action), action);
		}
	}

	/// <summary>
	/// This is how you will get the input instead of Keycode.key as
	/// this will allow the player to change their keybind.
	/// </summary>
	/// <param name="keyAction">the key you want the data for</param>
	/// <returns>data about the key</returns>
	public static KeyCode GetKey(KeyAction keyAction)
	{
		KeyData data = new();

		keyValuePairs.TryGetValue(keyAction, out data);

		return data.KeyCode;
	}

	/// <summary>
	/// Use this method to redind the action with the new key.
	/// </summary>
	/// <param name="newKey">The new key for the action</param>
	/// <param name="keyAction">The key action to update</param>
	public static void ChangeKeyBind(KeyCode newKey, KeyAction keyAction)
	{
		KeyData data = new();

		data = keyValuePairs[keyAction];

		data.KeyCode = newKey;

		// should move to BetterInputUI. - nah.
		if (data.UIElement != null)
			data.UIElement.GetComponent<InputKey>().Text.text = $"{data.KeyAction} [{data.KeyCode}]";

		keyValuePairs[keyAction] = data;


		PlayerPrefs.SetInt(data.KeyAction.ToString(), (int)data.KeyCode);
	}

	public static void SetUIElement(GameObject UIElement, KeyAction keyAction)
	{
		KeyData data = new();

		data = keyValuePairs[keyAction];

		data.UIElement = UIElement;

		keyValuePairs[keyAction] = data;
	}
}
