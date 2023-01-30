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
        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
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
