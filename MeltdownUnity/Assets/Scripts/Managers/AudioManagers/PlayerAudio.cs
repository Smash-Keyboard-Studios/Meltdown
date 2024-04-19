using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource PlayerAudioSource;
    public AudioClipLibrary[] AudioLibraryArray;

    public void PlayOneShotPlayerAudio(int i)
    {
        AudioClip PlayerClip = AudioLibraryArray[i].Clip;

        PlayerAudioSource.PlayOneShot(PlayerClip);
    }

    public void PlayPlayerAudio(int i)
    {
        AudioClip PlayerClip = AudioLibraryArray[i].Clip;

        PlayerAudioSource.clip = PlayerClip;
        PlayerAudioSource.Play();
    }

    public void StopPlayerAudio(int i)
    {
        PlayerAudioSource.Stop();
    }

    [System.Serializable]
    public class AudioClipLibrary
    {
        public string Name;
        public AudioClip Clip;
    }
}
