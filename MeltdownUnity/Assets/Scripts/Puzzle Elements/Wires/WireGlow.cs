using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WireGlow : MonoBehaviour
{

    public Material Off;
    public Material On;

    private bool b = false;

    private void Update()
    {
        foreach (var child in transform.GetComponentsInChildren<Renderer>())
        {
            if (b)
            {
                child.material = On;
            }
            else
            {
                child.material = Off;
            }
        }

        
    }

    public void Glow()
    {
        b = !b;
        //MaterialGlow.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;

    }

    

}
