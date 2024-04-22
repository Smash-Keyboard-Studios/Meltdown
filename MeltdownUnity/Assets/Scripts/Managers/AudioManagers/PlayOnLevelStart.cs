using UnityEngine;

public class PlayOnLevelStart : MonoBehaviour
{
    public EnvironmentAudio EnvAudio;
    void Start()
    {
        EnvAudio.loopaudio = true;
        for (int i = 0; i < EnvAudio.AudioLibraryArray.Length; i++)
        {
            EnvAudio.PlayEnvAudio(i);
        }
    }
}
