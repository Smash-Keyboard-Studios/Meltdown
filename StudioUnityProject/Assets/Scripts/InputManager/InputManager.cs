using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InputManager : MonoBehaviour
{
	public static InputManager Instance;

	public TMP_InputField field;

	public RectTransform parent;

	public GameObject Prefab;

	public GameObject KeyRebindUI;

	// defualt values - will move
	[Header("TMP - Defualt keys")]
	public KeyCode ForwardKey = KeyCode.W;
	public KeyCode BackwardsKey = KeyCode.S;
	public KeyCode LeftStrafeKey = KeyCode.A;
	public KeyCode RightStrafeKey = KeyCode.D;

	public KeyCode SprintKey = KeyCode.LeftShift;
	public KeyCode JumpKey = KeyCode.Space;
	public KeyCode CrouchKey = KeyCode.C;

	public KeyCode ShootFireKey = KeyCode.Mouse0;
	public KeyCode ShootIceKey = KeyCode.Mouse1;

	public enum KeyType
	{
		Forward,
		Backward,
		Left,
		Right,
		Sprint,
		Jump,
		Crouch,
		ShootFire,
		ShootIce,
		Spare
	}

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

		foreach (KeyType type in Enum.GetValues(typeof(KeyType)))
		{
			KeyData newKeyData = new();

			newKeyData.KeyType = type;
			// load from save
			newKeyData.KeyCode = GetDefultValues(newKeyData.KeyType);

			newKeyData.DisplayText = $"{newKeyData.KeyType} [{newKeyData.KeyCode}]";
			print(newKeyData.DisplayText);

			GameObject UIElement = Instantiate(Prefab, parent);

			newKeyData.UIElement = UIElement;

			// make into variable not this madness
			UIElement.GetComponent<InputKey>().keyType = newKeyData.KeyType;
			UIElement.GetComponent<InputKey>().text.text = newKeyData.DisplayText;

			keyValuePairs.Add(newKeyData.KeyType, newKeyData);

			// keyValuePairs.Add()
		}


	}

	private KeyCode GetDefultValues(KeyType KeyType)
	{
		KeyCode returnValue = KeyCode.None;

		switch (KeyType)
		{
			case KeyType.Forward:
				returnValue = ForwardKey;
				break;
			case KeyType.Backward:
				returnValue = BackwardsKey;
				break;
			case KeyType.Left:
				returnValue = LeftStrafeKey;
				break;
			case KeyType.Right:
				returnValue = RightStrafeKey;
				break;
			case KeyType.Sprint:
				returnValue = SprintKey;
				break;
			case KeyType.Jump:
				returnValue = JumpKey;
				break;
			case KeyType.Crouch:
				returnValue = CrouchKey;
				break;
			case KeyType.ShootFire:
				returnValue = ShootFireKey;
				break;
			case KeyType.ShootIce:
				returnValue = ShootIceKey;
				break;
			default:
				returnValue = KeyCode.None;
				break;
		}

		return returnValue;
	}


	// ! Use this one instead.
	// private void SetDefultValues(KeyData keyData)
	// {
	// 	switch (keyData.KeyType)
	// 	{
	// 		case KeyType.Forward:
	// 			keyData.KeyCode = KeyCode.W;
	// 			break;
	// 		case KeyType.Backward:
	// 			keyData.KeyCode = KeyCode.S;
	// 			break;
	// 		case KeyType.Left:
	// 			keyData.KeyCode = KeyCode.A;
	// 			break;
	// 		case KeyType.Right:
	// 			keyData.KeyCode = KeyCode.D;
	// 			break;
	// 		case KeyType.Sprint:
	// 			keyData.KeyCode = KeyCode.LeftShift;
	// 			break;
	// 		case KeyType.Jump:
	// 			keyData.KeyCode = KeyCode.Space;
	// 			break;
	// 		case KeyType.Crouch:
	// 			keyData.KeyCode = KeyCode.C;
	// 			break;
	// 		case KeyType.ShootFire:
	// 			keyData.KeyCode = KeyCode.Mouse0;
	// 			break;
	// 		case KeyType.ShootIce:
	// 			keyData.KeyCode = KeyCode.Mouse1;
	// 			break;
	// 		default:
	// 			keyData.KeyCode = KeyCode.None;
	// 			break;
	// 	}
	// }

	void Update()
	{


	}

	public KeyCode GetKey(KeyType type)
	{
		KeyData data = new();

		keyValuePairs.TryGetValue(type, out data);
		print(data.KeyCode.ToString());

		return data.KeyCode;
	}

	public void ChangeThisKey(KeyType keyType)
	{
		currentKeyRebind = keyType;
		RecordKeyStart();
	}

	public void RecordKeyStart()
	{
		KeyRebindUI.SetActive(true);
		field.readOnly = false;

		KeyData data = new();

		keyValuePairs.TryGetValue(currentKeyRebind, out data);

		field.SetTextWithoutNotify(data.KeyCode.ToString());
		field.ActivateInputField();
	}


	public void ChangeKeyBind()
	{
		if (field.text != string.Empty)
		{
			object res;
			Enum.TryParse(typeof(KeyCode), field.text, true, out res);

			KeyData data = new();

			data = keyValuePairs[currentKeyRebind];

			data.KeyCode = (KeyCode)res;
			print(currentKeyRebind.ToString());
			print(data.KeyCode.ToString());

			data.UIElement.GetComponent<InputKey>().text.text = $"{data.KeyType} [{data.KeyCode}]";

			keyValuePairs[currentKeyRebind] = data;


			field.readOnly = true;

			KeyRebindUI.SetActive(false);
		}
	}

	// public unsafe void ChangeKey(KeyCode* keycodePtr)
	// {
	// 	RecordKeyStart();


	// 	if (field.text != string.Empty)
	// 	{
	// 		object res;
	// 		Enum.TryParse(typeof(KeyCode), field.text, true, out res);

	// 		keycodePtr* = (KeyCode)res;

	// 		print(ForwardKey.ToString());

	// 		field.readOnly = true;
	// 	}
	// }
}
