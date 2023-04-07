using UnityEngine;
using System.Linq;
public class multiLerp : MonoBehaviour
{
    [SerializeField] Transform[] _points;
    void Update()
    {
        transform.position = MultiLerp(Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime, _points.Select(x=> x.position).ToArray());
    }
    Vector3 MultiLerp(float time, Vector3[] points)
    {
        if (points.Length == 1)
            return points[0];
        else if (points.Length == 2)
            return Vector3.Lerp(points[0], points[1], time);

        if (time == 0)
            return points[0];

        if (time == 1)
            return points[points.Length - 1];

        float t = time * (points.Length - 1);

        Vector3 pointA = Vector3.zero;
        Vector3 pointB = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            if (t < (float)i)
            {
                pointA = points[i - 1];
                pointB = points[i];
                return Vector3.Lerp(pointA, pointB, t - (i - 1));
            }
            else if (t == (float)i)
            {
                return points[i];
            }
        }
        return Vector3.zero;
    }
}
