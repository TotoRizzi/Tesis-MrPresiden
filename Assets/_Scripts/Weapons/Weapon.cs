using UnityEngine;
public enum WeaponType { Primary, Secundary, Granade }

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData _weaponData;

        protected float _weaponTimer;
        protected Rigidbody2D _rb;

        public WeaponData GetWeaponData { get { return _weaponData; } }
        PickUpArea _pickUpArea;
        SpriteRenderer _weaponSpriteRenderer;
        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            //_pickUpArea = GetComponentInChildren<PickUpArea>();
            //_weaponSpriteRenderer = GetComponent<SpriteRenderer>();
            //_weaponSpriteRenderer.sprite = _weaponData.mainSprite;
        }

        //private void OnMouseEnter()
        //{
        //    _weaponSpriteRenderer.sprite = _weaponData.selectedSprite;
        //}
        //
        //private void OnMouseOver()
        //{
        //    if (_pickUpArea.playerClose) _pickUpArea.ShowUI(true);
        //}
        //private void OnMouseExit()
        //{
        //    _weaponSpriteRenderer.sprite = _weaponData.mainSprite;
        //    _pickUpArea.ShowUI();
        //}
        public void Attack(Vector2 bulletDirection)
        {
            if (Time.time >= _weaponTimer)
            {
                _weaponTimer = Time.time + 1 / _weaponData.fireRate;
                WeaponAction(bulletDirection);
            }
        }

        public abstract void WeaponAction(Vector2 bulletDirection);

        #region BUILDER

        public Weapon SetParent(Transform parent)
        {
            transform.SetParent(parent);
            return this;
        }

        public Weapon SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }

        public Weapon PickUp()
        {
            _rb.simulated = false;
            transform.eulerAngles = Vector3.zero;
            return this;
        }

        public Weapon ThrowOut(Vector2 direction)
        {
            _rb.simulated = true;
            _rb.AddForce(direction * 3, ForceMode2D.Impulse);
            return this;
        }

        #endregion
    }
}
