using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ElementalCore : MonoBehaviour
{


    public UnityEvent FireActivate;
    public UnityEvent IceDeactivate;
    public UnityEvent IceActivate;
    public UnityEvent FireDeactivate; // Creates unity events for ice/fire activations/deactivations


    private bool FireElementalCoreActivated = false;
    private bool IceElementalCoreActivated = false; // Initilisation of booleans to check whether or not the core is already engaged 


    public Texture ElementalCoreFireMaterial;
    public Texture ElementalCoreIceMaterial; // Gets the materials for both the fire/ice core materials

    private Texture ElementalCoreObjectMaterialStatic; // Creates a static variable to store the base elemental core material
    private Material ElementalCoreObjectMaterial; // Gets a variable to allow for changing of the elemental core material 

    private GameObject FireWireHolder;
    private GameObject IceWireHolder; // Creates placeholder game objects in script to allow for easy access to the fire and ice holders

    private Color FireWireColor;
    private Color IceWireColor;
    private Color DeactivatedWireColor; // Creates easy access to the different wire colors

    private void Start()
    {
        ElementalCoreObjectMaterialStatic = transform.GetComponent<Renderer>().material.mainTexture;
        ElementalCoreObjectMaterial = transform.GetComponent<Renderer>().material; // Gets both the texture and material for main elemental core

        FireWireHolder = transform.parent.Find("FireWireHolder").gameObject;
        IceWireHolder = transform.parent.Find("IceWireHolder").gameObject; // Finds and stores the fire and ice wire holders

        FireWireColor = FireWireHolder.GetComponentInChildren<Renderer>().material.color;
        IceWireColor = IceWireHolder.GetComponentInChildren<Renderer>().material.color; // Finds and stores the oriignal fire and ice wire colors
        DeactivatedWireColor = Color.black;


        foreach(Transform child in FireWireHolder.transform)
        {
            child.GetComponent<Renderer>().material.color = DeactivatedWireColor;
            child.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }
        foreach (Transform child in IceWireHolder.transform)
        {
            child.GetComponent<Renderer>().material.color = DeactivatedWireColor;
            child.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        } // Goes through both holders and deactivates all wires
    }


    private void OnCollisionEnter(Collision collision)  
    {
        if (collision.collider.GetComponent<Fire>() != null && IceElementalCoreActivated != true) // Checks if the ice core is active and if fire hit
        {
            if (FireElementalCoreActivated != true) // To prevent it falsely activating the other conditions, it checks to see if fire elemental core is active here
            {
                // A sound of fire or something should be put here
                FireActivate.Invoke(); // Invokes the fire activation 
                ElementalCoreObjectMaterial.SetTexture("_BaseMap", ElementalCoreFireMaterial); // Sets elemental core to the fire core texture
                foreach (Transform child in FireWireHolder.transform)
                {
                    child.GetComponent<Renderer>().material.color = FireWireColor;
                } // Goes through all fire wires and sets them to the correct color
                FireElementalCoreActivated = true;  // Ensures program knows fire core is currently engaged
            }
        }
        else if (collision.collider.GetComponent<Ice>() != null && FireElementalCoreActivated != true) // Checks if the fire core is active and if ice hit
        {
            if (IceElementalCoreActivated != true) // To prevent it falsely activating the other conditions, it checks to see if ice elemental core is active here
            {
                // A sound of ice or something should be put here
                IceActivate.Invoke(); // Invokes the ice activation
                ElementalCoreObjectMaterial.SetTexture("_BaseMap", ElementalCoreIceMaterial); // Sets elemental core to the ice core texture
                foreach (Transform child in IceWireHolder.transform)
                {
                    child.GetComponent<Renderer>().material.color = IceWireColor;
                } // Goes through all ice wires and sets them to the correct color
                IceElementalCoreActivated = true; // Ensures program knows ice core is currently engaged
            }
        }
        else if(FireElementalCoreActivated == true) // If above conditions failed, checks if fire core is currently engaged
        {
            // Deactivation sound should be put here
            IceDeactivate.Invoke(); // Invokes ice deactivation, the counteraction to the fire activation
            ElementalCoreObjectMaterial.SetTexture("_BaseMap", ElementalCoreObjectMaterialStatic); // Sets the elemental core to the original texture
            foreach (Transform child in FireWireHolder.transform)
            {
                child.GetComponent<Renderer>().material.color = DeactivatedWireColor;
            } // Goes through all the fire wires and deactivates the colour
            FireElementalCoreActivated = false; // Deactivates the boolean
        }
        else if (IceElementalCoreActivated == true) // If above conditions failed, checks if ice core is currently engaged
        {
            // Deactivation sound should be put here
            FireDeactivate.Invoke(); // Invokes fire deactivation, the counteraction to the ice activation
            ElementalCoreObjectMaterial.SetTexture("_BaseMap", ElementalCoreObjectMaterialStatic); // Sets the elemental core to the original texture
            foreach (Transform child in IceWireHolder.transform)
            {
                child.GetComponent<Renderer>().material.color = DeactivatedWireColor;
            } // Goes through all the ice wires and deactivates the colour
            IceElementalCoreActivated = false; // Deactivates the boolean
        }
    }

}
