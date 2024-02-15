using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeIcePedestal : MonoBehaviour
{
    public GameObject PlayerObject;
    public AmmoController AmmoController;

    private void Start()
    {
        //get ammocontroller from player prefab
        AmmoController = PlayerObject.GetComponentInChildren<AmmoController>();
    }

    //interact with pedestal triggers refill ammo script
    public void UsePedestal(){
        AmmoController.IceAmmo = AmmoController.RefillLargePedestal(AmmoController.IceAmmo);
    }
}
