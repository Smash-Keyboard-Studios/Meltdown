using UnityEngine;

public class EnvironmentAudio : MonoBehaviour
{
    public AudioClipLibrary[] AudioLibraryArray;
    public bool loopaudio = false;

    public void PlayEnvAudio(int i, AudioSource source = null)
    {
        AudioClip EnvClip = AudioLibraryArray[i].Clip;
        AudioSource envsource = null;
        AudioReverbZone reverb = AudioLibraryArray[i].Reverb;

        if (source == null)
        {
            envsource = AudioLibraryArray[i].AudioSource;
        }
        else
        {
            envsource = source;
        }

        if (loopaudio == false)
        {
            envsource.PlayOneShot(EnvClip);
        }
        else
        {
            envsource.loop = true;
            envsource.clip = EnvClip;
            envsource.Play();
        }
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioSource AudioSource;
        public AudioReverbZone Reverb;
        public AudioClip Clip;
    }
}
