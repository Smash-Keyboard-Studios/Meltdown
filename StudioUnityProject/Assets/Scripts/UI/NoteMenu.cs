using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class NoteMenu : MonoBehaviour
{

    GameObject playerCharacter;
    GameObject playerHolder;
    GameObject playerUI;
    GameObject CurrentNote;

    public bool IsNoteActive;
        
    public void Start()
    {
        playerCharacter = GameObject.FindWithTag("Player");
        playerHolder = playerCharacter.transform.parent.gameObject;
        playerUI = GameObject.Find("PlayerUI");
        
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && IsNoteActive)
        {
            Destroy(CurrentNote);
            playerCharacter.SetActive(true);
        }
    }

    public void SetNoteToGUI(GameObject Note)
    {
        Note = Instantiate(Note);
        CurrentNote = Instantiate(Note);
        if (CurrentNote != null)
        {
            CurrentNote.transform.SetParent(playerUI.transform);
            CurrentNote.GetComponent<RectTransform>().localPosition = Vector3.zero;
            Debug.Log("Patrolling the mojave desert almost makes you wish for a nuclear winter");
            IsNoteActive = true;
            playerCharacter.SetActive(false);
            Debug.Log("War. War never changes.");
        }
    }

}
