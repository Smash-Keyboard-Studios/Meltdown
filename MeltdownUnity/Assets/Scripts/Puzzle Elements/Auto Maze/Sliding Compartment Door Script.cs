using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class SlidingCompartmentDoorScript : MonoBehaviour
{
    private Vector3 endPosition;
    private float elapsedTime;
    private float openingDuration = 1;
    private float percentageComplete;

    //Monitors whether the player has met the condition for the door to be opened
    private bool openDoorConditionMet = false;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        //Assigns the end position of the door opening based on its current position
        endPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (openDoorConditionMet)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / openingDuration;
            transform.position = Vector3.Lerp(transform.position, endPosition, percentageComplete);
        }

        //Hides the door if it is opened and out of player view
        if (transform.position == endPosition)
        {
            gameObject.SetActive(false);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OpenDoor()
    {
        openDoorConditionMet = true;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
