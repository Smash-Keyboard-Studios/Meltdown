using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
// ! ===============================
using UnityEditor;

[CustomEditor(typeof(CheckpointManager)), CanEditMultipleObjects]
public class CheckpointManagerUI : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		CheckpointManager myScript = (CheckpointManager)target;

		if (GUILayout.Button("Restart")) myScript.ReStartAtCheckpoint();
	}
}
// ! ===============================
#endif

public class CheckpointManager : MonoBehaviour
{
	public static CheckpointManager Current;

	public bool PlayerCanGoBack = false;

	public Vector3 CheckpointPos;

	private int _currentCheckpointID = -1;

	public int CurrentCheckpointID { get => _currentCheckpointID; set => _currentCheckpointID = value; }

	private int buildIndex = -1;

	void Awake()
	{
		if (Current != null && Current != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Current = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		_currentCheckpointID = -1;
		buildIndex = SceneManager.GetActiveScene().buildIndex;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetCheckpointID(int checkpointID)
	{
		_currentCheckpointID = checkpointID;
	}


	public void ReStartAtCheckpoint()
	{
		ReloadScene();

		if (_currentCheckpointID == -1) return;

		StartCoroutine(WaitForLevelToLoad());
	}

	private void ReloadScene()
	{
		buildIndex = SceneManager.GetActiveScene().buildIndex;

		if (LevelLoading.Instance == null) SceneManager.LoadScene(buildIndex);
		else LevelLoading.Instance.LoadScene(buildIndex);
	}

	IEnumerator WaitForLevelToLoad()
	{
		if (LevelLoading.Instance != null)
		{
			while (LevelLoading.Instance.loading)
			{

				yield return null;
			}
		}

		while (PlayerFinder.Current == null)
		{

			yield return null;
		}

		while (PlayerFinder.Current.GetPlayerGO() == null)
		{

			yield return null;
		}

		while (PlayerFinder.Current.GetPlayerTransform().position == null)
		{

			yield return null;
		}

		bool whileNotInSpot = true;

		while (PlayerFinder.Current.GetPlayerTransform().position != CheckpointPos && whileNotInSpot)
		{
			PlayerFinder.Current.GetPlayerGO().GetComponent<CharacterController>().enabled = false;
			PlayerFinder.Current.GetPlayerTransform().position = CheckpointPos;
			PlayerFinder.Current.GetPlayerGO().GetComponent<CharacterController>().enabled = true;

			if (PlayerFinder.Current.GetPlayerTransform().position == CheckpointPos)
			{
				whileNotInSpot = false;
			}
			yield return null;
		}

		PauseMenu.Paused = false;
		print(Time.timeScale);

		yield return null;
	}
}
