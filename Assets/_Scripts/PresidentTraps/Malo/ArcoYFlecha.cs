using System.Collections.Generic;
using UnityEngine;
public class ArcoYFlecha : MonoBehaviour
{
    [SerializeField] GameObject _arrowGO;
    [SerializeField] Transform[] _targets;
    [SerializeField] Transform _arrowParent;
    [SerializeField] float _arrowSpeed;

    int _counter;
    Dictionary<GameObject, Transform> _arrows = new Dictionary<GameObject, Transform>();
    private void Update()
    {
        if (_arrows.Count <= 0) return;

        foreach (var item in _arrows)
        {
            Vector3 distance = item.Value.position - item.Key.transform.position;
            item.Key.transform.right = distance;
            item.Key.transform.position += distance.normalized * _arrowSpeed * Time.deltaTime;
            if (distance.magnitude <= .1f)
            {
                item.Key.transform.SetParent(item.Value);
                _arrows.Remove(item.Key);
                continue;
            }
        }
    }
    public void Shoot()
    {
       var arrow = Instantiate(_arrowGO, _arrowParent.position, _arrowParent.rotation);
        arrow.transform.SetParent(GameObject.Find("MainGame").transform);
        _arrows[arrow] = _targets[_counter++ % _targets.Length];
    }
}
