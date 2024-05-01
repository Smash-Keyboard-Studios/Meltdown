using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static InputActions;
using System.Linq;

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

	void Update()
	{
		if (KeyRebindUI.activeSelf)
		{
			if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				InputManager.ChangeKeyBind(KeyCode.LeftControl, currentKeyToRebind);
				Field.readOnly = true;

				KeyRebindUI.SetActive(false);
			}
			else if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				InputManager.ChangeKeyBind(KeyCode.LeftShift, currentKeyToRebind);
				Field.readOnly = true;

				KeyRebindUI.SetActive(false);
			}
			else if (Input.GetKeyDown(KeyCode.Escape))
			{
				InputManager.ChangeKeyBind(KeyCode.Escape, currentKeyToRebind);
				Field.readOnly = true;

				KeyRebindUI.SetActive(false);
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				InputManager.ChangeKeyBind(KeyCode.Space, currentKeyToRebind);
				Field.readOnly = true;

				KeyRebindUI.SetActive(false);
			}
			else if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				InputManager.ChangeKeyBind(KeyCode.Mouse0, currentKeyToRebind);
				Field.readOnly = true;

				KeyRebindUI.SetActive(false);
			}
			else if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				InputManager.ChangeKeyBind(KeyCode.Mouse1, currentKeyToRebind);
				Field.readOnly = true;

				KeyRebindUI.SetActive(false);
			}
			else if (Input.GetKeyDown(KeyCode.Mouse3))
			{
				InputManager.ChangeKeyBind(KeyCode.Mouse3, currentKeyToRebind);
				Field.readOnly = true;

				KeyRebindUI.SetActive(false);
			}
		}
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
			InputManager.KeyData keyData = InputManager.keyValuePairs[action];

			GameObject UIElement = Instantiate(Prefab, Parent);

			InputManager.SetUIElement(UIElement.gameObject, keyData.KeyAction);

			InputKey inputKey = UIElement.GetComponent<InputKey>();
			inputKey.KeyType = keyData.KeyAction;
			inputKey.Text.text = keyData.DisplayText;


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
			result = KeyCode.None;
		}

		InputManager.ChangeKeyBind((KeyCode)result, currentKeyToRebind);



		Field.readOnly = true;

		KeyRebindUI.SetActive(false);
	}

	public void Abort()
	{
		Field.readOnly = true;

		KeyRebindUI.SetActive(false);
	}

	public void ResetAll()
	{
		InputManager.ResetKeyBinds();
	}

}
