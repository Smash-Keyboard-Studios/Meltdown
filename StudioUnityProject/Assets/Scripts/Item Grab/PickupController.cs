using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    public GameObject mainGun;

    Gun gun;
    public GameObject player;

    void Awake()
    {
        //Links the gun script tied to the player object to this script.
        gun = player.GetComponent<Gun>();
    }

    private void Update()
    {
        //Checks all of the time when the player is going to pick something up.
        //The Input is set to a toggle.
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Checks if at any point the player is actually holding an item.
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject);

                    //Disables the mainGun as well as allowing the player to fire their ice and/or fire when the object is picked up.
                    mainGun.SetActive(false);
                    gun.hasFire = false;
                    gun.hasIce = false;
                }
            }
            else
            {
                DropObject();

                //Enables the mainGun as well as allowing the player to fire their ice and/or fire when the object is dropped'
                mainGun.SetActive(true);
                gun.hasFire = true;
                gun.hasIce = true;
            }
        }
        //Check if the player actually has an object at any time.
        if (heldObj != null)
        {
            MoveObject();
        }

        void MoveObject()
        {
            //Checking the distance between the held object and then the hold area to make sure thats it's greater then 0.1.
            if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
            {
                //Where it's going to move to.
                Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
                heldObjRB.AddForce(moveDirection * pickupForce);
            }
        }

        void PickupObject(GameObject pickObj)
        {
            if (pickObj.GetComponent<Rigidbody>())
            {
                heldObjRB = pickObj.GetComponent<Rigidbody>();

                //Disables the gravity on the object when its been picked up.
                heldObjRB.useGravity = false;

                heldObjRB.drag = 10;

                //When object is picked up it's not going to rotate around.
                heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

                //Parents the object to the hold area.
                heldObjRB.transform.parent = holdArea;
                heldObj = pickObj;
            }
        }

        void DropObject()
        {
            //Enables the gravity on the object when its been dropped.
            heldObjRB.useGravity = true;

            heldObjRB.drag = 1;

            //When the object is dropped it can rotate again.
            heldObjRB.constraints = RigidbodyConstraints.None;

            //Unparents the object from the hold area.
            heldObj.transform.parent = null;
            heldObj = null;
        }
    }
}
