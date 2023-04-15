using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockFeedback : MonoBehaviour
{
    public ClockFeedback SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    private void Reset()
    {
    }

    public static void TurnOn(ClockFeedback b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(ClockFeedback b)
    {
        b.gameObject.SetActive(false);
    }
    public virtual void ReturnObject()
    {
        FRY_ClockFeedback.Instance.ReturnObject(this);
    }
}
