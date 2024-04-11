using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
	public UnityEvent OnButtonToggle;
	public UnityEvent OnButtonPress;
	public UnityEvent OnButtonRelease;
	
	private bool _isPressed = false;


	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.GetComponent<Rigidbody>() != null || collision.gameObject.GetComponent<PlayerMovementController>() != null)
		{
			Press();
        }
	}

	void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.GetComponent<Rigidbody>() != null || collision.gameObject.GetComponent<PlayerMovementController>() != null)
		{
			Press();
        }
	}

	private void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.GetComponent<Rigidbody>() != null || collision.gameObject.GetComponent<PlayerMovementController>() != null)
		{
			Unpress();
        }
	}

    public void Press()
	{
		if (!_isPressed)
		{
			OnButtonPress.Invoke();
            OnButtonToggle.Invoke();
            _isPressed = true;

		}
	}

	public void Unpress()
	{
		if (_isPressed)
		{
			OnButtonRelease.Invoke();
            OnButtonToggle.Invoke();
            _isPressed = false;
		}
	}
}
