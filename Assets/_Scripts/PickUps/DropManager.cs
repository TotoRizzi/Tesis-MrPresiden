using System.Collections.Generic;
using UnityEngine;
using PickUps;
public class DropManager : MonoBehaviour
{
    [Header("PickUps")]
    [SerializeField] PickUp[] _pickUpsCollection;
    Dictionary<PickUp, int> _pickUps = new Dictionary<PickUp, int>();
    void Start()
    {
        _pickUps.Add(_pickUpsCollection[0], 50);
    }
    public PickUp GetPickUpDrop() => RWS(_pickUps);
    T RWS<T>(Dictionary<T, int> values)
    {
        float sum = 0;
        foreach (var item in values)
            sum += item.Value;

        float random = Random.Range(0f, 1f);
        float count = 0;
        foreach (var item in values)
        {
            count += item.Value / sum;
            if (count >= random)
                return item.Key;
        }

        return default;
    }
}
