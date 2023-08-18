using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Node : MonoBehaviour
{
    public List<Node> neighbors = new List<Node>();
    public int cost = 1;

    [SerializeField] float _nodeRadius;
    private void Start()
    {
        var wallLayer = LayerMask.GetMask("Border");
        var nodeLayer = LayerMask.GetMask("Node");
        Collider2D[] nodesNeighbors = Physics2D.OverlapCircleAll(transform.position, _nodeRadius, nodeLayer);
        foreach (var item in nodesNeighbors)
        {
            var node = item.GetComponent<Node>();
            if (item == null) break;
            if (node != this)
            {
                if (IsBlocked(item.transform.position, .8f, wallLayer)) Destroy(item.gameObject);
                else if (InSight(transform.position, item.transform.position, wallLayer)) continue;
                else neighbors.Add(node);
            }
        }

        if (!neighbors.Any()) Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _nodeRadius);
    }
    bool InSight(Vector3 start, Vector3 end, LayerMask mask)
    {
        return Physics2D.Raycast(start, end - start, Vector3.Distance(start, end), mask);
    }
    bool IsBlocked(Vector3 start, float radius, LayerMask mask)
    {
        return Physics2D.OverlapCircle(start, radius, mask);
    }
}
