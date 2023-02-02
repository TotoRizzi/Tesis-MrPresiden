using UnityEngine;
using Weapons;

public class OnMouseOverWeapon : MonoBehaviour
{
    Weapon _myWeapon;
    PickUpArea _pickUpArea;
    SpriteRenderer _weaponSpriteRenderer;
    private void Start()
    {
        _myWeapon = GetComponentInParent<Weapon>();
        _pickUpArea = GetComponentInParent<PickUpArea>();
        _weaponSpriteRenderer = _myWeapon.GetComponent<SpriteRenderer>();
        _weaponSpriteRenderer.sprite = _myWeapon.GetWeaponData.mainSprite;
    }

    private void OnMouseEnter()
    {
        _weaponSpriteRenderer.sprite = _myWeapon.GetWeaponData.selectedSprite;
    }

    private void OnMouseOver()
    {
        if (_pickUpArea.playerClose) _pickUpArea.ShowUI(true);
    }

    private void OnMouseExit()
    {
        _weaponSpriteRenderer.sprite = _myWeapon.GetWeaponData.mainSprite;
        _pickUpArea.ShowUI();
    }
}
