using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;

public class VSEvent : MonoBehaviour
{


	// Start is called before the first frame update
	void Start()
	{
		// EventBus.Register<int>(EventNames.MyCustomEvent, i =>
		//   {
		// 	  Debug.Log("RECEIVED " + i);
		//   });
	}

	// Update is called once per frame
	void Update()
	{

	}
}
