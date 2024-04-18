using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteraction : MonoBehaviour, IInteractable
{
	public string ObjectName = "Note";

	string IInteractable.ObjectName { get => ObjectName; set => ObjectName = value; }

	public UnityEvent OnButtonPress;

	[SerializeField] private bool toggleButton = false;
	// [SerializeField] private bool timedButton = false;  // Serialised access fields for  designers to decide what button type

	// [SerializeField] private int waitTimer = 1; // Length of time between debounces
	// [SerializeField] private bool pauseInteraction = false;

	private Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void Interact()
	{
		PressButton();
	}

	public void PressButton()
	{
		OnButtonPress.Invoke();

		animator.SetTrigger("Press");

		//StartCoroutine(PlayAnimation());

	}

	IEnumerator PlayAnimation()
	{
		animator.SetBool("IsPressed", true);
		yield return new WaitForSeconds(0.01f);
		animator.SetBool("IsPressed", false);
	}
}
