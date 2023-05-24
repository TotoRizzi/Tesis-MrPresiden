using System;
using UnityEngine;
using Weapons;
public class WeaponManager : MonoBehaviour
{
    [SerializeField] Weapon _currentMainWeapon, _currentSecundaryWeapon;
    [SerializeField] GameObject _rightHand, _leftHand;

    Camera _mainCamera;
    Transform _mainWeaponContainer;
    Transform _secundaryWeaponContainer;
    InputManager _inputManager;
    Action _lookAtMouse = delegate { };
    bool _onWeaponTrigger;
    public Transform SecundaryWeaponContainer { get { return _secundaryWeaponContainer; } }
    public Transform MainWeaponContainer { get { return _mainWeaponContainer; } }
    public FireWeapon GetMainWeapon { get { return _currentMainWeapon?.GetComponent<FireWeapon>(); } }
    private void Start()
    {
        _inputManager = InputManager.Instance;
        _mainCamera = Camera.main;
        _mainWeaponContainer = transform.GetChild(0);
        _secundaryWeaponContainer = transform.GetChild(1);
        _currentSecundaryWeapon = _secundaryWeaponContainer.GetComponentInChildren<Weapon>();
        _currentSecundaryWeapon.PickUp(true);
        _lookAtMouse += SecundaryWeapon;

        //LoadWeapon();
    }
    private void Update()
    {
        _lookAtMouse?.Invoke();

        if (_inputManager.GetButton("Shoot") && _currentMainWeapon) _currentMainWeapon.Attack(GetMouseDirectionMain());

        if (_inputManager.GetButton("Knife") && _currentSecundaryWeapon)
        {
            Helpers.LevelTimerManager.StartLevelTimer();
            _currentSecundaryWeapon.Attack(GetMouseDirectionSecundary());
        }

        //if (_inputManager.GetButtonDown("Throw Weapon")) ThrowWeapon();

        if (_inputManager.GetButtonDown("Pick Up") && _onWeaponTrigger) SetWeapon();
    }

    #region Weapon Funcs
    public void SetWeapon()
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
        _rightHand.SetActive(false);
        _leftHand.SetActive(false);
    }
    private void ThrowWeapon()
    {
        RaycastHit2D raycast = Physics2D.Raycast(_mainWeaponContainer.position, GetMousePosition(), 1f, LayerMask.GetMask("Border"));
        if (raycast)
        {
            _currentMainWeapon.transform.position += (Vector3)raycast.normal / 2;
            _currentMainWeapon?.ThrowOut(raycast.normal).SetParent(null);
        }
        else
            _currentMainWeapon?.ThrowOut(GetMouseDirectionMain()).SetParent(null);

        _currentMainWeapon = null;
        _mainWeaponContainer.eulerAngles = Vector2.zero;
        _lookAtMouse -= MainWeapon;
        GameManager.instance.UiManager.SetDefaultWeaponSprite();
        _rightHand.SetActive(true);
        _leftHand.SetActive(true);
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
    public Vector2 GetMousePosition() => _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    Vector2 GetMouseDirectionMain() => (GetMousePosition() - (Vector2)_mainWeaponContainer.position).normalized;
    Vector2 GetMouseDirectionSecundary() => (GetMousePosition() - (Vector2)_secundaryWeaponContainer.position).normalized;

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>()) _onWeaponTrigger = true; 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ShowKeyUI>()) _onWeaponTrigger = false;
    }

    //public void SaveWeapon()
    //{
    //    if (_currentMainWeapon)
    //    {
    //        GameManager.instance.SaveDataManager.SaveString("MainWeapon", _currentMainWeapon.GetWeaponData.name);
    //        GameManager.instance.SaveDataManager.SaveInt("Ammo", _currentMainWeapon.GetComponent<FireWeapon>().GetCurrentAmmo);
    //    }
    //    else PlayerPrefs.DeleteKey("MainWeapon");
    //}
    //public void LoadWeapon()
    //{
    //    if (PlayerPrefs.HasKey("MainWeapon"))
    //    {
    //        var weapon = Instantiate(Resources.Load<Weapon>($"Weapons/{PlayerPrefs.GetString("MainWeapon")}"));
    //        weapon.GetComponent<FireWeapon>().GetCurrentAmmo = PlayerPrefs.GetInt("Ammo", default);
    //        EquipWeapon(weapon);
    //    }
    //}
    //private void OnDestroy()
    //{
    //    SaveWeapon();
    //}
}
