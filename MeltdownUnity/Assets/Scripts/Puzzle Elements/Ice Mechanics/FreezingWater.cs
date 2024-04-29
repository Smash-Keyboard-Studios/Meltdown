using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingWater : MonoBehaviour
{
	public GameObject IceCube;

	public float UnFreezTimer = 15f;

	GameObject go;

    private void Awake()
    {
		transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Ice>() != null)
		{

			go = Instantiate(IceCube, transform.localPosition, Quaternion.identity, transform);
			//call Audio Manager (SFX)

			go.transform.localScale = new Vector3(10f, 0.1f, 10f);

			StartCoroutine(FreezRun());
		}

		if (other.gameObject.GetComponent<Fire>() != null)
		{
			StopCoroutine(FreezRun());
			if (go != null) Destroy(go);
		}
	}

	private IEnumerator FreezRun()
	{
		yield return new WaitForSeconds(UnFreezTimer);
		if (go != null) Destroy(go);
		StopCoroutine(FreezRun());
	}
}
