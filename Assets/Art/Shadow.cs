using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Shadow : MonoBehaviour
{
    ShadowCaster2D _shadowCaster;
    private void Start()
    {
        _shadowCaster = GetComponent<ShadowCaster2D>();
    }
    void ShadowToggle() => _shadowCaster.enabled = !_shadowCaster.enabled;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Light")) ShadowToggle();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Light")) ShadowToggle();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Light")) ShadowToggle();
    }
}
