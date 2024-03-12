// THIS SCRIPT SHOULD BE ATTACHED TO AN OBJECT IN THE RELEVANT SCENE: E.G. Main Camera.
// distant-explosion is the Track 1 audio clip and Punch-a-rock is the Track 2 audio clip.

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
        [Range(0f, 1000f)] public float pitch;
        [Range(1f, 100f)] public float volume;
    }

    [Header("<b>Shake Parameters (All in %)</b>")]
    [Space]
    [SerializeField][Range(0f, 1000f)] private float _shakeSeconds = 180f;
    [SerializeField][Range(0f, 1000f)] private float _shakeHorizontal = 80f;
    [SerializeField][Range(0f, 1000f)] private float _shakeVertical = 150f;

    [Header("<b>Audio Parameters (All in %)</b>")]
    [Space]
    [SerializeField] private AudioVariables[] _audioTracks =
    {
        new AudioVariables { name = "Track 1", audioClip = null, pitch = 300f, volume = 70f },
        new AudioVariables { name = "Track 2", audioClip = null, pitch = 150f, volume = 50f }
    };

    [Header("<b>Action Paramaters</b>")]
    [Space]
    public bool Shake = false;
    public bool PlayClips = false;


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
        else
        {
            Debug.Log("The Main Camera could not be found.");
        }
    }

    private void Update()
    {
        if (Shake)
        {
            Shake = false;
            StartCoroutine(ShakeCamera());
        }
        else if (PlayClips)
        {
            PlayClips = false;
            PlayAudioClips();
        }
    }

    // [Custom Methods]

    // ShakeCamera—saves the original position and varies x and y positions by a random offset every frame.
    private IEnumerator ShakeCamera()
    {
        Vector3 originalPos = _mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _shakeSeconds/100)
        {
            float x = Random.Range(-1f, 1f) * _shakeHorizontal / 100; // % (Percentage)
            float y = Random.Range(-1f, 1f) * _shakeVertical / 100; // % (Percentage)

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
            AudioSource audioSource = _mainCamera.gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.pitch = pitch/100; // % (Percentage)
            audioSource.volume = volume/100; // % (Percentage)
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