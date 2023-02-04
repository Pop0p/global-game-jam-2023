using System.Collections;
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

    private bool _hasGrown = false;
    private float _time;
    private float _shrinkTryTime;


    private void OnEnable()
    {
        IsPaused = true;
        var xzscale = Random.Range(0.20f, 0.80f);
        transform.localScale = new Vector3(xzscale, Random.Range(1f, 3f), xzscale);
        _growthDuration = Random.Range(1f, 2f);
    }

    void Update()
    {
        if (!_hasGrown && _time >= ApparitionTime)
        {
            StartCoroutine(DoGrow());
        }
        if (_shrinkTryTime >= 3)
        {
            _shrinkTryTime = 0;
            if (_hasGrown && Random.Range(0f, 1f) <= 0.15f && _time >= ApparitionTime)
                StartCoroutine(DoShrink());
        }

        _time += Time.deltaTime;
        _shrinkTryTime += Time.deltaTime;
    }


    IEnumerator DoGrow()
    {
        if (_isLerping)
            yield break;

        _isLerping = true;
        _startPosition = transform.position;
        _currentTime = 0;
        Camera.main.GetComponent<CameraShake>().ShakeTo(.01f, _growthDuration);
        _growthDuration *= 2;
        _targetPosition = new Vector3(transform.position.x, transform.localScale.y, transform.position.z);

        while (true)
        {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _currentTime / _growthDuration);
            _currentTime += Time.deltaTime;

            if (_currentTime >= _growthDuration)
                break;

            yield return null;
        }

        _hasGrown = true;
        _isLerping = false;
        yield break;
    }

    IEnumerator DoShrink()
    {
        if (_isLerping)
            yield break;

        _isLerping = true;
        var temp = _startPosition;
        _startPosition = transform.position;
        _currentTime = 0;
        _targetPosition = new Vector3(transform.position.x, temp.y, transform.position.z);
        Camera.main.GetComponent<CameraShake>().ShakeTo(.01f, _growthDuration);
        while (true)
        {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _currentTime / _growthDuration);
            _currentTime += Time.deltaTime;

            if (_currentTime >= _growthDuration)
            {
                break;
            }

            yield return null;
        }

        _isLerping = false;
        _hasGrown = false;
        gameObject.SetActive(false);
        yield break;
    }
}
