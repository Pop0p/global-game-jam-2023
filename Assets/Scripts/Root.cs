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
    public float ShrinkTime = 1f;

    private float _timeSinceGrown = 0f;
    private bool _hasGrown = false;
    private float _time;

    public Cell Associated_cell;

    private void OnEnable()
    {
        IsPaused = true;
        _timeSinceGrown = 0;
        _time = 0;
        _growthDuration = .25f;
    }

    void Update()
    {
        if (!_hasGrown && _time >= ApparitionTime)
        {
            StartCoroutine(DoGrow());
        }
        if (!_isLerping && _hasGrown && _timeSinceGrown >= ShrinkTime)
        {
            StartCoroutine(DoShrink());
        }

        _time += Time.deltaTime;
        if (_hasGrown)
            _timeSinceGrown += Time.deltaTime;
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
        Associated_cell.HasRoot = false;
        gameObject.SetActive(false);
        yield break;
    }
}
