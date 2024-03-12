using UnityEngine;

public class SFXAudio : MonoBehaviour
{
    public AudioSource SFXAudioSource;
    public AudioClipLibrary[] AudioLibraryArray;

    public void PlaySFXAudio(int i)
    {
        AudioClip SFXClip = AudioLibraryArray[i].Clip;

        SFXAudioSource.PlayOneShot(SFXClip);
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioClip Clip;
    }
}
