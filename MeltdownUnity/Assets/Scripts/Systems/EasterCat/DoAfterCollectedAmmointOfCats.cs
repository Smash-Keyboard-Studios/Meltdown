using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoAfterCollectedAmmointOfCats : MonoBehaviour
{
	[Header("This is for the ammount needed to be collected on the current level")]
	public int MinAmmountNeeded = 1;

	public UnityEvent OnAmmountCollected;

	private bool _ranOnce = false;

	void Update()
	{
		if (CatTracker.Current == null) return;

		if (!_ranOnce && CatTracker.Current.CurrentCollected >= MinAmmountNeeded)
		{
			OnAmmountCollected.Invoke();

			_ranOnce = true;
		}
	}
}
