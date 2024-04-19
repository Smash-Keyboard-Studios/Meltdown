using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
	public Gun GunScript;

	public int IceAmmo = 0;
	public int FireAmmo = 0;

	public bool InfAmmo = false;

	public Material NormalMat;
	public Material IceMat;
	public Material FireMat;

	public GameObject[] FireCanisters;
	public GameObject[] IceCanisters;
	// Start is called before the first frame update
	void Start()
	{
		GunScript = gameObject.GetComponent<Gun>();

	}

	// Update is called once per frame
	void Update()
	{
		//cap ice and fire ammo at 3
		if (InfAmmo)
		{
			IceAmmo = 3;
			FireAmmo = 3;
			return;
		}

		if (IceAmmo > 3)
		{
			IceAmmo = 3;
		}

		if (FireAmmo > 3)
		{
			FireAmmo = 3;
		}

		for (int i = 1; i <= 3; i++)
		{
			if (i <= IceAmmo) IceCanisters[i - 1].SetActive(true);
			if (i > IceAmmo) IceCanisters[i - 1].SetActive(false);
		}

		for (int i = 1; i <= 3; i++)
		{
			if (i <= FireAmmo) FireCanisters[i - 1].SetActive(true);
			if (i > FireAmmo) FireCanisters[i - 1].SetActive(false);
		}
	}

	public int RefillPedestal(int Ammo, int RefillAmount)
	{
		if (Ammo < RefillAmount)
		{
			Ammo = RefillAmount;
		}
		return Ammo;
	}

	public void SetMat(GameObject canister, Material mat)
	{
		canister.GetComponent<Renderer>().material = mat;
	}
}
