using System;
using UnityEngine;
using Weapons;
public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon _currentMainWeapon;
    [SerializeField] Weapon _currentSecundaryWeapon;

    Camera _mainCamera;
    Transform _mainWeaponContainer;
    Transform _secundaryWeaponContainer;
    System.Action _lookAtMouse = delegate { };

    public Transform SecundaryWeaponContainer { get { return _secundaryWeaponContainer; } }
    private void Start()
    {
        _mainCamera = Camera.main;
        _mainWeaponContainer = transform.GetChild(0);
        _secundaryWeaponContainer = transform.GetChild(1);
    }
    private void Update()
    {
        _lookAtMouse?.Invoke();

        if (Input.GetMouseButton(0) && _currentMainWeapon) _currentMainWeapon.Attack(GetMouseDirectionMain());

        if (Input.GetMouseButton(1) && _currentSecundaryWeapon) _currentSecundaryWeapon.Attack(GetMouseDirectionSecundary());

        if (Input.GetKeyDown(KeyCode.G)) ThrowWeapon(ref _currentMainWeapon, _mainWeaponContainer, MainWeapon);

        if (Input.GetKeyDown(KeyCode.E)) SetWeapon();
    }

    #region Weapon Funcs
    void SetWeapon()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(_mainWeaponContainer.position, GetMouseDirectionMain().normalized, 2f, GameManager.instance.WeaponLayer);
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

        if (!newWeapon.CanPickUp) return;

        if (newWeapon.GetWeaponData.weaponType == WeaponType.MainWeapon)
        {
            if (_currentMainWeapon)
                ThrowWeapon(ref _currentMainWeapon, _mainWeaponContainer, MainWeapon);

            EquipWeapon(ref _currentMainWeapon, newWeapon, _mainWeaponContainer, MainWeapon);
            _currentMainWeapon.UpdateCurrentWeapon();
            _currentMainWeapon.GetComponent<FireWeapon>().UpdateAmmoAmount();
        }
        else
        {
            if (_currentSecundaryWeapon)
                ThrowWeapon(ref _currentSecundaryWeapon, _secundaryWeaponContainer, SecundaryWeapon);

            EquipWeapon(ref _currentSecundaryWeapon, newWeapon, _secundaryWeaponContainer, SecundaryWeapon);
        }
    }
    void EquipWeapon(ref Weapon weapon, Weapon newWeapon, Transform WeaponContainer, Action weaponAction)
    {
        weapon = newWeapon;
        _lookAtMouse += weaponAction;
        newWeapon.PickUp().SetParent(WeaponContainer).SetPosition(WeaponContainer.position);
    }
    private void ThrowWeapon(ref Weapon weapon, Transform WeaponContainer, Action weaponAction)
    {
        weapon?.ThrowOut(GetMouseDirectionMain()).SetParent(null);
        weapon = null;
        WeaponContainer.eulerAngles = Vector2.zero;
        _lookAtMouse -= weaponAction;
    }
    void MainWeapon()
    {
        _mainWeaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());
        Vector2 weaponSize = new Vector2(_currentMainWeapon.transform.localScale.x, Mathf.Sign(GetMouseDirectionMain().x));
        _currentMainWeapon.transform.localScale = weaponSize;
    }
    void SecundaryWeapon()
    {
        _secundaryWeaponContainer.eulerAngles = new Vector3(0, 0, GetAngle());
        Vector2 weaponSize = new Vector2(_currentSecundaryWeapon.transform.localScale.x, Mathf.Sign(GetMouseDirectionSecundary().x));
        _currentSecundaryWeapon.transform.localScale = weaponSize;
    }

    #endregion

    #region Mouse Funcs
    public float GetAngle() => Mathf.Atan2(GetMouseDirectionMain().y, GetMouseDirectionMain().x) * Mathf.Rad2Deg;
    Vector2 GetMousePosition() => _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    Vector2 GetMouseDirectionMain() => (GetMousePosition() - (Vector2)_mainWeaponContainer.position).normalized;
    Vector2 GetMouseDirectionSecundary() => (GetMousePosition() - (Vector2)_secundaryWeaponContainer.position).normalized;

    #endregion
}
