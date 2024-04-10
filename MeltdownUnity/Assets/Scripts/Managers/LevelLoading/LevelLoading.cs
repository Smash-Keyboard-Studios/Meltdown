using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This is how levels are loaded with the splash screen.
/// You just need to callt the loadScene func to load the scene you want.
/// </summary>
public class LevelLoading : MonoBehaviour
{
	// Instance so other scripts can call funtions.
	public static LevelLoading Instance;

	// The loading screen UI.
	public GameObject LoadingScreen;
	public Slider ProgressBar;

	// used to stop loading the level multiple times when reloading is called more than once when loading.
	private bool isReloading = false;

	// if the level is being loaded.
	public bool loading = false;

	// this prevents loading when enabled.
	public bool overideAll = false;

	// progress of loading the scene.
	private float totalSceneProgress;

	// This is used to keep track of levels being loaded.
	List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

	// sets the instance
	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
			// prevents this of being destroyed on load.
			DontDestroyOnLoad(this.gameObject);
		}
	}

	// sets variables.
	private void Start()
	{
		if (overideAll) return;

		LoadingScreen.SetActive(false);

		LoadMainMenu();

		// SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

	}


	private void Update()
	{
		if (overideAll) return;

		// stops reloading of reloading multiple times.
		isReloading = LoadingScreen.gameObject.activeSelf;
	}

	/// <summary>
	/// A functiuon to load scene with index of 1.
	/// </summary>
	public void LoadMainMenu()
	{
		LoadScene(1);
	}

	/// <summary>
	/// Loads the scene with the given index async.
	/// </summary>
	/// <param name="indexNumber">build scene index</param>
	public void LoadScene(int indexNumber)
	{
		if (overideAll) return;

		loading = true;
		LoadingScreen.gameObject.SetActive(true);
		SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));

		if (SceneManager.sceneCount > 1)
		{
			scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1)));
		}

		scenesLoading.Add(SceneManager.LoadSceneAsync(indexNumber, LoadSceneMode.Additive));

		StartCoroutine(GetSceneLoadProgress());
	}

	/// <summary>
	/// Loads the scene with the give name async.
	/// </summary>
	/// <param name="mapName">build scene name</param>
	public void LoadScene(string mapName)
	{
		if (overideAll) return;

		loading = true;
		LoadingScreen.gameObject.SetActive(true);
		SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));

		if (SceneManager.sceneCount > 1)
		{
			scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1)));
		}

		scenesLoading.Add(SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive));

		StartCoroutine(GetSceneLoadProgress());
	}

	/// <summary>
	/// Used to reload the current scene loaded.
	/// </summary>
	public void Reload()
	{
		if (overideAll) return;

		loading = true;
		if (isReloading) return;
		SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));

		LoadingScreen.gameObject.SetActive(true);

		Scene save = SceneManager.GetSceneAt(1);

		scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1)));
		scenesLoading.Add(SceneManager.LoadSceneAsync(save.buildIndex, LoadSceneMode.Additive));

		StartCoroutine(GetSceneLoadProgress());
	}

	/// <summary>
	/// Used to keep track of loading.
	/// </summary>
	/// <returns></returns>
	public IEnumerator GetSceneLoadProgress()
	{
		for (int i = 0; i < scenesLoading.Count; i++)
		{
			while (!scenesLoading[i].isDone)
			{
				totalSceneProgress = 0;

				foreach (AsyncOperation operation in scenesLoading)
				{
					totalSceneProgress += operation.progress;
				}

				totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

				ProgressBar.value = totalSceneProgress;

				yield return null;
			}
		}

		loading = false;
		LoadingScreen.gameObject.SetActive(false);

		if (SceneManager.sceneCount > 1)
		{
			SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
		}
		else
		{
			SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
		}
	}
}
