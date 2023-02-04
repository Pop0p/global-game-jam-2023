using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Root : MonoBehaviour
{


    private float _growthDuration;
    private float _currentTime;
    private Vector3 _targetPosition;
    private Vector3 _startPosition;
    private bool _isLerping;

    public bool IsPaused = true;
    public float ApparitionTime = 0;

    private bool _hasLerped = false;
    private float _time;


    private void OnEnable()
    {
        IsPaused = true;
        var xzscale = Random.Range(0.20f, 0.80f);
        transform.localScale = new Vector3(xzscale, Random.Range(1f, 3f), xzscale);
        _growthDuration = Random.Range(0.2f, .55f);
    }

    void Update()
    {
        if (!_hasLerped && _time >= ApparitionTime)
        {
            StartCoroutine(DoGrow());
            _hasLerped = true;
        }

        _time += Time.deltaTime;
    }


    IEnumerator DoGrow()
    {
        if (_isLerping)
            yield return null;

        _isLerping = true;
        _startPosition = transform.position;
        _targetPosition = new Vector3(transform.position.x, transform.localScale.y, transform.position.z);
        Camera.main.GetComponent<CameraShake>().ShakeTo(.01f, _growthDuration);

        while (true)
        {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _currentTime / _growthDuration);
            _currentTime += Time.deltaTime;

            if (_currentTime >= _growthDuration)
                break;

            yield return null;
        }

        _isLerping = false;
        yield return null; ;
    }
}
