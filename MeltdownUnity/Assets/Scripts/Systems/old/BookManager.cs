using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
	Animator animator;

	bool _isOpen = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		animator.SetBool("IsBookOpen", _isOpen);
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
