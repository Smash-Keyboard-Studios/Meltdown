using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockDestroyer : MonoBehaviour
{
    public IceCubeSpawner iceCubeSpawner;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnDestroy()
    {
        iceCubeSpawner.RemoveCube();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
