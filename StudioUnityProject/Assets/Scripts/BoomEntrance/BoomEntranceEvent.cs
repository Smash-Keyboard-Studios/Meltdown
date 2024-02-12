using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEntrance : MonoBehaviour
{

    // [Non-Editor Variables]

    private Camera _mainCamera;

    // [Editor Variables]

    [Header("<b>Shake Parameters</b>")]
    [Space]
    [SerializeField] private float _shakeDuration = 3.0f;
    [SerializeField] private float _shakeHorizontal = 0.2f;
    [SerializeField] private float _shakeVertical = 0.8f;

    // [Events]

    private void Start()
    {
        // Automatically finds the main camera.
        _mainCamera = Camera.main;

        if (_mainCamera != null)
        {
            StartCoroutine(ShakeCamera());
        }
    }

    // [Custom Methods]

    // ShakeCamera—saves the original position and varies x and y positions by a random offset every frame.

    private IEnumerator ShakeCamera()
    {
        Vector3 originalPos = _mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * _shakeHorizontal;
            float y = Random.Range(-1f, 1f) * _shakeVertical;

            _mainCamera.transform.position = originalPos + new Vector3(x, y, originalPos.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _mainCamera.transform.position = originalPos;
    }
}