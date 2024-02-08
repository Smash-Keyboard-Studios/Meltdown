using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	bool _paused = false;

	public GameObject PauseMenuObject;

	// Start is called before the first frame update
	void Start()
	{
		PauseMenuObject.SetActive(_paused);
		Time.timeScale = _paused ? 0 : 1;
		if (MouseLockManager.Instance != null)
			MouseLockManager.Instance.MouseVisable = _paused;
		else
		{
			Cursor.lockState = _paused ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = _paused;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_paused = !_paused;
			PauseMenuObject.SetActive(_paused);
			Time.timeScale = _paused ? 0 : 1;
			if (MouseLockManager.Instance != null)
				MouseLockManager.Instance.MouseVisable = _paused;
			else
			{
				Cursor.lockState = _paused ? CursorLockMode.None : CursorLockMode.Locked;
				Cursor.visible = _paused;
			}
		}
	}

	public void MainMenu()
	{
		if (LevelLoading.Instance != null)
			LevelLoading.Instance.LoadScene(1);
		else
			SceneManager.LoadScene(0);

	}
}
