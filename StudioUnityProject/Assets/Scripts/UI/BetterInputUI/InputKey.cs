using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputKey : MonoBehaviour
{

	// public unsafe KeyCode* key = null;
	public InputManager.KeyType keyType;
	public TMP_Text text;
	public Button KeyButton;

	private delegate void OnChangeKey();
	OnChangeKey onChangeKey;

	void OnEnable()
	{
		if (KeyButton != null) KeyButton.onClick.AddListener(ChangeKey);
	}

	void OnDisable()
	{
		if (KeyButton != null) KeyButton.onClick.RemoveListener(ChangeKey);
	}



	void ChangeKey()
	{
		InputManager.Instance.ChangeThisKey(keyType);
	}
}
