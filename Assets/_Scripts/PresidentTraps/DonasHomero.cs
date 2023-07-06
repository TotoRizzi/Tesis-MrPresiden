using System.Collections;
using UnityEngine;
public class DonasHomero : MonoBehaviour
{
    [SerializeField] GameObject _palanca;
    [SerializeField] GameObject[] _donas;
    [SerializeField] InfiniteRotation[] _cintas;

    System.Action _state = delegate { };
    void Start()
    {
        for (int i = 0; i < _cintas.Length; i++) _cintas[i].enabled = false;

        Helpers.LevelTimerManager.OnLevelStart += () => StartCoroutine(DiabloAnimation());
    }
    void Update()
    {
        _state?.Invoke();
    }
    void Palanca(Vector3 currentRot, Vector3 goalRot, float time, bool go)
    {
        float timer = 0;
        while (timer <= time)
        {
            timer += Time.deltaTime;
            _palanca.transform.eulerAngles = Vector3.Lerp(currentRot, goalRot, timer / time);

            for (int i = 0; i < _cintas.Length; i++) _cintas[i].enabled = go;
            if (go)
                for (int i = 0; i < _donas.Length; i++) _donas[i].transform.position += -Vector3.right * Time.deltaTime;
        }
        _state = delegate { };
    }
    IEnumerator DiabloAnimation()
    {
        float maxTime = Helpers.LevelTimerManager.LevelMaxTime;
        float palancaTime = 1;
        float loopTime = 3;
        Vector3 palancaRotation = new Vector3(0, 0, -25f);
        Vector3 currentRotation = _palanca.transform.eulerAngles;
        while (Helpers.LevelTimerManager.Timer <= maxTime)
        {

            _state = () => Palanca(currentRotation, palancaRotation, 1f, true);

            yield return new WaitForSeconds(palancaTime);

            for (int i = 0; i < _cintas.Length; i++) _cintas[i].enabled = false;
            _state = () => Palanca(palancaRotation, currentRotation, .5f, false);

            yield return new WaitForSeconds(loopTime);
        }
    }
}
