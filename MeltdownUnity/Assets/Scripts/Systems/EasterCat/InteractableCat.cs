using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCat : MonoBehaviour, IInteractable
{
	public string ObjectName = "Cat";

	// public GameObject CatImage;

	[HideInInspector]
	public bool Collected = false;

	// magic
	string IInteractable.ObjectName { get => ObjectName; set => ObjectName = value; }

	void IInteractable.Interact()
	{
		CollectCat();
	}

	void Awake()
	{


	}

	// Start is called before the first frame update
	void Start()
	{
		gameObject.tag = "InteractableObject";

		if (CatTracker.Current != null)
		{
			CatTracker.Current.CatAmmountOnLevel++;
		}
	}

	public void CollectCat()
	{
		if (CatTracker.Current != null && !Collected)
		{
			CatTracker.Current.CollectCat();

			Display.Current.CreateDisplayText($"Collected cat {CatTracker.Current.CurrentCollected}/{CatTracker.Current.CatAmmountOnLevel}", 3, 1.8f);

			Collected = true;

			Destroy(this.gameObject);
		}
	}
}
