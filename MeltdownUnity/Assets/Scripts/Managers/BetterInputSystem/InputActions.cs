using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for input actions.
/// </summary>
public static class InputActions
{
	/// <summary>
	/// All base input. think of it as the defult values.
	/// If you want any defult values, they will go here
	/// </summary>

	// TODO Remove to replace at some point, dont forget about save system.
	// defult keys.
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

	public static KeyCode UIKey = KeyCode.Escape;

	/// <summary>
	/// the differnt actions the player can do.
	/// </summary>
	public enum KeyAction
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
		UI,
		Spare
	}

	/// <summary>
	/// Gets the defualt value for the given action.
	/// Once you add a new action to the KeyType enum, you can set the defult value here.
	/// It iterates through to see anymatches and returns the KeyCode.
	/// </summary>
	/// <param name="keyAction"></param>
	/// <returns>KeyCode</returns>
	public static KeyCode GetDefultValues(KeyAction keyAction)
	{
		KeyCode returnValue = KeyCode.None;

		switch (keyAction)
		{
			case KeyAction.Forward:
				returnValue = ForwardKey;
				break;
			case KeyAction.Backward:
				returnValue = BackwardsKey;
				break;
			case KeyAction.Left:
				returnValue = LeftStrafeKey;
				break;
			case KeyAction.Right:
				returnValue = RightStrafeKey;
				break;
			case KeyAction.Sprint:
				returnValue = SprintKey;
				break;
			case KeyAction.Jump:
				returnValue = JumpKey;
				break;
			case KeyAction.Crouch:
				returnValue = CrouchKey;
				break;
			case KeyAction.ShootFire:
				returnValue = ShootFireKey;
				break;
			case KeyAction.ShootIce:
				returnValue = ShootIceKey;
				break;
			case KeyAction.Interact:
				returnValue = InteractKey;
				break;
			case KeyAction.UI:
				returnValue = UIKey;
				break;
			default:
				returnValue = KeyCode.None;
				break;
		}

		return returnValue;
	}
}
