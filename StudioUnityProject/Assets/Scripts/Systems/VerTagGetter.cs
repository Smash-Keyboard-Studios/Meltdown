using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VerTagGetter : MonoBehaviour
{
	private TMP_Text text;

	// Start is called before the first frame update
	void Start()
	{
		text = GetComponent<TMP_Text>();
		text.text = $"ver: {Application.version}";
	}

	// Update is called once per frame
	void Update()
	{

	}
}
