using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
	public static PlayerFinder Current;

	void Awake()
	{
		if (Current != null && Current != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Current = this;
		}
	}

	public GameObject GetPlayerGO()
	{
		return gameObject;
	}

	public Transform GetPlayerTransform()
	{
		return transform;
	}
}
