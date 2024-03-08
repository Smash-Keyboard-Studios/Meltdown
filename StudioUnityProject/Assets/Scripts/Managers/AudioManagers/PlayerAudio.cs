using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource PlayerAudioSource;
    public AudioClipLibrary[] AudioLibraryArray;

    public void PlayPlayerAudio(int i)
    {
        AudioClip PlayerClip = AudioLibraryArray[i].Clip;

        PlayerAudioSource.PlayOneShot(PlayerClip);
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioClip Clip;
    }
}
