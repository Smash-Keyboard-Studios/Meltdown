using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
