using Components;
using Components.Health;
using Managers;
using UnityEngine;
using static NTC.Global.Pool.NightPool;

namespace Projectile
{
    [RequireComponent(typeof(MoveComponent))]
    public class ProjectileComponent : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _lifeTime = 3f;
        private SideType _side;
        private Vector3 _direction;

        private MoveComponent _moveComponent;

        private void Start()
        {
            _moveComponent = GetComponent<MoveComponent>();
            Destroy(gameObject, _lifeTime);
        }

        private void FixedUpdate()
        {
            _moveComponent.Move(_direction);
        }

        public void Init(DirectionType directionType, SideType side)
        {
            _side = side;
            _direction = directionType.ConvertTypeToDirection();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out FireComponent fireComponent))
            {
                if (fireComponent.Side == _side) return;

                SoundManager.instance.Play(SoundManager.SoundType.ProjectileCollision);
                var health = collision.GetComponent<HealthComponent>();
                health.Damage(_damage);
                Despawn(gameObject);
            }

            else if (collision.TryGetComponent(out CellComponent cellComponent))
            {
                SoundManager.instance.Play(SoundManager.SoundType.ProjectileCollision);
                if (cellComponent.IsDestroyableByProjectile)
                {
                    Destroy(cellComponent.gameObject);
                }

                if (cellComponent.DestroyProjectile)
                {
                    Despawn(gameObject);
                }
            }

            else if (collision.transform.TryGetComponent(out FrameOfTileMap _))
            {
                SoundManager.instance.Play(SoundManager.SoundType.ProjectileCollision);
                Despawn(gameObject);
            }
        }
    }
}