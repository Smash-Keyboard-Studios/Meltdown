using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loads the save manager prefab incase the save manager does not esxist.
/// </summary>
[DefaultExecutionOrder(-10)]
public class SMBackupBoot : MonoBehaviour
{
	void Awake()
	{
		if (SaveManager.current == null)
		{
			GameObject go = Instantiate(Resources.Load("SaveManager", typeof(GameObject)) as GameObject, Vector3.zero, Quaternion.identity);//, transform.Find("Slot" + indexNumber));
			go.name = "SaveManager BU";
			Debug.LogWarning("WARNING!\nSave Manager was not detected, automatically instaciated the object");
		}
	}

	public static SaveManager CheckAndCreateSaveManager()
	{
		if (SaveManager.current == null)
		{
			Debug.LogWarning("WARNING!\nSave Manager was not detected, automatically instaciated the object");
			Instantiate(Resources.Load("SaveManager", typeof(GameObject)) as GameObject, Vector3.zero, Quaternion.identity);//, transform.Find("Slot" + indexNumber));
			return SaveManager.current;
		}
		else
		{
			return SaveManager.current;
		}
	}
}
