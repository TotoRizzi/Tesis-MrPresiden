using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData _weaponData;
        [SerializeField] protected Transform _uiSignPosition;

        protected float _weaponTimer;
        protected Rigidbody2D _rb;
        protected WeaponManager _weaponManager;
        protected SpriteRenderer _spriteRenderer;

        EquipableUI _equipableUI;
        public WeaponData GetWeaponData { get { return _weaponData; } }

        public bool CanPickUp => Mathf.Abs(_rb.velocity.magnitude) < .1f;
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
            if (!CanPickUp) return;
            _spriteRenderer.sprite = _weaponData.selectedSprite;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }

        private void OnMouseOver()
        {
            if (!CanPickUp) return;
            ShowPickUpSign();
        }

        private void OnMouseExit()
        {
            if (!CanPickUp) return;
            _spriteRenderer.sprite = _weaponData.mainSprite;
            _equipableUI.SetActive(false);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_uiSignPosition.position, Vector2.down * 1f);
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
            if (Vector2.Distance(_weaponManager.SecundaryWeaponContainer.position, transform.position) <= 2.3f)
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
            _rb.velocity = Vector2.zero;
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
