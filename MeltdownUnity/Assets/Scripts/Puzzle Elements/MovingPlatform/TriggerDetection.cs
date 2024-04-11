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

	List<Entity> entities = new List<Entity>();

	void OnTriggerEnter(Collider other)
	{
		entities.Add(new Entity(other.transform.parent, other.transform));
		other.transform.SetParent(transform, true);
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

					other.transform.SetParent(entity.parent, true);

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
}
