using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System;

/// <summary>
/// This handles loading and saving the save data to the save file.
/// </summary>
public class SaveManager : MonoBehaviour
{
	public static SaveManager current;

	public string saveName = "0";

	/// <summary>
	/// C# event
	/// </summary>
	public event Action onSave;
	public void GameSaveInvoke()
	{
		if (onSave != null)
		{
			onSave();
		}
	}

	void Awake()
	{
		if (current != null && current != this)
		{
			Destroy(this);
		}
		else
		{
			current = this;
			DontDestroyOnLoad(this);
		}

		if (SerializationManager.Load(Application.persistentDataPath + "/saves/0.save") == null)
			SerializationManager.Save("0", SaveData.Current);
		else
			SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/0.save");
	}

	/// <summary>
	/// This should be called to save not the SerializationManager.
	/// This has a event tied to it.
	/// </summary>
	public void ForceSave()
	{
		SerializationManager.Save(saveName, SaveData.Current);
		GameSaveInvoke();
	}
}
