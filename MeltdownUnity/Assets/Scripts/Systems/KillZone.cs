using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
	public bool IsEnabled = true;

	private void OnTriggerEnter(Collider collision)
	{

		if (collision.transform.CompareTag("Player") && IsEnabled)
		{
			int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

			if (LevelLoading.Instance == null) SceneManager.LoadScene(currentSceneIndex);
			else LevelLoading.Instance.LoadScene(currentSceneIndex);
		}
	}
}
