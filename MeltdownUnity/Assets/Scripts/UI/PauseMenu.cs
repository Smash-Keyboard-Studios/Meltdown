using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PauseMenu : MonoBehaviour
{

	[Header("Volume Setting")]
	[SerializeField] private TMP_Text volumeTextValue = null;
	[SerializeField] private Slider volumeSlider = null;

	[Header("Gameplay Settings")]
	[SerializeField] private TMP_Text ControllerSenTextValue = null;
	[SerializeField] private Slider ControllerSenSlider = null;
	public float mainControllerSen = 1f;

	[Header("Toggle Settings")]
	[SerializeField] private Toggle crouchToggle = null;


	[Header("Comfirmation")]
	[SerializeField] private GameObject comfirmationPrompt = null;

	public static bool Paused = false;
	public static bool Overiding = false;
	public GameObject PauseMenuCanvas;

	MouseLookController cam;
	public GameObject player;

	private void Awake()
	{
		cam = player.GetComponent<MouseLookController>();
	}

	void Start()
	{
		Time.timeScale = 1f;
		Play();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !Overiding)
		{
			if (Paused)
			{
				Play();
				//Cursor.lockState = CursorLockMode.Locked;
				//Cursor.visible = false;

			}
			else
			{
				Stop();

			}
		}
	}

	void Stop()
	{
		PauseMenuCanvas.SetActive(true);
		Time.timeScale = 0f;
		Paused = true;
		MouseLockManager.Instance.MouseVisable = true;
		cam.Locked = true;

	}

	void Play()
	{
		MouseLockManager.Instance.MouseVisable = false;
		PauseMenuCanvas.SetActive(false);
		Time.timeScale = 1f;
		Paused = false;
		cam.Locked = false;
	}
	public void Resume()
	{
		Play();
	}

	public void GameExitButton()
	{
		LevelLoading.Instance.LoadScene(1);
	}
	public void ExitButton()
	{
		//Allows the player to quit the application
		Application.Quit();
	}

	public void SetVolume(float volume)
	{
		//Allows the player to change the volume
		AudioListener.volume = volume;
		volumeTextValue.text = volume.ToString("0.0");
	}

	public void VolumeSave()
	{
		//Saves the volume changes
		PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
		StartCoroutine(ConfirmationBox());
	}

	public void SetControllerSen(float sensitivity)
	{
		//Allows the player to changed their mouse sensitivity
		mainControllerSen = sensitivity;
		ControllerSenTextValue.text = sensitivity.ToString("0.0");

	}

	public void GameplaySave()
	{
		//Allows the player to save their gameplay changes
		if (crouchToggle.isOn)
		{
			//Allows crouch to become a toggle button instead of a crouch
			PlayerPrefs.SetInt("masterCrouch", 1);
		}
		else
		{
			//Allows crouch to become a hold down button instead of a crouch
			PlayerPrefs.SetInt("masterCrouch", 0);
		}

		PlayerPrefs.SetFloat("masterSen", mainControllerSen);
	}

	public IEnumerator ConfirmationBox()
	{
		//Displays a confirmation window and starts a timer. Once that timer ends the window will vanish.
		comfirmationPrompt.SetActive(true);
		yield return new WaitForSeconds(2);
		comfirmationPrompt.SetActive(false);
	}
}
