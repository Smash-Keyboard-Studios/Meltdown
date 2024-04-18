using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	private static SaveData _current;
	public static SaveData Current
	{
		get
		{
			if (_current == null)
			{
				
					Debug.LogError("Save Manager does not exist. Cannot save any data!");
				return null;
				
			}

			return _current;
		}
		set
		{
			_current = value;
		}
	}

	//public PlayerProfile Profile;

	public float Sensitivity = 1f;
	public float MaxVolume = 50f;
	public bool ToggleCrouch = false;

	public int CurrentLevel = 0;

	// keybinds
}
