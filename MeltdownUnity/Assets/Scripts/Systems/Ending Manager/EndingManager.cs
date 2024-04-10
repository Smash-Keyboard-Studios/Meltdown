using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
	public float DelayTime = 1f;

	public float Speed = 0.1f;

	private float _localTime = 0;

	private bool _running = false;

	private bool _enabled = false;

	public GameObject UI;

	public Image imageBG;

	public RectTransform creditsPage;

	public Vector2 StartPos;
	public Vector2 EndPos;

	// Start is called before the first frame update
	void Start()
	{
		// useless because unity UI is on Late Update. Grrrrr
		// size = creditsPage.sizeDelta;
		// creditsPage.localPosition = new Vector3(creditsPage.localPosition.x, -size.y, 0);


		UI.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (LevelLoading.Instance != null)
			{
				LevelLoading.Instance.LoadScene(1);
			}
			else
			{
				SceneManager.LoadScene(1);
			}
		}

		if (_enabled)
			_localTime += Time.deltaTime * Speed;
		else if (_running)
			_localTime += Time.deltaTime * 0.23f;
		else
			_localTime += Time.deltaTime;



		if (_localTime >= DelayTime && !_enabled)
		{
			if (_localTime >= DelayTime + 1)
			{
				_enabled = true;
				_localTime = 0f;
			}
			else
			{
				_running = true;
				UI.SetActive(true);
				imageBG.color = new Color(0, 0, 0, Mathf.Lerp(-0.1f, 1, _localTime - DelayTime));
			}
		}

		if (_enabled && _running)
		{

			creditsPage.localPosition = new Vector3(creditsPage.localPosition.x, Mathf.Lerp(StartPos.y, EndPos.y, _localTime), 0);
		}

		if (_enabled && _running && _localTime >= 1.1)
		{
			if (LevelLoading.Instance != null)
			{
				LevelLoading.Instance.LoadScene(1);
			}
			else
			{
				SceneManager.LoadScene(1);
			}
		}
	}
}
