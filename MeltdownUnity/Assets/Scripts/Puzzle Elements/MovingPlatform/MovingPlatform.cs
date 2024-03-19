using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
	public Transform[] Segments;


	public float LengthOfOneSegment;

	[Range(0, 1)] public float MaxExtents = 1f;

	public bool Extending = false;

	public bool Loop = false;

	public bool IsPaused = false;

	public float PauseTime = 1f;

	private float Speed;

	public float NormalSpeed = 1f;
	public float SlowSpeed = 0.2f;

	public float FrozenDuration = 5f;

	public bool IsForzen = false;

	private float _time;


	// Start is called before the first frame update
	void Start()
	{
		Speed = NormalSpeed;
	}

	// Update is called once per frame
	void Update()
	{


		if (_time >= 1 && Loop && !IsPaused && Extending)
		{
			StartCoroutine(Pause());
			Extending = false;
		}
		else if (_time <= 0 && Loop && !IsPaused && !Extending)
		{
			StartCoroutine(Pause());
			Extending = true;
		}

		if (IsPaused) return;

		if (_time <= 1 && Extending)
		{
			_time += Time.deltaTime * (Speed / Segments.Length);
		}
		else if (_time >= 0 && !Extending)
		{
			_time -= Time.deltaTime * (Speed / Segments.Length);
		}

		foreach (var segments in Segments)
		{
			segments.localPosition = Vector3.Lerp(new Vector3(segments.localPosition.x, 0, segments.localPosition.z), new Vector3(segments.localPosition.x, LengthOfOneSegment * MaxExtents, segments.localPosition.z), _time);
		}
	}

	IEnumerator Pause()
	{
		IsPaused = true;
		yield return new WaitForSeconds(PauseTime);
		IsPaused = false;
	}

	IEnumerator Slow()
	{
		IsForzen = true;
		Speed = SlowSpeed;
		yield return new WaitForSeconds(FrozenDuration);
		Speed = NormalSpeed;
		IsForzen = false;
	}

	public void Freez()
	{
		if (!IsForzen)
		{
			StartCoroutine(Slow());
		}
	}

	public void UnFreez()
	{
		if (IsForzen)
		{
			StopCoroutine(Slow());
			Speed = NormalSpeed;
			IsForzen = false;
		}
	}

	public void ToggleExtending()
	{
		Extending = !Extending;
	}

	public void SetExtending(bool isExtending)
	{
		Extending = isExtending;
	}
}
