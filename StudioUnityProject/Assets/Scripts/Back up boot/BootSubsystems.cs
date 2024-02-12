using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootSubsystems : MonoBehaviour
{
	int sceneID;

	void Start()
	{
		if (LevelLoading.Instance == null)
		{
			DontDestroyOnLoad(this.gameObject);
			sceneID = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(0);
		}
	}

	void Update()
	{
		if (LevelLoading.Instance != null)
		{
			LevelLoading.Instance.overideAll = true;
			LevelLoading.Instance.LoadScene(sceneID);

			LevelLoading.Instance.overideAll = false;
			Destroy(this.gameObject);
		}
	}
}
