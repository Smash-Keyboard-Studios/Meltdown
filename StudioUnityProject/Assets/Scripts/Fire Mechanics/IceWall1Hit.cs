using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWall1Hit : MonoBehaviour
{
    const int shrinkPercent = 100;
    const float shrinkDelay = 0.03f;

    private bool isShrinking;
	private Vector3 originalScale;
	public float objectScale;

	[SerializeField] private ParticleSystem steamParticles;

	// Start is called before the first frame update
	private void Start()
	{
		//set initial values for variables
		isShrinking = false;
		originalScale = transform.localScale;
		objectScale = 100f;

		steamParticles = GetComponentInChildren<ParticleSystem>();
		steamParticles.Stop();
	}

	// Update is called once per frame
	private void Update()
	{
		//set scale to objectscale/100
		transform.localScale = originalScale * objectScale / 100;

        //remove object if objectscale < 1
        if (objectScale <= 1)
        {
            gameObject.SetActive(false);
        }
	}

	////reduces size of ice by 1/3 of its original size
	//private void ShrinkIce()
	//{
	//	objectScale -= shrinkPercent;
	//}

	//gradually reduces the size over time to be -34%
	private IEnumerator ShrinkIceGradual()
	{
		isShrinking = true;
        steamParticles.Play();

        for (int i = 1; i < shrinkPercent; i++)
		{
			yield return new WaitForSeconds(shrinkDelay);
			objectScale -= 1;
		}

		isShrinking = false;
        steamParticles.Stop();
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
