using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This creates the missing manages so you can still player the game
/// in the editor.
/// </summary>
public class BootSubsystems : MonoBehaviour
{
	void Awake()
	{
		// if the input manager does not exists, create teh back up prefab with it attached.
		if (InputManager.Existing == false)
		{
			Debug.LogWarning("Failed to find input manager");
			GameObject go = Instantiate(Resources.Load("Backup boot/BackupBoot", typeof(GameObject))) as GameObject;
			DontDestroyOnLoad(go);
		}
	}
}
