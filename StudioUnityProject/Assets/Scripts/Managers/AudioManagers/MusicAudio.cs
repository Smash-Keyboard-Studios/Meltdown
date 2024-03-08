using UnityEngine;

public class MusicAudio : MonoBehaviour
{
    public AudioSource MusicAudioSource;
    public AudioClipLibrary[] AudioLibraryArray;

    public void PlayMusicAudio(int i)
    {
        AudioClip MusicClip = AudioLibraryArray[i].Clip;

        MusicAudioSource.PlayOneShot(MusicClip);
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioClip Clip;
    }
}
