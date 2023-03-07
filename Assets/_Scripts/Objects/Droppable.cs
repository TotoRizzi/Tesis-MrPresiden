using UnityEngine;
namespace Droppables
{
    public abstract class Droppable : MonoBehaviour, IBreakable, IDamageable
    {
        [SerializeField] protected GameObject _destroyedVersion;

        bool _drop;
        protected Transform _dropPosition;

        [Range(0, 1)]
        protected float _probabilityDrop;
        protected virtual void Start()
        {
            //_drop = Random.value <= _probabilityDrop;
            //if (_drop) _dropPosition = transform.GetChild(0);
        }
        public void Break()
        {
            if (_destroyedVersion != null)
                Instantiate(_destroyedVersion, transform.position, Quaternion.identity);
            //if (_drop) 
            DropObject();
            Destroy(gameObject);
        }
        protected abstract void DropObject();
        public void TakeDamage(float dmg)
        {
            Die();
        }

        public void Die()
        {
            Break();
        }
    }
}
