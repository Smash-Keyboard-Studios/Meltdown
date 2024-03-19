using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[Inspectable]
public class PuzzleComp : MonoBehaviour, IPuzzle
{
	public bool CurrentState = false;
	bool IPuzzle.Active
	{
		get
		{
			return CurrentState;
		}
		set
		{
			CurrentState = value;
		}
	}

	public void SetState(bool b)
	{
		CurrentState = b;
	}

	public void ToggleState()
	{
		CurrentState = !CurrentState;
	}
}
