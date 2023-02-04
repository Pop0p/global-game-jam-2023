using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0f;

    private bool _isLerping;
    private float _startingAmount;
    private float _currentTime;

    Vector3 originalPos;

    public void SetShake(float amount, float duration)
    {
        shakeAmount = amount;
        shakeDuration = duration;
    }

    public void ShakeTo(float amount, float duration)
    {
        IEnumerator Do()
        {
            if (_isLerping)
                yield return null;

            _isLerping = true;
            _startingAmount = amount;
            _currentTime = 0;

            while (true)
            {
                shakeAmount = Mathf.Lerp(_startingAmount, amount, _currentTime / duration);
                shakeDuration = duration;
                _currentTime += Time.deltaTime;

                if (_currentTime >= duration)
                    break;

                yield return null;
            }
            _isLerping = false;
            yield return null;
        }

        StartCoroutine(Do());
    }


    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
        }
    }

}
