using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ANDComp : MonoBehaviour
{

	public bool A = false;
	public bool B = false;

	[Space]
	public bool Result = false;

	[Space]
	public UnityEvent OnConditionMet;

	[Space]
	public UnityEvent OnConditionNotMet;

	private bool alreadyTriggered = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!alreadyTriggered && A && B)
		{
			alreadyTriggered = true;
			Result = true;
			OnConditionMet.Invoke();
		}
		else if ((!A || !B) && alreadyTriggered)
		{
			OnConditionNotMet.Invoke();
			Result = false;
			alreadyTriggered = false;
		}
	}

	public void DebugMessage()
	{
		print($"Testing A:{A} B:{B} Result:{Result} aleardyTriggered:{alreadyTriggered}");
	}

	// primary
	public void ToggleA()
	{
		A = !A;
	}

	public void SetA(bool b)
	{
		A = B;
	}


	// Other 
	public void ToggleB()
	{
		B = !B;
	}

	public void SetB(bool b)
	{
		B = b;
	}
}
