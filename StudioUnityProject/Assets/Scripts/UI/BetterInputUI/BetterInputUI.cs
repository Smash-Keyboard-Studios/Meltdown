using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static InputActions;

/// <summary>
/// This is where the UI is managed.
/// It converts and updates information from the UI to the InputManager.
/// </summary>
public class BetterInputUI : MonoBehaviour
{
	// TODO make this not a singleton.
	public static BetterInputUI Instance;

	[Header("UI variables")]

	[Tooltip("The text box / input field for the player to enter the nee key")]
	public TMP_InputField Field;

	[Tooltip("The place where the UI elements will go under")]
	public RectTransform Parent;

	[Tooltip("The UI element with the button and the text")]
	public GameObject Prefab;

	[Tooltip("The UI to enter a new key that will be used to rebind the old key")]
	public GameObject KeyRebindUI;

	[HideInInspector] // dont want the designers to edit this.
	public KeyAction currentKeyToRebind;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	void Start()
	{
		KeyRebindUI.SetActive(false);

		GenerateRebindUI();
	}


	/// <summary>
	/// This will create the UI elemets for each key action.
	/// This will dynamically change automatically when you add more keys.
	/// </summary>
	private void GenerateRebindUI()
	{
		// sets up the input system per key.
		foreach (KeyAction action in Enum.GetValues(typeof(KeyAction)))
		{
			InputManager.KeyData newKeyData = new();

			newKeyData.KeyAction = action;

			// TODO load from save
			newKeyData.KeyCode = GetDefultValues(newKeyData.KeyAction);

			newKeyData.DisplayText = $"{newKeyData.KeyAction} [{newKeyData.KeyCode}]";

			GameObject UIElement = Instantiate(Prefab, Parent);

			newKeyData.UIElement = UIElement;

			InputKey inputKey = UIElement.GetComponent<InputKey>();
			inputKey.KeyType = newKeyData.KeyAction;
			inputKey.Text.text = newKeyData.DisplayText;

			InputManager.keyValuePairs.Add(newKeyData.KeyAction, newKeyData);

			// keyValuePairs.Add()
		}
	}

	/// <summary>
	/// This is called when the button next to the key is pressed.
	/// This is what you call when you wamt to promt to change key.
	/// </summary>
	/// <param name="keyType">The key action you want to update</param>
	public void ChangeThisKey(KeyAction keyType)
	{
		currentKeyToRebind = keyType;
		RecordKeyStart();
	}

	/// <summary>
	/// Enables the input field.
	/// </summary>
	public void RecordKeyStart()
	{
		KeyRebindUI.SetActive(true);
		Field.readOnly = false;

		// oof that is very long. Might rework.
		Field.SetTextWithoutNotify(InputManager.keyValuePairs[currentKeyToRebind].KeyCode.ToString());
		Field.ActivateInputField();
	}

	/// <summary>
	/// This is called when the Input Field is updated with the new character.
	/// </summary>
	public void UpdateKeyWithNewKeyEntered()
	{
		object result;

		Enum.TryParse(typeof(KeyCode), Field.text, true, out result);

		if (result == null)
		{
			print("space");
			result = KeyCode.Space;
		}

		InputManager.ChangeKeyBind((KeyCode)result, currentKeyToRebind);

		Field.readOnly = true;

		KeyRebindUI.SetActive(false);
	}

}
