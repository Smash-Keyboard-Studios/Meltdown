using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextCount : MonoBehaviour
{
	private TMP_Text text;

	// Start is called before the first frame update
	void Start()
	{
		text = GetComponent<TMP_Text>();
	}

	// Update is called once per frame
	void Update()
	{
		if (SaveManager.current == null || SaveData.Current == null)
		{
			text.text = "NULL";
			return;
		}

		float total = SaveData.Current.CollectedOnLevel.Count;

		float count = 0;

		foreach (var item in SaveData.Current.CollectedOnLevel)
		{
			if (item.Value)
			{
				count++;
			}
		}

		float math = (count / total) * 100f;

		text.text = math.ToString("##") + "%";
	}
}
