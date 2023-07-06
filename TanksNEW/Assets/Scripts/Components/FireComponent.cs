using System;
using Cysharp.Threading.Tasks;
using Managers;
using Projectile;
using UnityEngine;

namespace Components
{
    public class FireComponent : MonoBehaviour
    {
        [SerializeField] private ProjectileComponent _bulletPrefab;
        [SerializeField] private float _fireDelay;
        [SerializeField] private SideType _side;
        public SideType Side => _side;
        private bool _canFire = true;

        public async UniTask Fire(bool check = true)
        {
            if (check && _canFire == false) return;
            _canFire = false;
            var projectile = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            var dirType = transform.eulerAngles.ConvertRotationFromType();

            projectile.Init(dirType, _side);
            SoundManager.instance.Play(SoundManager.SoundType.Shoot);
            await UniTask.Delay(TimeSpan.FromSeconds(_fireDelay));
            _canFire = true;
        }
    }
}