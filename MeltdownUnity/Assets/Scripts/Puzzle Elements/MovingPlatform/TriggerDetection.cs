using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
	[Serializable]
	struct Entity
	{
		public Transform parent;
		public Transform self;
		public Entity(Transform theParent, Transform theSelf)
		{
			parent = theParent;
			self = theSelf;
		}
	}

	public string PlayerTag = "Player";
	private int _playerID = -1;

	public LayerMask IgnoredLayers;
	public string[] IgnoredTags;

	public Vector3 _velocity;
	public Vector3 _lastPos;

	private PlayerMovementController pmc;

	List<Entity> entities = new List<Entity>();

	void Start()
	{
		_lastPos = transform.position;
	}

	void OnTriggerEnter(Collider other)
	{
		if (IgnoredLayers == (IgnoredLayers | (1 << other.gameObject.layer))) return;
		if (CompareTag(other.transform)) return;

		entities.Add(new Entity(other.transform.parent, other.transform));
		if (other.transform.tag == PlayerTag)
		{
			_playerID = entities.Count - 1;
			pmc = other.transform.GetComponent<PlayerMovementController>();
			pmc.LimitMovementSpeedToMaxSpeed = false;
		}
		else
		{
			other.transform.SetParent(transform, true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		try
		{
			if (entities.Count <= 0) return;

			foreach (var entity in entities)
			{

				if (entity.self == other.transform)
				{

					// :vomit the ammoint of nesting.
					if (other.transform.tag == PlayerTag)
					{
						_playerID = -1;
						pmc.LimitMovementSpeedToMaxSpeed = true;

						// limit the speed of the player.
						if (!pmc.isGrounded && pmc.velocity.magnitude > pmc.SprintSpeed)
						{
							pmc.velocity = pmc.velocity.normalized * pmc.SprintSpeed;
						}
					}
					else
					{
						other.transform.SetParent(entity.parent, true);
					}


					entities.Remove(entity);

					break;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex);
		}


	}

	void Update()
	{
		if (_lastPos != transform.position)
		{
			_velocity = (transform.position - _lastPos) / Time.deltaTime;
			_lastPos = transform.position;
		}
		else
		{
			_velocity = Vector3.zero;
		}

		if (_playerID != -1 && pmc.isGrounded)
		{
			// want to add force.


			PlayerMovementController pmc = entities[_playerID].self.GetComponent<PlayerMovementController>();

			Vector3 velNoY = new Vector3(_velocity.x, 0, _velocity.z);


			pmc.velocity += velNoY;

			pmc.velocity = new Vector3(pmc.velocity.x, (pmc.velocity.y < 0 ? -2 : pmc.velocity.y), pmc.velocity.z);
		}


	}

	// Function to compare tag of a collision.
	private bool CompareTag(Transform other)
	{
		if (IgnoredTags.Length > 0)
		{
			foreach (var tag in IgnoredTags)
			{
				if (other.gameObject.CompareTag(tag)) return true;
			}
		}

		return false;
	}
}
