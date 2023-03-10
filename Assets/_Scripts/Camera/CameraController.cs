using System.Collections;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    GameManager _gameManager;
    Player _player;

    [SerializeField] AnimationCurve _curve;
    [SerializeField] float[] _clampX = new float[2];
    [SerializeField] float[] _clampY = new float[2];
    [SerializeField] float _smooth;
    private void Start()
    {
        _gameManager = GameManager.instance;
        _player = _gameManager.Player;
        _gameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(Shaking());
    }
    private void LateUpdate()
    {
        float xClamp = Mathf.Clamp(_player.transform.position.x, _clampX[0], _clampX[1]);
        float yClamp = Mathf.Clamp(_player.transform.position.y, _clampY[0], _clampY[1]);
        Vector3 targetPosition = new Vector3(xClamp, yClamp, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smooth * Time.deltaTime);
    }
    IEnumerator Shaking()
    {
        Debug.Log("Shaking");

        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < _gameManager.CameraShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strenght = _curve.Evaluate(elapsedTime / _gameManager.CameraShakeDuration);
            transform.localPosition = startPosition + Random.insideUnitSphere * strenght;
            yield return null;
        }

        transform.localPosition = startPosition;
    }
}
