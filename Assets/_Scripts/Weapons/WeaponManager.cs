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
    Action _lookAtMouse = delegate { };

    public Transform SecundaryWeaponContainer { get { return _secundaryWeaponContainer; } }
    private void Start()
    {
        _mainCamera = Camera.main;
        _mainWeaponContainer = transform.GetChild(0);
        _secundaryWeaponContainer = transform.GetChild(1);
        _currentSecundaryWeapon = _secundaryWeaponContainer.GetComponentInChildren<Weapon>();
        _currentSecundaryWeapon.PickUp();
        _lookAtMouse += SecundaryWeapon;

        LoadWeapon();

        PlayerPrefs.DeleteAll();
    }
    private void Update()
    {
        _lookAtMouse?.Invoke();

        if (Input.GetMouseButton(0) && _currentMainWeapon) _currentMainWeapon.Attack(GetMouseDirectionMain());

        if (Input.GetMouseButton(1) && _currentSecundaryWeapon) _currentSecundaryWeapon.Attack(GetMouseDirectionSecundary());

        if (Input.GetKeyDown(KeyCode.G)) ThrowWeapon();

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

        if (_currentMainWeapon)
            ThrowWeapon();

        EquipWeapon(newWeapon);
    }
    void EquipWeapon(Weapon newWeapon)
    {
        _currentMainWeapon = newWeapon;
        _lookAtMouse += MainWeapon;
        _currentMainWeapon.PickUp().SetParent(_mainWeaponContainer).SetPosition(_mainWeaponContainer.position);
        _currentMainWeapon.UpdateCurrentWeapon();
        _currentMainWeapon.GetComponent<FireWeapon>().UpdateAmmoAmount();
    }
    private void ThrowWeapon()
    {
        _currentMainWeapon?.ThrowOut(GetMouseDirectionMain()).SetParent(null);
        _currentMainWeapon = null;
        _mainWeaponContainer.eulerAngles = Vector2.zero;
        _lookAtMouse -= MainWeapon;
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

    public void SaveWeapon()
    {
        PlayerPrefs.SetString("MainWeapon", _currentMainWeapon.GetWeaponData.name);
        PlayerPrefs.SetInt("Ammo", _currentMainWeapon.GetComponent<FireWeapon>().GetCurrentAmmo);
    }
    public void LoadWeapon()
    {
        if (PlayerPrefs.HasKey("MainWeapon"))
        {
            var weapon = Instantiate(Resources.Load<Weapon>($"Weapons/{PlayerPrefs.GetString("MainWeapon")}"));
            weapon.GetComponent<FireWeapon>().GetCurrentAmmo = PlayerPrefs.GetInt("Ammo", default);
            EquipWeapon(weapon);
        }
    }
}
