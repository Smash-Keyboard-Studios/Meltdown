using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLockManager : MonoBehaviour
{
	public static MouseLockManager Instance;

	public bool MouseVisable = false;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	void Update()
	{
		if (MouseVisable)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public void ToggleMouse()
	{
		MouseVisable = !MouseVisable;
	}
}
