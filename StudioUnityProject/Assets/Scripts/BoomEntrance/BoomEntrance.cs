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

    [Header("<b>Shake Parameters (%)</b>")]
    [Space]
    [SerializeField] private float _shakeDuration = 180f;
    [SerializeField] private float _shakeHorizontal = 80f;
    [SerializeField] private float _shakeVertical = 150f;

    [Header("<b>Audio Parameters (%)</b>")]
    [Space]
    [SerializeField] private AudioClip _audioClip1;
    [SerializeField] private float _pitch1 = 300f;
    [SerializeField] private float _volume1 = 70f;
    [SerializeField] private AudioClip _audioClip2;
    [SerializeField] private float _pitch2 = 150f;
    [SerializeField] private float _volume2 = 50f;

    // [Events]

    private void Start()
    {
        // Automatically finds the main camera.
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
        PlayAudioClip(_audioClip1, _pitch1, _volume1);
        PlayAudioClip(_audioClip2, _pitch2, _volume2);
    }
}