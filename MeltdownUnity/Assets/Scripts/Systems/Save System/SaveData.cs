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

	// touch and die
	public Dictionary<int, bool> CollectedOnLevel = new Dictionary<int, bool>
	{
		{2, false},
		{3, false},
		{4, false},
		{5, false},
		{6, false},
		{7, false},
		{8, false},
		{9, false},
	};

	// used to know the last level the player was on.
	public int CurrentLevel = 0;

	// keybinds
}
