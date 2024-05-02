using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
	Animator animator;

	[HideInInspector]
	public bool _isOpen = false;

	public static BookManager Current;

	void Awake()
	{
		if (Current != null && Current != this)
		{
			Destroy(this);
		}
		else
		{
			Current = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		animator.SetBool("IsBookOpen", _isOpen);

		if (_isOpen && (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.UI)) || Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.Interact)) || Input.GetKeyDown(KeyCode.F)))
		{
			_isOpen = false;
		}
	}

	public void ToggleBook()
	{
		_isOpen = !_isOpen;
	}

	public void SetBookState(bool b)
	{
		_isOpen = b;
	}
}
