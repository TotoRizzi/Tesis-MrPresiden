using UnityEngine;
public class GroundCheck : MonoBehaviour
{
    private bool _isGrounded;
    LayerMask _groundLayer;
    public bool IsGrounded { get { return _isGrounded; } private set { } }

    private void Start()
    {
        _groundLayer = LayerMask.GetMask("Border") + LayerMask.GetMask("Ground");
    }
    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(transform.position, .2f, _groundLayer);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, .2f);
    }
}
