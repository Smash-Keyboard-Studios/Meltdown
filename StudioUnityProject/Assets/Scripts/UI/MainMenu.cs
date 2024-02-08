using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public GameObject[] UIElements;

	// Start is called before the first frame update
	void Start()
	{
		OpenUI(0);

		if (LevelLoading.Instance == null) SceneManager.LoadScene(0);


		MouseLockManager.Instance.MouseVisable = true;

		// Cursor.lockState = CursorLockMode.None;
		// Cursor.visible = true;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void OpenUI(int id)
	{
		for (int i = 0; i < UIElements.Length; i++)
		{
			if (i == id)
				UIElements[i].SetActive(true);
			else
				UIElements[i].SetActive(false);
		}
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void LoadScene(int idex)
	{
		LevelLoading.Instance.LoadScene(idex);
	}
}
