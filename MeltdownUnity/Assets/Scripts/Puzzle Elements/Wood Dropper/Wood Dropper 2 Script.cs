using Codice.Client.Common.GameUI;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class WoodDropper2Script : MonoBehaviour
{
    //Warning! This script is evil and messy, its very terribly made but at least it works!


    //Gameobjects manipulated by script
    public GameObject platformsPrefab;
    public GameObject droppableObjectsPrefab;
    public GameObject woodenBarrel;
    public GameObject suspensionRope;
    private GameObject platforms;
    private GameObject droppableObjects;

    //Components of gameobjects manipulated by script
    private Rigidbody woodenBarrelrb;

    private Vector3 endPosition = new Vector3(-35.77f, 3.49f, -46.72f);
    private float elapsedTime;
    private float protractingDuration = 3;
    private float percentageComplete;

    //Monitors whether the player has met the condition for the platforms to be protracted
    private bool protractPlatformConditionMet = false;

    //Numbers of platforms the player can destroy per attempt
    public int maxDestroyablePlats;

    //Number of platforms contained in the puzzle
    public int noOfWoodPlats = 18;

    //Checks whether the puzzle is currently being reset 
    public bool resettingPuzzle;

    //Checks whether the puzzles platforms have correctly been set after a reset
    public bool puzzleCorrectlySet;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        woodenBarrelrb = woodenBarrel.GetComponent<Rigidbody>();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (!resettingPuzzle)
        {
            CheckForMaxPlatformsDestroyed();
        }
        else
        {
            if (CheckForPuzzleResetFinished())
            {
               ResetValues(); //Resets values used for resetting puzzle if complete
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void FixedUpdate()
    {
        //Checks if animation for protacting platforms i
        if (protractPlatformConditionMet)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / protractingDuration;
            platforms.transform.position = Vector3.Lerp(platforms.transform.position, endPosition, percentageComplete);
        }


        //Assigns correct hierarchy if movement is complete
        if (platforms != null)
        {
            if (platforms.transform.position == endPosition)
            {
                //Reassigns the correct hierarchy using platforms contained as children of platforms prefab
                for (int i = 0; i < platforms.transform.childCount; i++)
                {
                    platforms.transform.GetChild(i).SetParent(transform);
                }
                //Reassigns the correct hierarchy using objects contained as children of droppableObjects prefab
                for (int i = 0; i < droppableObjects.transform.childCount; i++)
                {
                    droppableObjects.transform.GetChild(i).SetParent(transform);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ReleaseBarrel()
    {
        //Destroys the rope and releases the barrel from suspension
        suspensionRope.SetActive(false);
        woodenBarrelrb.isKinematic = false;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ResetPuzzle()
    {
        //Resets the relevant elements of the puzzle
        if (!resettingPuzzle)
        {
            resettingPuzzle = true;
            puzzleCorrectlySet = false;
            DestroyPlatformsAndDroppableObjects();
            StartCoroutine(RespawnPlatforms());
            StartCoroutine(RespawnDroppableObjects());
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void DestroyPlatformsAndDroppableObjects()
    {
        //Checks all the children for platforms and droppable objects
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Platform") || transform.GetChild(i).name.Contains("Object"))
            {
                //Destroys all platforms and objects
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator RespawnPlatforms()
    {
        yield return new WaitForSeconds(0.1f);
        platforms = GameObject.Instantiate(platformsPrefab, new Vector3(-36.273f, 3.49f, -46.054f), Quaternion.Euler(0, -37.5f, 0));
        protractPlatformConditionMet = true;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    IEnumerator RespawnDroppableObjects()
    {
        yield return new WaitForSeconds(1f);
        droppableObjects = GameObject.Instantiate(droppableObjectsPrefab, new Vector3(-42.327f, 0.65f, -48.91f), Quaternion.Euler(0, 0, 0));

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void CheckForMaxPlatformsDestroyed()
    {
        int counter = 0;

        //Checks all the children for number of platforms still alive 
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Wood Platform"))
            {
                counter++;
            }
        }

        //Checks if the puzzle has had 18 platforms at a given point and is therefore correctly set
        if (counter == 18)
        {
            puzzleCorrectlySet = true;
        }

        if (puzzleCorrectlySet)
        {
            //Checks if the player has destroyed more platforms than the max allowed
            if ((counter < (noOfWoodPlats - maxDestroyablePlats)))
            {
                //Resets the puzzle to allow for further attempts
                ResetPuzzle();
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void ResetValues()
    {
        //Resets values used for handling resetting the puzzle
        percentageComplete = 0;
        elapsedTime = 0;
        protractPlatformConditionMet = false;

        Destroy(platforms);
        platforms = null;

        Destroy(droppableObjects);
        droppableObjects = null;

        resettingPuzzle = false;

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private bool CheckForPuzzleResetFinished()
    {
        int objectCounter = 0;
        int platformCounter = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Wood Platform"))
            {
                platformCounter++;
            }
            else if (transform.GetChild(i).name.Contains("Object"))
            {
                objectCounter++;
            }
        }

        if ((platformCounter == 18) && (objectCounter == 8))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}