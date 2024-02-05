using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InputActions;

/// <summary>
/// This class is for the UI elements for each key.
/// It holds what key it is. This is the UI for the player to
/// change idividual keys.
/// </summary>
public class InputKey : MonoBehaviour
{

	// The keyType is to identify when key this will edit.
	[HideInInspector]
	public KeyAction KeyType;

	[Header("UI variables")]

	// The Button is so I can link the on click event so it will work.
	[Tooltip("The button for the player to click to rebind this key")]
	public Button KeyButton;

	// This is for the display.
	[Tooltip("The information box to indicate to the player what action this is and the current bound key")]
	public TMP_Text Text;

	// This is for event handling with the button.
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
	/// This calls a funtion from InputManager which is a singleton.
	/// </summary>
	void ChangeKey()
	{
		// TODO make this a get comp. could set the variable here when instancing this object.
		BetterInputUI.Instance.ChangeThisKey(KeyType);
	}
}
