using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel9 : MonoBehaviour, IInteractable
{
    public string ObjectName = "Drill";

    string IInteractable.ObjectName { get => ObjectName; set => ObjectName = value; }

    private void Start()
    {
        gameObject.tag = "InteractableObject";
    }

    public void Interact()
    {
        const int level9Index = 10;

        if (LevelLoading.Instance == null)
        {
            SceneManager.LoadScene(level9Index);
        }
        else if (!LevelLoading.Instance.loading)
        {
            LevelLoading.Instance.LoadScene(level9Index);
        }
    }
}
