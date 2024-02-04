using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InputActions;

public class InputKey : MonoBehaviour
{

	// This is the current information for the key element on the UI.
	// The keyType is to identify when key this will edit.
	// The Button is so I can link the on click event so it will work.
	// This object will be instantiated and will need to be coded to refrences externally to this script.
	public KeyType KeyType;
	public Button KeyButton;

	// This is for the display.
	public TMP_Text Text;

	// This is for event handling.
	void OnEnable()
	{
		if (KeyButton != null) KeyButton.onClick.AddListener(ChangeKey);
	}

	void OnDisable()
	{
		if (KeyButton != null) KeyButton.onClick.RemoveListener(ChangeKey);
	}

	/// <summary>
	/// This is for when the player click remap key.
	/// This calls a funtion form InputManager which is a singleton.
	/// </summary>
	void ChangeKey()
	{
		// TODO make this a get comp.
		BetterInputUI.Instance.ChangeThisKey(KeyType);
	}
}
