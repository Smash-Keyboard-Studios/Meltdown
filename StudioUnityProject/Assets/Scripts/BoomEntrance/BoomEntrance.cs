// THIS SCRIPT SHOULD BE ATTACHED TO THE MAIN CAMERA.
// distant-explosion should be audio clip 1 and Punch-a-rock should be audio clip 2.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEntrance : MonoBehaviour
{

    // [Non-Editor Variables]

    private Camera _mainCamera;

    // [Editor Variables]

    // Audio Template

    [System.Serializable]
    public struct AudioVariables
    {
        public string name;
        public AudioClip audioClip;
        public float pitch;
        public float volume;
    }

    [Header("<b>Shake Parameters (%)</b>")]
    [Space]
    [SerializeField] private float _shakeDuration = 180f;
    [SerializeField] private float _shakeHorizontal = 80f;
    [SerializeField] private float _shakeVertical = 150f;

    [Header("<b>Audio Parameters (%)</b>")]
    [Space]
    [SerializeField] private AudioVariables[] _audioTracks =
    {
        new AudioVariables { name = "Track 1", audioClip = null, pitch = 300f, volume = 70f },
        new AudioVariables { name = "Track 2", audioClip = null, pitch = 150f, volume = 50f }
    };


    // [Events]

    private void Start()
    {
        // Selects the main camera.
        _mainCamera = Camera.main;

        if (_mainCamera != null)
        {
            StartCoroutine(ShakeCamera());
            PlayAudioClips();
        }
    }

    // [Custom Methods]

    // ShakeCamera—saves the original position and varies x and y positions by a random offset every frame.

    private IEnumerator ShakeCamera()
    {
        Vector3 originalPos = _mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _shakeDuration/100)
        {
            float x = Random.Range(-1f, 1f) * _shakeHorizontal/100;
            float y = Random.Range(-1f, 1f) * _shakeVertical/100;

            _mainCamera.transform.position = originalPos + new Vector3(x, y, originalPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _mainCamera.transform.position = originalPos;
    }

    // Adds a temporary audio source to the camera, and plays the clips at a certain speed (pitch) and volume.

    private void PlayAudioClip(AudioClip audioClip, float pitch, float volume)
    {
        if (audioClip != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.pitch = pitch/100;
            audioSource.volume = volume/100;
            audioSource.Play();
            Destroy(audioSource, audioClip.length);
        }
    }

    private void PlayAudioClips()
    {
        foreach (var audioVar in _audioTracks)
        {
            PlayAudioClip(audioVar.audioClip, audioVar.pitch, audioVar.volume);
        }
    }
}