using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NoteMenu : MonoBehaviour
{
    GameObject playerCharacter;
    GameObject playerHolder;
    GameObject playerUI;
    GameObject CurrentNote;
    PauseMenu pausemenu;

    public bool IsNoteActive = false;
        
    public void Start()
    {

        playerCharacter = GameObject.FindWithTag("Player");
        playerHolder = playerCharacter.transform.parent.gameObject;
        playerUI = GameObject.Find("PlayerUI");

    }

    public void Update()
    {
        PauseMenu.Overiding = false;
        PauseMenu.Paused = true;
        if (Input.GetKeyDown(KeyCode.Escape) && IsNoteActive)
        {
            Cursor.visible = false;
            PauseMenu.Overiding = true;
            PauseMenu.Paused = false;
     
            Destroy(CurrentNote);
            IsNoteActive = false;
            playerCharacter.SetActive(true);
        }
    }

    public void SetNoteToGUI(GameObject Note)
    {
        CurrentNote = Instantiate(Note);
            CurrentNote.transform.SetParent(playerUI.transform);
            CurrentNote.GetComponent<RectTransform>().localPosition = Vector3.zero;
            Debug.Log("Patrolling the mojave desert almost makes you wish for a nuclear winter");
            playerCharacter.SetActive(false);
            Debug.Log("War. War never changes.");
        IsNoteActive = true;
    }

}
