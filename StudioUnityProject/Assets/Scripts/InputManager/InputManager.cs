using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InputManager : MonoBehaviour
{
	public static InputManager Instance;

	public TMP_InputField field;

	public KeyCode ForwardKey = KeyCode.W;
	public KeyCode BackwardsKey = KeyCode.S;
	public KeyCode LeftStrafeKey = KeyCode.A;
	public KeyCode RightStrafeKey = KeyCode.D;

	public KeyCode SprintKey = KeyCode.LeftShift;
	public KeyCode JumpKey = KeyCode.Space;

	enum KeyType
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

	public struct key
	{
		KeyCode keyCode;
		KeyType keyType;
		String text;
	}

	public key asdasd;


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
		field.text = ForwardKey.ToString();
	}

	void Update()
	{


	}


	public void RecordKeyStart()
	{
		field.readOnly = false;
		field.SetTextWithoutNotify(string.Empty);
		field.ActivateInputField();

	}


	public void ChangeKey()
	{
		if (field.text != string.Empty)
		{
			object res;
			Enum.TryParse(typeof(KeyCode), field.text, true, out res);

			ForwardKey = (KeyCode)res;

			print(ForwardKey.ToString());

			field.readOnly = true;
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
