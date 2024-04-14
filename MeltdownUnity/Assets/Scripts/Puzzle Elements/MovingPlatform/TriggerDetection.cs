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

	private Vector3 _velocity;
	private Vector3 _lastPos;

	List<Entity> entities = new List<Entity>();

	void Start()
	{
		_lastPos = transform.position;
	}

	void OnTriggerEnter(Collider other)
	{
		entities.Add(new Entity(other.transform.parent, other.transform));
		if (other.transform.tag == PlayerTag)
		{
			_playerID = entities.Count - 1;
			other.transform.GetComponent<PlayerMovementController>().LimitMovementSpeedToMaxSpeed = false;
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
						other.transform.GetComponent<PlayerMovementController>().LimitMovementSpeedToMaxSpeed = true;
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

		if (_playerID != -1)
		{
			// want to add force.
			PlayerMovementController pmc = entities[_playerID].self.GetComponent<PlayerMovementController>();

			pmc.velocity += _velocity;
		}
	}
}
