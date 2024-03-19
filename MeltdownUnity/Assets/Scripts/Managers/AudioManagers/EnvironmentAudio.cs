using UnityEngine;

public class EnvironmentAudio : MonoBehaviour
{
    public AudioSource EnvAudioSource;
    public AudioClipLibrary[] AudioLibraryArray;

    public void PlayEnvAudio(int i)
    {
        AudioClip EnvClip = AudioLibraryArray[i].Clip;

        EnvAudioSource.PlayOneShot(EnvClip);
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioClip Clip;
    }
}
