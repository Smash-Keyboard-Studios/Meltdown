using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElementalCore : MonoBehaviour
{
    public UnityEvent OnActivate;

    public Material ElementalCoreFireMaterial;
    public Material ElementalCoreIceMaterial;
    public GameObject ElementalCoreObject;


    private void OnCollisionEnter(Collision collision)  
    {
        print("REEEEEEEEEEEEEEE");
        if (collision.collider.GetComponent<Fire>() != null)
        {
            ElementalCoreObject.GetComponent<Material>.
        }
        else if(collision.collider.GetComponent<Ice>() != null)
        {
            Debug.Log("Collider has hit Ice");
            OnActivate.Invoke();
            if (FirePrefab.activeInHierarchy == true)
            {
                FirePrefab.SetActive(false);
            }
            IcePrefab.SetActive(true);
        }
    }

}
