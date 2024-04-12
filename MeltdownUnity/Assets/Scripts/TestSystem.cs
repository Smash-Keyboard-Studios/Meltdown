using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystem : MonoBehaviour
{
	List<PuzzleCompThing> objects = new List<PuzzleCompThing>();

	bool working = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		int count = 0;

		foreach (PuzzleCompThing thing in objects)
		{
			if (thing.active == true)
			{
				count++;
			}
		}

		if (objects.Count == count)
		{
			working = true;
		}
		else
		{
			working = false;
		}
	}
}

[Serializable]
public class PuzzleCompThing : MonoBehaviour
{
	public bool active = false;

	public void SetBool(bool b)
	{
		active = b;
	}

	public void Toggle()
	{
		active = !active;
	}
}
