using UnityEngine;

public class SFXAudio : MonoBehaviour
{
    public AudioClipLibrary[] AudioLibraryArray;

    public void PlaySFXAudio(int i, AudioSource source = null)
    {
        AudioClip SFXClip = AudioLibraryArray[i].Clip;
        AudioSource sfxsource = null;

        if (source == null)
        {
            sfxsource = AudioLibraryArray[i].AudioSource;
        }
        else
        {
            sfxsource = source;
        }

        sfxsource.PlayOneShot(SFXClip);
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioSource AudioSource;
        public AudioClip Clip;
    }
}
