using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;




#if UNITY_EDITOR
// ! ===============================
using UnityEditor;

[CustomEditor(typeof(SaveDy)), CanEditMultipleObjects]
public class UIForSaveDy : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		SaveDy myScript = (SaveDy)target;
		if (GUILayout.Button("DycriptSave")) myScript.DycriptSave();
	}
}
// ! ===============================
#endif


public class SaveDy : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void DycriptSave()
	{
		if (!Directory.Exists(Application.persistentDataPath + "/saves"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/saves");
		}

		// string save = "0.save";
		string path = Application.persistentDataPath + "/saves/";

		// FileStream file = File.Create(Path.Combine(path, save));

		SaveData saveData = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/0.save");


		using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "Decrypt.txt")))
		{

			outputFile.WriteLine("Sense: " + saveData.Sensitivity);
			outputFile.WriteLine("Volume: " + saveData.MaxVolume);
			outputFile.WriteLine("Toggle crouch: " + saveData.ToggleCrouch);

			outputFile.WriteLine("Level Stat: ");
			foreach (var res in saveData.CollectedOnLevel)
			{
				outputFile.WriteLine($"{res.Key} {res.Value}");
			}
			outputFile.WriteLine("End Stat ");

			outputFile.WriteLine("Last Level: " + saveData.CurrentLevel);

			outputFile.Close();
		}
	}
}
