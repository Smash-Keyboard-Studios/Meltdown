using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BasicMovementNotUsed : MonoBehaviour
{
    // Start is called before the first frame update

    Transform playerTransform;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        }
    }
}
