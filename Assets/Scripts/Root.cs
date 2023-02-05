using System.Collections;
using System.Threading;
using UnityEngine;

public class Root : MonoBehaviour
{


    private float _growthDuration;
    private float _currentTime;
    private Vector3 _targetPosition;
    private Vector3 _startPosition;
    private float _targetRotation;
    private Quaternion _startRotation;
    private bool _isLerping;

    public bool IsPaused = true;
    public float ApparitionTime = 0;
    public float ShrinkTime = 1f;

    private float _timeSinceGrown = 0f;
    private bool _hasGrown = false;
    private float _time;


    public bool Alone;
    public bool SameTime;

    public Cell Associated_cell;

    private AudioSource _audioSource;


    public GameObject RootWarningParent;
    private RootWarning _rootWarning;

    private void Awake()
    {
        if (_rootWarning == null)
            _rootWarning = RootWarningParent.transform.Find("Growth").GetComponent<RootWarning>();
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        IsPaused = true;
        _timeSinceGrown = 0;
        _time = 0;
        _growthDuration = .25f;
        transform.rotation = Quaternion.identity;
        _rootWarning.transform.localScale = new Vector3(0.1f, 1, 0.1f);
        RootWarningParent.SetActive(true);

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

    public void Setup(Cell c, float delay, bool alone)
    {
        Associated_cell = c;
        ApparitionTime = delay;
        transform.position = c.Position;
        var pos = transform.GetComponent<Renderer>().bounds.center;
        pos.y = 0.05f;
        Alone = alone;
        RootWarningParent.transform.position = pos;
        _rootWarning.LETSGONG_DURATION = delay;
        StartCoroutine(_rootWarning.LETSGONGGGGG());
    }

    IEnumerator DoGrow()
    {
        if (_isLerping)
            yield break;

        if (Alone)
            _audioSource.Play();
        RootWarningParent.SetActive(false);
        _isLerping = true;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _targetRotation = Random.Range(0, 360f);
        _currentTime = 0;
        Camera.main.GetComponent<CameraShake>().ShakeTo(.01f, _growthDuration);
        _growthDuration *= 2;
        _targetPosition = new Vector3(transform.position.x, .5f, transform.position.z);

        while (true)
        {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _currentTime / _growthDuration);
            transform.rotation = Quaternion.Lerp(_startRotation, Quaternion.Euler(_startRotation.eulerAngles.x, _targetRotation, _startRotation.eulerAngles.z), _currentTime / _growthDuration);

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
