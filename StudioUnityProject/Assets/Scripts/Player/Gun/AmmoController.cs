using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public Gun GunScript;

    public int IceAmmo;
    public int FireAmmo;

    public int LargePedestalRefill = 3;
    public int SmallPedestalRefill = 1;

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

    public int RefillLargePedestal(int Ammo){
        Ammo = LargePedestalRefill;
        return Ammo;
    }

    public int RefillSmallPedestal(int Ammo){
        if(Ammo < SmallPedestalRefill){
            Ammo = SmallPedestalRefill;
        }
        return Ammo;
    }
}
