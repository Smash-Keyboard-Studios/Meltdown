using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;


public class MainMenu : MonoBehaviour
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
    [SerializeField]  private GameObject comfirmationPrompt = null;

    public void NewGameDialogYes()
    {
        //Loads Level 1 if player clicks yes
        SceneManager.LoadScene(2);
    }

    //This is where the override code will be referenced/placed 
    public void LoadGameDialogYes()
    {
        
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
