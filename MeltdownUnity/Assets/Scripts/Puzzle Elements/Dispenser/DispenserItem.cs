using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// </summary>
/// This script is added upon being dispensed by a ceiling dispenser
/// Destroys object when going below a certain threshold when entering a kill zone
/// Required to relay information back to dispenser
/// </summary>

public class DispenserItem : MonoBehaviour
{
    public ObjectDispenser owner;
    public float possibleDeathHeight = -100; // The height at which the object must go below before being destroyed

    private bool beingDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KillZone>() != null && !beingDestroyed)
        {
            possibleDeathHeight = other.transform.position.y; // Sets the new death height to the kill zone Y coord
            StartCoroutine(HeightBelowCheck());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<KillZone>() != null && !beingDestroyed)
        {
            if (transform.position.y > possibleDeathHeight)
            {
                StopCoroutine(HeightBelowCheck()); // Object is no longer below kill zone
            }
        }

    }

    IEnumerator HeightBelowCheck()
    {
        while (true)
        {
            if (transform.position.y < possibleDeathHeight - 2)
            {

                StartCoroutine(DestroySelf());
                StopCoroutine(HeightBelowCheck());
            }
            yield return new WaitForSeconds(.5f); // Stalls for half a second before repeating
        }
    }

    IEnumerator DestroySelf()
    {
        for (int i = 1; i <= 15; i++)
        {
            gameObject.transform.localScale /= 1.2f;
            yield return new WaitForSeconds(.1f);
        }

        owner.FreeUpSpace();
        Destroy(this.gameObject);
        StopCoroutine(DestroySelf());

    }
}
