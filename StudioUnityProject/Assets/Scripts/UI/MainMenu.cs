using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{

    public GameObject[] UIElements;

    public Slider SensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        OpenUI(0);

        if (LevelLoading.Instance == null) SceneManager.LoadScene(0);


        MouseLockManager.Instance.MouseVisable = true;

        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }

    // sensitivity testing purposes
    void Update()
    {

        //print("sensitivity is " + SensitivitySlider.value);

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

    public void Test()
    {
        int sens = PlayerPrefs.GetInt("SensitivitySlider", -2);

        if (sens == -2)
        {
            sens = 3;
            PlayerPrefs.SetInt("SensitivitySlider", sens);
            PlayerPrefs.Save();
        }



        if (sens > 5) { sens = 5; }
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
