using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class NotCondition : MonoBehaviour, IPuzzle
{

	public IPuzzle A;

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
		if (A == null)
		{
			Debug.LogError("Cannot work logic with missing referance!");
			return;
		}

		CurrentState = !A.Active;
	}
}
