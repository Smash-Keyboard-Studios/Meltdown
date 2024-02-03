using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InputManager : MonoBehaviour
{
	public TMP_InputField field;

	public KeyCode ForwardKey;
	public KeyCode BackwardsKey;
	public KeyCode LeftStrafeKey;
	public KeyCode RightStrafeKey;

	public KeyCode SprintKey;
	public KeyCode JumpKey;

	void Start()
	{
		field.text = ForwardKey.ToString();
	}

	void Update()
	{


	}

	public void ChangeKey()
	{
		if (field.text != string.Empty)
		{
			object res;
			Enum.TryParse(typeof(KeyCode), field.text, true, out res);

			ForwardKey = (KeyCode)res;

			print(ForwardKey.ToString());

			field.
		}
	}
}
