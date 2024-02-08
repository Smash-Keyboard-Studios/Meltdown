using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWall : MonoBehaviour
{
	private bool isShrinking;
	public float objectScale;

	// Start is called before the first frame update
	private void Start()
	{
		//set initial values for variables
		isShrinking = false;
		objectScale = 100f;
	}

	// Update is called once per frame
	private void Update()
	{
		//set scale to objectscale/100
		transform.localScale = Vector3.one * objectScale / 100;

		//remove object if objectscale < 1
		if (objectScale <= 1)
		{
			gameObject.SetActive(false);
		}
	}

	//reduces size of ice by 1/3 of its original size
	private void ShrinkIce()
	{
		objectScale -= 34;
	}

	const int shrinkPercent = 34;
	const float shrinkDelay = 0.03f;

	//gradually reduces the size over time to be -34%
	private IEnumerator ShrinkIceGradual()
	{
		isShrinking = true;
		for (int i = 1; i < shrinkPercent; i++)
		{
			yield return new WaitForSeconds(shrinkDelay);
			objectScale -= 1;
		}
		isShrinking = false;
	}

	//replace fireScript with whatever script the fire projectile contains
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Fire>() != null)
		{
			if (!isShrinking)
			{
				StartCoroutine("ShrinkIceGradual");
			}
		}
	}
}
