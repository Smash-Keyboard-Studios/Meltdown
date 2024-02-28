using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMenu : MonoBehaviour
{

    GameObject playerCharacter;
    GameObject playerHolder;
        
    public void Start()
    {
        playerCharacter = GameObject.FindWithTag("Player");
        playerHolder = playerCharacter.transform.parent.gameObject;
    }

    public void ShowNote(GameObject Note)
    {
        playerCharacter.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        Note = Instantiate(Note);
        Note.transform.SetParent(playerHolder.transform.GetChild(1)); // gets the canvas, if the positioning of the canvas index changes then this will no longer work
        Note.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }
}
