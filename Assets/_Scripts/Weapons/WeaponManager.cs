using UnityEngine;
using Weapons;
public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon _currentWeapon;

    Camera _mainCamera;
    Transform _weaponContainer;
    System.Action _lookAtMouse = delegate { };
    private void Start()
    {
        _mainCamera = Camera.main;
        _weaponContainer = transform.GetChild(0);
    }
    private void Update()
    {
        _lookAtMouse?.Invoke();

        if (Input.GetMouseButton(0) && _currentWeapon)
        {
            _currentWeapon.Attack(GetMouseDirection());
            if (_currentWeapon.GetWeaponData.weaponType == WeaponType.Granade) DeactiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.G)) ThrowWeapon();

        if (Input.GetKeyDown(KeyCode.E)) SetWeapon();
    }
    void SetWeapon()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(_weaponContainer.position, GetMouseDirection().normalized, 2f, GameManager.instance.WeaponLayer);
        if (hit.Length <= 0) return;

        Weapon newWeapon = null;
        float minValue = 0;
        foreach (var weapon in hit)
        {
            float distance = Vector2.Distance(weapon.transform.position, GetMousePosition());

            if (minValue == 0)
            {
                minValue = distance;
                newWeapon = weapon.collider.GetComponent<Weapon>();
            }
            else if (distance < minValue)
            {
                minValue = distance;
                newWeapon = weapon.collider.GetComponent<Weapon>();
            }
        }

        if (_currentWeapon && newWeapon)
            ThrowWeapon();

        _currentWeapon = newWeapon;
        _currentWeapon.PickUp().SetParent(transform.GetChild(0)).SetPosition(transform.GetChild(0).position);
        _lookAtMouse = CurrentWeapon;
    }
    private void ThrowWeapon()
    {
        _currentWeapon?.ThrowOut(GetMouseDirection()).SetParent(null);
        DeactiveWeapon();
    }
    void DeactiveWeapon()
    {
        if (_currentWeapon)
        {
            _currentWeapon = null;
            _weaponContainer.eulerAngles = Vector3.zero;
            _lookAtMouse = delegate { };
        }
    }
    void CurrentWeapon()
    {
        _weaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());
        Vector2 weaponSize = new Vector2(_currentWeapon.transform.localScale.x, Mathf.Sign(GetMouseDirection().x));
        _currentWeapon.transform.localScale = weaponSize;
    }

    public float GetAngle() => Mathf.Atan2(GetMouseDirection().y, GetMouseDirection().x) * Mathf.Rad2Deg;
    Vector2 GetMousePosition() => _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    Vector2 GetMouseDirection() => (GetMousePosition() - (Vector2)_weaponContainer.position).normalized;
}
