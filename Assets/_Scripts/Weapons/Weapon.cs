using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData _weaponData;
        [SerializeField] protected Transform _uiSignPosition;

        protected float _weaponTimer;
        protected Rigidbody2D _rb;
        SpriteRenderer _spriteRenderer;
        EquipableUI _equipableUI;
        WeaponManager _weaponManager;
        public WeaponData GetWeaponData { get { return _weaponData; } }
        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _weaponManager = FindObjectOfType<WeaponManager>();
            _equipableUI = FindObjectOfType<EquipableUI>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _weaponData.mainSprite;
        }

        private void OnMouseEnter()
        {
            _spriteRenderer.sprite = _weaponData.selectedSprite;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }

        private void OnMouseOver()
        {
            ShowPickUpSign();
        }

        private void OnMouseExit()
        {
            _spriteRenderer.sprite = _weaponData.mainSprite;
            _equipableUI.SetActive(false);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        public void Attack(Vector2 bulletDirection)
        {
            if (Time.time >= _weaponTimer)
            {
                _weaponTimer = Time.time + 1 / _weaponData.fireRate;
                WeaponAction(bulletDirection);
            }
        }

        public abstract void WeaponAction(Vector2 bulletDirection);

        void ShowPickUpSign()
        {
            if (Vector2.Distance(_weaponManager.WeaponContainer.position, transform.position) <= 2.3f)
                _equipableUI.SetActive(true).SetPosition(_uiSignPosition.position + Vector3.up);
            else _equipableUI.SetActive(false);
        }

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
