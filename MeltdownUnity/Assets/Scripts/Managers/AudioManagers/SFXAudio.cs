using UnityEngine;

public class SFXAudio : MonoBehaviour
{
    public AudioClipLibrary[] AudioLibraryArray;
    public bool loopaudio = false;

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

        if(loopaudio == false)
        {
            sfxsource.PlayOneShot(SFXClip);
        }
        else
        {
            sfxsource.loop = true;
            sfxsource.Play();
        }
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioSource AudioSource;
        public AudioClip Clip;
    }
}
