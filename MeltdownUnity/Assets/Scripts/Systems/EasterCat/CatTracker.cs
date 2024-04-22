using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatTracker : MonoBehaviour
{
	public static CatTracker Current;

	[HideInInspector]
	public int CatAmmountOnLevel = 0;

	[HideInInspector]
	public int CurrentCollected = 0;

	private bool _alreadyChangedSaveData = false;

	void Awake()
	{
		if (Current != null && Current != this)
		{
			Destroy(this);
		}
		else
		{
			Current = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (SaveData.Current != null && SaveManager.current != null && !_alreadyChangedSaveData && CurrentCollected >= CatAmmountOnLevel)
		{
			try
			{
				int buildIndex = SceneManager.GetActiveScene().buildIndex;
				SaveData.Current.CollectedOnLevel[buildIndex] = true;
				SaveManager.current.ForceSave();
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex);
			}
		}
	}

	public void CollectCat()
	{
		CurrentCollected++;
	}
}
