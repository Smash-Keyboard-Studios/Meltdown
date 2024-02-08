using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenuActivator : MonoBehaviour
{
	public Button quit;
	public Button resume;
	public Button options;
	public bool EscapeMenuOpen;


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			EscapeMenuOpen = true;
			if (EscapeMenuOpen == false && Input.GetKeyDown(KeyCode.Escape))
			{
				quit.gameObject.SetActive(true);
				resume.gameObject.SetActive(true);
				options.gameObject.SetActive(true);
			}
			else
			{
				EscapeMenuOpen = false;
				quit.gameObject.SetActive(false);
				resume.gameObject.SetActive(true);
				options.gameObject.SetActive(true);
			}


		}


	}

	public void Quit()
	{
		//application.Quit();
		Debug.Log("quit");
	}
	public void Resume()
	{
		EscapeMenuOpen = false;
		Debug.Log("resume application");
	}
	public void Options()
	{
		EscapeMenuOpen = false;
		//SettingsMenuOpen = true;
		Debug.Log("settings menu open");
	}
}
