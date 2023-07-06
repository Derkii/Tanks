using UnityEngine;

namespace Components
{
    public class CellComponent : MonoBehaviour
    {
        [SerializeField]
        private bool _isDestroyableByProjectile;
        [SerializeField]
        private bool _destroyProjectile;
        public bool IsDestroyableByProjectile => _isDestroyableByProjectile;
        public bool DestroyProjectile => _destroyProjectile;
    }
}