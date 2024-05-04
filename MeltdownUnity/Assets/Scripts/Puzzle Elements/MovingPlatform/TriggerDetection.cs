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
		try
		{

			if (IgnoredLayers == (IgnoredLayers | (1 << other.gameObject.layer))) return;
			if (CompareTag(other.transform)) return;

			// yes I hate it too		
			if (other.transform.parent != null)
				if (other.transform.parent.name == "HoldArea") return;

			entities.Add(new Entity(other.transform.parent, other.transform));
			if (other.transform.tag == PlayerTag)
			{
				_playerID = entities.Count - 1;
				pmc = other.transform.GetComponent<PlayerMovementController>();
				pmc.LimitMovementSpeedToMaxSpeed = false;
			}
			else if (other.GetComponent<Rigidbody>() == null)
			{
				other.transform.SetParent(transform, true);
			}
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
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
					else if (other.GetComponent<Rigidbody>() == null)
					{
						other.GetComponent<Rigidbody>().angularDrag = 0.05f;
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

	void OnTriggerStay(Collider other)
	{
		try
		{
			if (CompareTag(other.transform)) return;

			// can optimise this, I wont as time is a pain. :yes:
			if (other.transform.parent == null)
			{
				foreach (var entity in entities)
				{
					if (other.transform == entity.self) return;
				}

				entities.Add(new Entity(other.transform.parent, other.transform));
			}
			else
			{
				if (other.transform.parent.name == "HoldArea")
				{
					foreach (var entity in entities)
					{
						if (other.transform == entity.self) entities.Remove(entity);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.Message);
		}
	}

	void Update()
	{
		try
		{
			if (_playerID != -1)
			{
				for (int i = 0; i < entities.Count; i++)
				{
					if (entities[i].self == pmc.transform)
					{
						_playerID = i;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.Message);
		}

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


			//PlayerMovementController pmc = entities[_playerID].self.GetComponent<PlayerMovementController>();

			// create the vector with no y velocity.
			Vector3 velNoY = new Vector3(_velocity.x, 0, _velocity.z);

			if (pmc != null)
			{
				// stops y boosting.
				pmc.velocity += velNoY;

				// allows the player to go down on vertical moving platforms.
				pmc.velocity = new Vector3(pmc.velocity.x, (pmc.velocity.y < 0 ? -2 : pmc.velocity.y), pmc.velocity.z);
			}
		}

		foreach (var entity in entities)
		{
			if (entity.self.tag != PlayerTag && entity.self.GetComponent<Rigidbody>() != null)
			{
				Rigidbody rb = entity.self.GetComponent<Rigidbody>();

				rb.angularDrag = 10000f;

				// allows the player to go down on vertical moving platforms.

				Vector3 velocityForEntity = new Vector3(_velocity.x, (rb.velocity.y < -2 ? -2 : rb.velocity.y), _velocity.z);

				if (_velocity.x <= 0 && _velocity.x >= 0) velocityForEntity.x = 0;
				if (_velocity.z <= 0 && _velocity.z >= 0) velocityForEntity.z = 0;


				rb.velocity = velocityForEntity;
			}
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
