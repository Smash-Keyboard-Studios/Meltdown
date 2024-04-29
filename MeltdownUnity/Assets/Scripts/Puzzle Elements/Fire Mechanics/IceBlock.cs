using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
	//delay num of seconds between each 1% of shrinking
	const float shrinkDelay = 0.03f;

	private bool isShrinking;
    private bool isDestroyed;

	private Vector3 originalScale;

    //this is a scale from 1 to 4, 1 being the original 3m and 4 being 0.5m
    //the object scale will be its original scale / objectsetscale, so 1/1, 1/2, 1/3, 1/4
	[Header ("Object Size = 1 / ")] public float objectSetScale;

    //object scale is object set scale x 100
    public float objectScale;

    //smallest size it can shrink to
    [Header ("Max Shrink = 1 / ")] public float maxShrinkSize;

	//steam particles
    [SerializeField] private ParticleSystem steamParticles;

	//water puddle object that will be created when melting the ice
	[SerializeField] private GameObject WaterPuddle;

	//audio stuff
	public AudioSource AudioSource;

	// Start is called before the first frame update
	private void Start()
	{
		//set initial values for variables
		isShrinking = false;
        isDestroyed = false;
		originalScale = transform.localScale;

        objectScale = objectSetScale * 100;

		steamParticles = GetComponentInChildren<ParticleSystem>();
		steamParticles.Stop();

		AudioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	private void Update()
	{
		//set scale to original scale / objectsetscale
        //but use object scale to do this so it is gradual shrink
		transform.localScale = originalScale * 100 / objectScale;

        //remove object if objectsetscale is too small
        if (objectSetScale > maxShrinkSize && !isDestroyed)
        {
            StartCoroutine("RemoveIceBlock");
            isDestroyed = true;
        }
	}

	//gradually reduces the size over time
	private IEnumerator ShrinkIceGradual()
	{
        //AUDIO GUY PUT SOME SIZZLY SOUNDS FOR STEAM HERE PLS!!!
		AudioSource.Play();

		isShrinking = true;
        steamParticles.Play();

        //add one to object set scale
        objectSetScale += 1;

        //for loop repeat 100 times
        for (int i = 1; i < 100; i++)
		{
			yield return new WaitForSeconds(shrinkDelay);
			objectScale += 1;
		}

		isShrinking = false;
        steamParticles.Stop();

        //object scale is object set scale x 100
        objectScale = objectSetScale * 100;

		//create instance of water puddle after shrinking, cancelled for now
		//Instantiate(WaterPuddle, transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity);
    }

    private IEnumerator RemoveIceBlock()
    {
        yield return new WaitForSeconds(shrinkDelay * 100);
        gameObject.SetActive(false);
    }

	//replace fireScript with whatever script the fire projectile contains
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Fire>() != null)
		{
			if (!isShrinking)
			{
				StartCoroutine("ShrinkIceGradual");
				//create instance of water puddle after shrinking
			}
		}
	}
}
