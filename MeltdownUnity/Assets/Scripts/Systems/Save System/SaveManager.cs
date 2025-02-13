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

[DefaultExecutionOrder(-5)]
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

	public event Action onLoad;
	public void GameLoadInvoke()
	{
		if (onSave != null)
		{
			onLoad();
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
		{
            SaveData.Current = new SaveData();
            SerializationManager.Save("0", SaveData.Current);
		}
		else
		{
			SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/0.save");
			GameLoadInvoke();
		}
	}

	/// <summary>
	/// This should be called to save not the SerializationManager.
	/// This has a event tied to it.
	/// </summary>
	public void ForceSave()
	{
        if (SerializationManager.Load(Application.persistentDataPath + "/saves/0.save") == null) { SaveData.Current = new SaveData(); }
        SerializationManager.Save(saveName, SaveData.Current);
		GameSaveInvoke();
	}

	public void ForceLoad()
	{
		SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/0.save");
		GameLoadInvoke();
	}
}
