using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
	[Header("This must be in order with the other checkpoints, 0  is start")]
	public int CheckpointID = -1;

	public Transform playerSpawnLocation;

	// Start is called before the first frame update
	void Start()
	{
		if (CheckpointID == -1) Debug.LogError("ID CANNOT BE -1, THEY MUST BE IN ORDER"); // yell at the desingers.
	}

	// Update is called once per frame
	void Update()
	{

	}


	private void OnTriggerEnter(Collider other)
	{

		if (other.transform.tag == "Player")
		{
			CheckpointManager.Current.CurrentCheckpointID = CheckpointID;
			CheckpointManager.Current.CheckpointPos = playerSpawnLocation.position;
			CheckpointManager.Current.FireAmmo = PlayerFinder.Current.GetPlayerTransform().GetComponent<AmmoController>().FireAmmo;
			CheckpointManager.Current.IceAmmo = PlayerFinder.Current.GetPlayerTransform().GetComponent<AmmoController>().IceAmmo;
			CheckpointManager.Current.RotationOfPlayer = PlayerFinder.Current.GetPlayerTransform().rotation;
			GetComponent<BoxCollider>().enabled = false;
		}
	}
}
