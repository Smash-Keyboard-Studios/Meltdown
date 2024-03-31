using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class is how the moving platforms work.
/// </summary>
public class MovingPlatform : MonoBehaviour
{
	[Header("Piston Segments")]
	[Tooltip("The Piston segments")]
	public Transform[] Segments;

	[Tooltip("The average length of a piston segment")]
	public float LengthOfOneSegment;


	[Header("Pistion Variables")]
	[Tooltip("The max distance the moving platform is allowed to move")]
	public float MaxExtents = 1f;

	[Tooltip("Weather the pistion is extending or retraction")]
	public bool Extending = false;


	[Header("Loop Variables")]
	[Tooltip("Weather the pistion should extend and retract manually or automatic")]
	public bool Loop = false;

	[Tooltip("Weather the moving platform is stationary during loop")]
	public bool IsPaused = false;

	[Tooltip("How long the to pause for")]
	public float PauseTime = 1f;

	[Header("Speed Variables")]
	[Tooltip("The defult speed of the moving platform")]
	public float NormalSpeed = 1f;

	[Tooltip("The speed when frozen, use 0 to stop")]
	public float SlowSpeed = 0.2f;

	// Used for math and to store current speed.
	private float _speed;

	[Header("Elemental Variables")]
	[Tooltip("How long the feez effect will last")]
	public float FrozenDuration = 5f;

	[Tooltip("If the moving platform is under the frozen status effect")]
	public bool IsForzen = false;

	// used for moving the platforms. Lerp needs T (Time) variable.
	private float _time;

	// Used to multily max distance to get desired distance. (math is done in update)
	private float _multForLength = 0;

	// Start is called before the first frame update
	void Start()
	{
		// set the speed to defualt.
		_speed = NormalSpeed;
	}

	// Very important. A void returns nothing, that what it means and does. It tells anything calling the function "I dont return anything, so dont expect anything".
	void Update()
	{
		// get the percent value for the max distance given, Making sure that if the maxed extents is over max length, then defult to 1 otherwise get the percentage. (ternary operation)
		_multForLength = MaxExtents / (LengthOfOneSegment * Segments.Length) >= (LengthOfOneSegment * Segments.Length) ? 1f : MaxExtents / (LengthOfOneSegment * Segments.Length);
		// Fuck no, I will not make this into a if statement.

		// if the moving platform is looping, not pause, extending and the time is greater than 1.
		// Start the pause coroutine and set the bool extending to false.
		if (_time >= 1 && Loop && !IsPaused && Extending)
		{
			StartCoroutine(Pause());
			Extending = false;
		}
		// else if the moving platform is looping, not pause, not extending and the time is less than 1.
		// Start the pause coroutine and toggle the bool extending to true.
		else if (_time <= 0 && Loop && !IsPaused && !Extending)
		{
			StartCoroutine(Pause());
			Extending = true;
		}

		// if the game is paused. This should pause, but just in case. (delta time should be paused if time scale is set to 0)
		if (IsPaused) return;

		// if the time is less than 1 and is extending than increment the time.
		if (_time <= 1 && Extending)
		{
			_time += Time.deltaTime * (_speed / Segments.Length);
		}
		// else if the time is greater than 0 and is retracting then decrease the time.
		else if (_time >= 0 && !Extending)
		{
			_time -= Time.deltaTime * (_speed / Segments.Length);
		}

		// cycles throgh each segment in the array and moves the platform with vector 3 lerp. X and Z do not set positions, only Y.
		foreach (var segments in Segments)
		{
			// can fix incase the platform is not handled correctly. Change 0 to local pos y and add local pos y on the other sid.
			segments.localPosition = Vector3.Lerp(new Vector3(segments.localPosition.x, 0, segments.localPosition.z), new Vector3(segments.localPosition.x, LengthOfOneSegment * _multForLength, segments.localPosition.z), _time);
		}
	}

	// Coroutine to pause the platform over a set ammount of time.
	IEnumerator Pause()
	{
		IsPaused = true;
		yield return new WaitForSeconds(PauseTime);
		IsPaused = false;
	}

	// coroutine to slow the platform over a set ammount of time.
	IEnumerator Slow()
	{
		IsForzen = true;
		_speed = SlowSpeed;
		yield return new WaitForSeconds(FrozenDuration);
		_speed = NormalSpeed;
		IsForzen = false;
	}

	// * functions for extenal scriopts.

	// a function to freez the moving platform.
	public void Freez()
	{
		if (!IsForzen)
		{
			StartCoroutine(Slow());
		}
	}

	// a function to un freez the moving platform.
	public void UnFreez()
	{
		if (IsForzen)
		{
			StopCoroutine(Slow());
			_speed = NormalSpeed;
			IsForzen = false;
		}
	}

	// A function to toggle the extending state.
	public void ToggleExtending()
	{
		Extending = !Extending;
	}

	// A funtion to set the extending state.
	public void SetExtending(bool isExtending)
	{
		Extending = isExtending;
	}
}
