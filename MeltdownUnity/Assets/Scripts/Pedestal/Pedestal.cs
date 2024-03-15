using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    [Header ("Amount ammo is refilled by")] public int RefillAmount;
    [Header ("Set True for ice refill and false for fire refill")] public bool IsIcePedestal;

    public GameObject PlayerObject;
    public AmmoController AmmoController;

    private void Start()
    {
        //set player object
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        //get ammocontroller from player prefab
        AmmoController = PlayerObject.GetComponentInChildren<AmmoController>();
    }

    //interact with pedestal triggers refill ammo script
    public void UsePedestal(){
        //if ice then refill ice ammo with refillamount
        if(IsIcePedestal){
            AmmoController.IceAmmo = AmmoController.RefillPedestal(AmmoController.IceAmmo, RefillAmount);
        }
        else
        //if fire then refill fire ammo with refillamount
        {
            AmmoController.FireAmmo = AmmoController.RefillPedestal(AmmoController.FireAmmo, RefillAmount);
        }
    }
}
