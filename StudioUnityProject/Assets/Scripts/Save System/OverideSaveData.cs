using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
// ! ===============================
using UnityEditor;

[CustomEditor(typeof(OverideSaveData)), CanEditMultipleObjects]
public class OverideSaveDataButtons : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		OverideSaveData myScript = (OverideSaveData)target;

		if (GUILayout.Button("Force sensitivty")) myScript.SaveNewSens();
	}
}
// ! ===============================
#endif

/// <summary>
/// Used to overide save data.
/// </summary>
public class OverideSaveData : MonoBehaviour
{

	[SerializeField] public float sensitivity = 1f;

	void Start()
	{

		sensitivity = SaveData.Current.Sensitivity;

		// event listener
		SaveManager.current.onSave += OnSaveGame;
	}

	public void OnSaveGame()
	{


		sensitivity = SaveData.Current.Sensitivity;

	}

	public void SaveNewSens()
	{
		SaveData.Current.Sensitivity = sensitivity;
		SerializationManager.Save("0", SaveData.Current);
		SaveManager.current.GameSaveInvoke();
	}
}
