using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputKey : MonoBehaviour
{

	// public unsafe KeyCode* key = null;
	public KeyCode key;
	public TMP_Text text;
	public Button KeyButton;

	private delegate void OnChangeKey();
	OnChangeKey onChangeKey;



	void OnEnable()
	{
		KeyButton.onClick.AddListener(ChangeKey);
	}

	void OnDisable()
	{
		KeyButton.onClick.RemoveListener(ChangeKey);
	}



	void ChangeKey()
	{

	}
}
