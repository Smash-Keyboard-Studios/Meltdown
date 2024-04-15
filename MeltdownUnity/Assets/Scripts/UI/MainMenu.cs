using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;


public class MainMenu : MonoBehaviour
{
	[Header("Volume Setting")]
	[SerializeField] private TMP_Text volumeTextValue = null;
	// [SerializeField] private Slider volumeSlider = null;

	[Header("Gameplay Settings")]
	[SerializeField] private TMP_Text ControllerSenTextValue = null;
	//[SerializeField] private Slider ControllerSenSlider = null;
	public float mainControllerSen = 1f;

	[Header("Toggle Settings")]
	[SerializeField] private Toggle crouchToggle = null;


	[Header("Comfirmation")]
	[SerializeField] private GameObject comfirmationPrompt = null;

	void Start()
	{
		if (MouseLockManager.Instance != null) MouseLockManager.Instance.MouseVisable = true;

		if (SaveManager.current != null)
		{
			SaveManager.current.ForceLoad();
		}
	}

	public void StartNewGame()
	{
		SaveData.Current = new SaveData();
		if (SaveManager.current != null) SaveManager.current.ForceSave();
		LoadGameDialogYes();

	}

	//This is where the override code will be referenced/placed 
	public void LoadGameDialogYes()
	{
		SaveManager.current.ForceLoad();
		if (SaveData.Current.CurrentLevel > 1)
			LevelLoading.Instance.LoadScene(SaveData.Current.CurrentLevel);
		else
			LevelLoading.Instance.LoadScene(2);
		SaveManager.current.ForceSave();
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

	public void DeleteSaveData()
	{
		SaveData.Current = new SaveData();
		if (SaveManager.current != null) SaveManager.current.ForceSave();
	}

	public void SaveSettings()
	{
		if (SaveData.Current == null) return;
		SaveData.Current.MaxVolume = volumeSlider.value;
		SaveData.Current.Sensitivity = ControllerSenSlider.value;
		SaveData.Current.ToggleCrouch = crouchToggle.isOn;
        if (SaveManager.current != null) SaveManager.current.ForceSave();
    }

	public void LoadSettings()
	{
		if (SaveManager.current != null) SaveManager.current.ForceLoad();
        if (SaveData.Current == null) return;
		volumeSlider.value = SaveData.Current.MaxVolume;
        ControllerSenSlider.value = SaveData.Current.Sensitivity;
		crouchToggle.isOn = SaveData.Current.ToggleCrouch;
    }

	public IEnumerator ConfirmationBox()
	{
		//Displays a confirmation window and starts a timer. Once that timer ends the window will vanish.
		comfirmationPrompt.SetActive(true);
		yield return new WaitForSeconds(2);
		comfirmationPrompt.SetActive(false);
	}


}
