using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private bool isBeingHeld = false;
    private Transform previousParent;
    private float pickupRange = 2f;
    private Vector3 pickupOffset = new Vector3(0.1f, -0.2f, 1f);
    private Vector3 pickupRotation = new Vector3(90f, 0f, 0f);

    void Update()
    {
        // Check for input to pick up or drop the object
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isBeingHeld)
                TryPickUp();
            else
                Drop();
        }
    }

    void TryPickUp()
    {
        // Check if the object is in range and in the player's field of view
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.gameObject == gameObject)
                PickUp();
        }
    }

    void PickUp()
    {
       
        previousParent = transform.parent;

        // Set the object's parent to the main camera
        transform.SetParent(Camera.main.transform);

        transform.localPosition = pickupOffset;
        transform.localEulerAngles = pickupRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Set the object as being held
        SetBeingHeld(true);
    }

    void Drop()
    {
       
        transform.SetParent(previousParent);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        // Set the object as not being held
        SetBeingHeld(false);
    }

    //set whether the object is being held by the player
    public void SetBeingHeld(bool held)
    {
        isBeingHeld = held;
    }
}
