using System.Collections;
using UnityEngine;
using System;
public class CameraController : MonoBehaviour
{
    GameManager _gameManager;
    Player _player;
    Action _cameraBehaviour;

    [SerializeField] AnimationCurve _curve;
    [SerializeField] float[] _clampX = new float[2];
    [SerializeField] float[] _clampY = new float[2];
    [SerializeField] float _smooth;
    [SerializeField] bool _static;
    private void Start()
    {
        _gameManager = GameManager.instance;
        _player = _gameManager.Player;
        _gameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(Shaking());

        _cameraBehaviour = _static ? delegate { } : (Action)CameraClamped;
    }
    private void LateUpdate()
    {
        _cameraBehaviour();
    }
    void CameraClamped()
    {
        float xClamp = Mathf.Clamp(_player.transform.position.x, _clampX[0], _clampX[1]);
        float yClamp = Mathf.Clamp(_player.transform.position.y, _clampY[0], _clampY[1]);
        Vector3 targetPosition = new Vector3(xClamp, yClamp, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smooth * Time.deltaTime);
    }
    public void StartCameraShake(float duration) => StartCoroutine(CameraShake(duration));
    public void StartShaking() => StartCoroutine(Shaking());

    IEnumerator CameraShake(float duration)
    {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * .015f;
            float y = UnityEngine.Random.Range(-1f, 1f) * .015f;

            transform.localPosition = new Vector3(startPosition.x + x, startPosition.y + y, startPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = startPosition;
    }
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < _gameManager.CameraShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strenght = _curve.Evaluate(elapsedTime / _gameManager.CameraShakeDuration);
            transform.localPosition = startPosition + UnityEngine.Random.insideUnitSphere * strenght;
            yield return null;
        }

        transform.localPosition = startPosition;
    }
}
