using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootWarning : MonoBehaviour
{
    private LineRenderer _line;

    private bool _isLerping;
    private Vector3 _startingScale;
    private float _currentTime;
    public float LETSGONG_DURATION;
    // Start is called before the first frame update
    void Start()
    {
        _line= GetComponent<LineRenderer>();
        DrawCircle(0.15f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LETSGONGGGGG()
    {
        if (_isLerping)
            yield break;

        _isLerping = true;
        _startingScale = transform.localScale;
        _currentTime = 0;

        while (true)
        {
            transform.localScale = Vector3.Lerp(_startingScale, Vector3.one, _currentTime / LETSGONG_DURATION);

            _currentTime += Time.deltaTime;

            if (_currentTime >= LETSGONG_DURATION)
                break;

            yield return null;
        }

        _isLerping = false;
        yield break;
    }

    public void DrawCircle(float radius, float lineWidth)
    {
        var segments = 360;
        _line.useWorldSpace = false;
        _line.startWidth = lineWidth;
        _line.endWidth = lineWidth;
        _line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0.05f, Mathf.Cos(rad) * radius);
        }

        _line.SetPositions(points);
    }
}
