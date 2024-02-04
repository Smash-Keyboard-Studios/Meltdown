using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static InputActions;

public class BetterInputUI : MonoBehaviour
{
	// TODO make this not a singleton.
	public static BetterInputUI Instance;

	public TMP_InputField Field;

	public RectTransform Parent;

	public GameObject Prefab;

	public GameObject KeyRebindUI;

	public KeyType currentKeyRebind;

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

		// sets up the input system per key.
		foreach (KeyType type in Enum.GetValues(typeof(KeyType)))
		{
			InputManager.KeyData newKeyData = new();

			newKeyData.KeyType = type;
			// TODO load from save
			newKeyData.KeyCode = GetDefultValues(newKeyData.KeyType);

			newKeyData.DisplayText = $"{newKeyData.KeyType} [{newKeyData.KeyCode}]";
			print(newKeyData.DisplayText);

			GameObject UIElement = Instantiate(Prefab, Parent);

			newKeyData.UIElement = UIElement;

			// TODO make into variable not this madness
			UIElement.GetComponent<InputKey>().KeyType = newKeyData.KeyType;
			UIElement.GetComponent<InputKey>().Text.text = newKeyData.DisplayText;

			InputManager.Instance.keyValuePairs.Add(newKeyData.KeyType, newKeyData);

			// keyValuePairs.Add()
		}
	}


	public void ChangeThisKey(KeyType keyType)
	{
		currentKeyRebind = keyType;
		RecordKeyStart();
	}


	public void RecordKeyStart()
	{
		KeyRebindUI.SetActive(true);
		Field.readOnly = false;

		// oof that is very long. Might rework.
		Field.SetTextWithoutNotify(InputManager.Instance.keyValuePairs[currentKeyRebind].KeyCode.ToString());
		Field.ActivateInputField();
	}


	public void UpdateKeyWithNewKeyEntered()
	{
		object result;

		Enum.TryParse(typeof(KeyCode), Field.text, true, out result);

		if (result == null)
		{
			print("space");
			result = KeyCode.Space;
		}

		InputManager.Instance.ChangeKeyBind((KeyCode)result, currentKeyRebind);

		Field.readOnly = true;

		KeyRebindUI.SetActive(false);
	}

}
