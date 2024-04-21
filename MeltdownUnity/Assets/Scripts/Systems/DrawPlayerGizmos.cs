using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPlayerGizmos : MonoBehaviour
{
	public Mesh PlayerMesh;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnDrawGizmos()
	{
		if (PlayerMesh == null) return;

		Gizmos.DrawMesh(PlayerMesh, transform.position, transform.rotation, new Vector3(0.5f, 0.85f, 0.5f));
	}
}
