using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWall : MonoBehaviour
{
	//delay num of seconds between each 1% of shrinking
	const float shrinkDelay = 0.03f;

	private bool isShrinking;
	private Vector3 originalScale;
	public float objectScale;

	[Header("Number of Hits Needed to Destroy Wall")] public int numOfHits = 3; //this is number of hits needed
	private int shrinkPercent;

	//ice audiosource
	public AudioSource IceSource;

	//steam particles
	[SerializeField] private ParticleSystem steamParticles;

	//water puddle object that will be created when melting the ice
	[SerializeField] private GameObject WaterPuddle;

	private bool _isWaitingOnParticles = false;

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
		//set shrink percent to correspond with number of hits
		shrinkPercent = (100 / numOfHits) + 1;

		//set scale to objectscale/100
		transform.localScale = originalScale * objectScale / 100;

		//remove object if objectscale <= 1
		if (objectScale <= 1 && !_isWaitingOnParticles)
		{
			gameObject.SetActive(false);
		}
	}

	////reduces size of ice by 1/3 of its original size without any transition
	//private void ShrinkIce()
	//{
	//	objectScale -= shrinkPercent;
	//}

	//gradually reduces the size over time to be -34%
	private IEnumerator ShrinkIceGradual()
	{
		isShrinking = true;
		steamParticles.Play();
		StartCoroutine(WaitingForParticles());

		for (int i = 1; i < shrinkPercent; i++)
		{
			yield return new WaitForSeconds(shrinkDelay);
			objectScale -= 1;
		}

		isShrinking = false;
		//steamParticles.Stop();

		//create instance of water puddle after shrinking
		Instantiate(WaterPuddle, transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity);
	}

	private IEnumerator WaitingForParticles()
	{
		_isWaitingOnParticles = true;
		yield return new WaitForSeconds(steamParticles.main.duration);
		_isWaitingOnParticles = false;
	}

	//replace fireScript with whatever script the fire projectile contains
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Fire>() != null)
		{
			if (!isShrinking)
			{
				//play audio
				IceSource.Play();

				StartCoroutine("ShrinkIceGradual");
				//create instance of water puddle after shrinking
			}
		}
	}
}
