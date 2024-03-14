using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public Gun GunScript;

    public int IceAmmo;
    public int FireAmmo;

    // Start is called before the first frame update
    void Start()
    {
        GunScript = gameObject.GetComponent<Gun>();

        //set ice and fire ammo to 0
        IceAmmo = 0;
        FireAmmo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //cap ice and fire ammo at 3
        if(IceAmmo > 3){
            IceAmmo = 3;
        }
        
        if(FireAmmo > 3){
            FireAmmo = 3;
        }
    }

    public int RefillPedestal(int Ammo, int RefillAmount){
        if(Ammo < RefillAmount){
            Ammo = RefillAmount;
        }
        return Ammo;
    }
}
