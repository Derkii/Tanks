using Components;
using Components.Health;
using Lean.Pool;
using Managers;
using UnityEngine;
using VContainer;

namespace Projectile
{
    [RequireComponent(typeof(MoveComponent))]
    public class ProjectileComponent : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _lifeTime = 3f;
        private Vector3 _direction;

        private MoveComponent _moveComponent;
        private SideType _side;

        [Inject] private SoundManager _soundManager;

        private void Start()
        {
            _moveComponent = GetComponent<MoveComponent>();
            LeanPool.Despawn(this, _lifeTime);
        }

        private void FixedUpdate()
        {
            _moveComponent.Move(_direction);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out FireComponent fireComponent))
            {
                if (fireComponent.Side == _side) return;

                _soundManager.Play(SoundManager.SoundType.ProjectileCollision);
                var health = collision.GetComponent<HealthComponent>();
                health.Damage(_damage);
                LeanPool.Despawn(this);
            }

            else if (collision.TryGetComponent(out CellComponent cellComponent))
            {
                _soundManager.Play(SoundManager.SoundType.ProjectileCollision);

                if (cellComponent.DestroyProjectile) LeanPool.Despawn(this);
                if (cellComponent.IsDestroyableByProjectile) Destroy(cellComponent.gameObject);
            }

            else if (collision.transform.TryGetComponent(out FrameOfTileMap _))
            {
                _soundManager.Play(SoundManager.SoundType.ProjectileCollision);
                LeanPool.Despawn(this);
            }
        }

        public void Init(DirectionType directionType, SideType side)
        {
            _side = side;
            _direction = directionType.ConvertTypeToDirection();
        }
    }
}