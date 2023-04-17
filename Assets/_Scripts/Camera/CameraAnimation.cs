using System.Collections;
using UnityEngine;
public class CameraAnimation : MonoBehaviour
{
    [SerializeField] Vector3 _defeatDir;
    [SerializeField] Vector3 _victoryDir;

    Vector3 _initialPos;
    private void OnEnable()
    {
        _initialPos = transform.position;

        StartCoroutine(Lerp());
    }
    private void OnDisable()
    {
        StopCoroutine(Lerp());
    }
   IEnumerator Lerp()
    {
        float timer = 0;
        while(timer <= 1f)
        {
            timer += Time.deltaTime;
            transform.position = Helpers.LevelTimerManager.Timer <= Helpers.LevelTimerManager.LevelMaxTime ? Vector3.Lerp(_initialPos, _victoryDir, timer / 1f) :
                                                                                                             Vector3.Lerp(_initialPos, _defeatDir, timer / 1f);
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(10, 5, timer / 1f);
            yield return null;
        }
    }
}
