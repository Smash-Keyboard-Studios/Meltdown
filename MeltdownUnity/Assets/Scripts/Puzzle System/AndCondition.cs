using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndCondition : MonoBehaviour, IPuzzle
{
	public IPuzzle A;
	public IPuzzle B;

	private bool CurrentState = false;
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

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (A == null || B == null)
		{
			Debug.LogError("Cannot work logic with missing referance!");
			return;
		}

		if (A.Active && B.Active)
		{
			CurrentState = true;
		}
		else
		{
			CurrentState = false;
		}
	}
}
