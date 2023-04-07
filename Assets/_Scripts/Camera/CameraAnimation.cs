using UnityEngine;
public class CameraAnimation : MonoBehaviour
{
    [SerializeField] Vector3 _defeatDir;
    [SerializeField] Vector3 _victoryDir;
    private void Update()
    {
        transform.position = Helpers.LevelTimerManager.Timer <= Helpers.LevelTimerManager.LevelMaxTime ? Vector3.MoveTowards(transform.position, _victoryDir, Time.deltaTime / .11f) :
                                                                                                         Vector3.MoveTowards(transform.position, _defeatDir, Time.deltaTime / .11f);
    }
}
