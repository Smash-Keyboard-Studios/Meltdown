using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
	public bool IsEnabled = true;

	public bool CanUseCollisions = false;

	private void OnTriggerEnter(Collider collision)
	{

		if (collision.transform.CompareTag("Player") && IsEnabled)
		{
			if (CheckpointManager.Current == null)
				ReloadScene();
			else
				CheckpointManager.Current.ReStartAtCheckpoint();
		}
	}

	private void OnCollisionEnter(Collision other)
	{

		if (other.transform.CompareTag("Player") && IsEnabled && CanUseCollisions)
		{
			if (CheckpointManager.Current == null)
				ReloadScene();
			else
				CheckpointManager.Current.ReStartAtCheckpoint();
		}
	}

	private void ReloadScene()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		if (LevelLoading.Instance == null) SceneManager.LoadScene(currentSceneIndex);
		else LevelLoading.Instance.LoadScene(currentSceneIndex);
	}
}
