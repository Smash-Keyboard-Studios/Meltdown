using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
	public Animator animator;

	public bool AutoSetAnimator = false;

	// Start is called before the first frame update
	void Start()
	{
		if (AutoSetAnimator) animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void TriggerAnimationString(string TriggerName)
	{
		animator.SetTrigger(TriggerName);
	}

	public void TriggerAnimationID(int TriggerID)
	{
		animator.SetTrigger(TriggerID);
	}
}
