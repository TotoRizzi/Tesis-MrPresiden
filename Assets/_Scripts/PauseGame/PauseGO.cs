using System.Collections.Generic;
using UnityEngine;
public class PauseGO : IScreen
{
    Dictionary<Behaviour, bool> _before;
    Transform _root;
    List<Vector2> _rbForces = new List<Vector2>();
    public PauseGO(Transform root)
    {
        _before = new Dictionary<Behaviour, bool>();
        _root = root;
    }
    public void Activate()
    {
        var count = 0;
        foreach (var keyValue in _before)
        {
            keyValue.Key.enabled = keyValue.Value;
            var rb = keyValue.Key.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.WakeUp();
                rb.AddForce(_rbForces[count], ForceMode2D.Impulse);
                count++;
            }
        }
        _rbForces.Clear();
        _before.Clear();
    }
    public void Deactivate()
    {
        foreach (var b in _root.GetComponentsInChildren<Behaviour>())
        {
            _before[b] = b.enabled;
            var rb = b.GetComponent<Rigidbody2D>();
            if (rb)
            {
                _rbForces.Add(rb.velocity);
                rb.Sleep();
            }
            b.enabled = false;
        }
    }

    public void PauseObjectsInCinematic()
    {
        foreach (var b in _root.GetComponentsInChildren<Behaviour>())
        {
            if (b.gameObject.tag == "Cinematica") continue;
            _before[b] = b.enabled;
            var rb = b.GetComponent<Rigidbody2D>();
            if (rb) rb.Sleep();
            b.enabled = false;
        }
    }
    public void Free()
    {
        GameObject.Destroy(_root.gameObject);
    }
}
