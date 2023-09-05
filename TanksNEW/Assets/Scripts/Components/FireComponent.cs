using System;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using Managers;
using Projectile;
using UnityEngine;
using VContainer;

namespace Components
{
    public class FireComponent : MonoBehaviour
    {
        [SerializeField] private ProjectileComponent _projectilePrefab;
        [SerializeField] private float _fireDelay;
        [SerializeField] private SideType _side;
        private bool _canFire = true;

        [Inject] private IObjectResolver _resolver;
        [Inject] private SoundManager _soundManager;
        public SideType Side => _side;

        public async UniTask Fire(bool check = true)
        {
            if (check && _canFire == false) return;
            _canFire = false;
            var projectile = LeanPool.Spawn(_projectilePrefab, transform.position, transform.rotation);
            var dirType = transform.eulerAngles.ConvertRotationFromType();
            projectile.Init(dirType, _side);
            _resolver.Inject(projectile);
            _soundManager.Play(SoundManager.SoundType.Shoot);
            await UniTask.Delay(TimeSpan.FromSeconds(_fireDelay));
            _canFire = true;
        }
    }
}