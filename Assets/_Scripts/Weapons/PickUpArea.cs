using UnityEngine;
public class PickUpArea : MonoBehaviour
{
    public bool playerClose { get; private set; }

    EquipableUI _equipableUI;
    private void Awake()
    {
        _equipableUI = FindObjectOfType<EquipableUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player) playerClose = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            playerClose = false;
            ShowUI();
        }
    }

    public void ShowUI(bool playerClose = false)
    {
        _equipableUI.SetPosition(transform.position + Vector3.up).SetActive(playerClose);
    }
}
