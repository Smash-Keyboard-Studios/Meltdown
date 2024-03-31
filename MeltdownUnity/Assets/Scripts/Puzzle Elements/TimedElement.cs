using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedElement : MonoBehaviour
{
	[Header("Events")]
	public UnityEvent OnStart;
	public UnityEvent OnEnd;

	[Header("Duration in seconds")]
	public float Duration;

	[Header("One time use")]
	public bool OneTimeUse = false;

	private float _time;
	private bool Started = false;



	void Update()
	{
		if (Started && _time < Duration)
		{
			_time += Time.deltaTime;
		}
		else if (Started && _time >= Duration)
		{
			_time = 0;
			Started = false;

			OnEnd.Invoke();
		}
	}

	public void StartTimer()
	{
		Started = true;
		OnStart.Invoke();
	}
}
