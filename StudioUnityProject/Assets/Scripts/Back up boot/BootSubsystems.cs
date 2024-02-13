using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootSubsystems : MonoBehaviour
{
	void Start()
	{
		if (InputManager.Exsiting == false && transform.tag == "BackUpBoot")
		{
			gameObject.AddComponent<InputManager>();
			gameObject.AddComponent<LevelLoading>();

			transform.name = "BackupBoot";

			Debug.LogError("Failed to find input manager");
		}
		else if (InputManager.Exsiting == false && transform.tag != "BackUpBoot")
		{
			Instantiate(Resources.Load("Backup boot/BackupBoot", typeof(GameObject)));
		}
	}
}
