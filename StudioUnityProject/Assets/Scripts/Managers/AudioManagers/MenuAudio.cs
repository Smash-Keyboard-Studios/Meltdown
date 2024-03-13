using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    public AudioSource MenuAudioSource;
    public AudioClipLibrary[] AudioLibraryArray;

    public void PlayMenuAudio(int i)
    {
        AudioClip MenuClip = AudioLibraryArray[i].Clip;

        MenuAudioSource.PlayOneShot(MenuClip);
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioClip Clip;
    }
}
