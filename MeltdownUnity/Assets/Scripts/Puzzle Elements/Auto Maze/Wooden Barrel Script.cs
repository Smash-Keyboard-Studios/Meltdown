using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

    
public class WoodenBarrelScript : MonoBehaviour
{
    //Components of the barrel
    private Rigidbody rb;

    //Checks whether the barrel should be rolling or not
    private bool barrelRolling = false;


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        //Assigning values of components
        rb = GetComponent<Rigidbody>();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void FixedUpdate()
    {
        //Stops the barrel rolling if the puzzle has not started or if it is completed
        if (barrelRolling && !AutoMaze1Script.puzzleComplete)
        {
            rb.AddForce(new Vector3(-1, 0, -1), ForceMode.Force);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void RollBarrel()
    {
        //Causes barrel to roll towards the exit of the maze 
        StartCoroutine(EnableBarrelRolling());
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    private IEnumerator EnableBarrelRolling()
    {
        yield return new WaitForSeconds(1f);
        barrelRolling = true;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
