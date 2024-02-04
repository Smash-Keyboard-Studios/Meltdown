using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActions : MonoBehaviour
{
	/// <summary>
	/// All base input. think of it as the defult values.
	/// If you want any defult values, they will go here
	/// </summary>

	// TODO Remove to replace at some point, dont forget about save system.
	public static KeyCode ForwardKey = KeyCode.W;
	public static KeyCode BackwardsKey = KeyCode.S;
	public static KeyCode LeftStrafeKey = KeyCode.A;
	public static KeyCode RightStrafeKey = KeyCode.D;

	public static KeyCode SprintKey = KeyCode.LeftShift;
	public static KeyCode JumpKey = KeyCode.Space;
	public static KeyCode CrouchKey = KeyCode.C;

	public static KeyCode ShootFireKey = KeyCode.Mouse0;
	public static KeyCode ShootIceKey = KeyCode.Mouse1;

	public static KeyCode InteractKey = KeyCode.E;

	/// <summary>
	/// the diffet actions the player can do.
	/// </summary>
	public enum KeyType
	{
		Forward,
		Left,
		Backward,
		Right,
		Sprint,
		Jump,
		Crouch,
		ShootFire,
		ShootIce,
		Interact,
		Spare
	}

	/// <summary>
	/// Gets the defualt value for the given action.
	/// Once you add a new action to the KeyType enum, you can set the defult value here.
	/// It iterates through to see anymatches and returns the KeyCode.
	/// </summary>
	/// <param name="KeyType"></param>
	/// <returns>KeyCode</returns>
	public static KeyCode GetDefultValues(KeyType KeyType)
	{
		KeyCode returnValue = KeyCode.None;

		switch (KeyType)
		{
			case KeyType.Forward:
				returnValue = ForwardKey;
				break;
			case KeyType.Backward:
				returnValue = BackwardsKey;
				break;
			case KeyType.Left:
				returnValue = LeftStrafeKey;
				break;
			case KeyType.Right:
				returnValue = RightStrafeKey;
				break;
			case KeyType.Sprint:
				returnValue = SprintKey;
				break;
			case KeyType.Jump:
				returnValue = JumpKey;
				break;
			case KeyType.Crouch:
				returnValue = CrouchKey;
				break;
			case KeyType.ShootFire:
				returnValue = ShootFireKey;
				break;
			case KeyType.ShootIce:
				returnValue = ShootIceKey;
				break;
			case KeyType.Interact:
				returnValue = InteractKey;
				break;
			default:
				returnValue = KeyCode.None;
				break;
		}

		return returnValue;
	}
}
