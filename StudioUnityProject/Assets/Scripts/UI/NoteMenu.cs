using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoteMenu : MonoBehaviour
{
    GameObject playerCharacter;
    GameObject playerUI;
    GameObject CurrentNote;
    GameObject NoteUIBackground;

    public bool IsNoteActive = false;
        
    public void Start()
    {

        playerCharacter = GameObject.FindWithTag("Player");
        playerUI = GameObject.Find("PlayerUI"); // Grabs both the player object and the player UI
        NoteUIBackground = playerUI.transform.Find("NoteBackground").gameObject; // Grabs the NoteBackground to enable it

    }

    public void Update()
    {
        PauseMenu.Overiding = false;
        PauseMenu.Paused = true; 
        if(Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.UI)))
        {

            // Audio should be played here to indicate the closing of a note
            PauseMenu.Overiding = true;
            PauseMenu.Paused = false; // This prevents the pause menu from opening up when trying to close a note

            NoteUIBackground.SetActive(false); // Deactivates note background
            Destroy(CurrentNote); // Destroys the note to prevent duplicates
            IsNoteActive = false;
            playerCharacter.SetActive(true); // Reactivates the player
        }
    }

    public void SetNoteToGUI(GameObject Note) 
    {
        // Audio should be played here to indicate the opening of a note 
        NoteUIBackground.SetActive(true); // Activate note background
        CurrentNote = Instantiate(Note); // Retrieves the note prefab and instantiates it
        CurrentNote.transform.SetParent(playerUI.transform);
        CurrentNote.GetComponent<RectTransform>().localPosition = Vector3.zero; // Puts the note into the player UI and ensures its position is directly in the middle of the screen
        playerCharacter.SetActive(false); // Prevents player from moving
        IsNoteActive = true;
    }

}
