using System.Collections;
using UnityEngine;
public class Cannon : MonoBehaviour
{
    [SerializeField] Transform _fuseStart, _finishStart, _sparks;

    LineRenderer _lineRenderer;
    Vector3 _offset = new Vector3(-.15f, 0);
    Vector3 _initialPos;
    Vector3 _targetPos;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _fuseStart.position);
        _lineRenderer.SetPosition(1, _finishStart.position);
        _initialPos = _lineRenderer.GetPosition(0);
        _targetPos = _lineRenderer.GetPosition(1);
        StartCoroutine(Mecha());
    }
    IEnumerator Mecha()
    {
        while (Helpers.LevelTimerManager.Timer <= Helpers.LevelTimerManager.LevelMaxTime)
        {
            _fuseStart.position = Vector3.Lerp(_initialPos, _targetPos, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
            _lineRenderer.SetPosition(0, _fuseStart.position);
            _sparks.position = _fuseStart.position + _offset;
            yield return null;
        }
        _sparks.gameObject.SetActive(false);
        _lineRenderer.positionCount = 0;
    }
}
