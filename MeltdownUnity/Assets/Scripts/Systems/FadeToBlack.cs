using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{

	private bool _fadingOut = false;

	private bool _fading = false;

	private float _localTime = 0;

	public float Speed = 0.5f;

	public Image BlackScreen;

	public static FadeToBlack Current;

	void Awake()
	{
		if (Current != null && Current != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Current = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		FadeOut(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (_fading && _fadingOut && _localTime <= 1)
		{
			_localTime += Time.deltaTime * Speed;
		}
		else if (_fading && !_fadingOut && _localTime >= 0)
		{
			_localTime -= Time.deltaTime * Speed;
		}
		else if (_fading)
		{
			_fading = false;
			if (!_fadingOut) BlackScreen.gameObject.SetActive(false);
		}

		BlackScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, _localTime));
	}

	public void FadeOut(bool fadingOut = true)
	{
		if (fadingOut) _localTime = 0;
		else _localTime = 1;

		_fadingOut = fadingOut;

		_fading = true;

		BlackScreen.gameObject.SetActive(true);
	}
}
