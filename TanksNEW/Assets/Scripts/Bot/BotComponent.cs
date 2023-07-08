using System;
using System.Linq;
using Components;
using Components.Health;
using Cysharp.Threading.Tasks;
using TagScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bot
{
    [RequireComponent(typeof(MoveComponent), typeof(FireComponent), typeof(HealthComponent))]
    public class BotComponent : MonoBehaviour
    {
        [SerializeField] private float _minTime, _maxTime;
        [SerializeField] private Transform _startPosition;
        private DirectionType _dirType;
        public HealthComponent Health;
        private FireComponent _fireComponent;
        private MoveComponent _moveComponent;
        private Vector3 velocity;
        private bool _wait;

        private void Start()
        {
            _fireComponent = GetComponent<FireComponent>();
            _moveComponent = GetComponent<MoveComponent>();
            Fire();
        }

        private async UniTaskVoid Fire()
        {
            while (true)
            {
                if (this.GetCancellationTokenOnDestroy().IsCancellationRequested) break;

                await _fireComponent.Fire();
            }
        }

        private void FixedUpdate()
        {
            var hits = Physics2D.RaycastAll(_startPosition.transform.position, -_startPosition.right, 5f);
            var iteration = 0;
            foreach (var hit in hits.Where(t => t.transform != null))
            {
                iteration++;
                if (hit.transform.TryGetComponent(out CellComponent cellComponent))
                {
                    if (!cellComponent.IsDestroyableByProjectile)
                    {
                        ChangeDirection(Random.Range(_minTime, _maxTime));
                    }

                    break;
                }
                else if (
                    hit.transform.TryGetComponent(out FrameOfTileMap _)
                    || hit.transform.TryGetComponent(out Water _)
                    || hit.transform.TryGetComponent(out Player _)
                    || (hit.transform.TryGetComponent(out BotComponent _) && iteration > 1))
                {
                    ChangeDirection();
                    break;
                }
            }

            _moveComponent.Move(_dirType);
        }
        private async UniTask ChangeDirection(float delay)
        {
            if (_wait == false)
            {
                _wait = true;
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
                await ChangeDirection();
                _wait = false;
            }
        }

        private async UniTask ChangeDirection()
        {
            var oldType = _dirType;
            var newType = _dirType;
            while (newType == oldType)
            {
                newType = _dirType.RandomType();
                await UniTask.Yield();
            }

            _dirType = newType;
        }
    }
}