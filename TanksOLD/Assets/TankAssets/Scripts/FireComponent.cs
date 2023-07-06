using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class FireComponent : MonoBehaviour
    {
        [SerializeField]
        private Projectile _bulletPrefab;
        private bool _canFire = true;
        [SerializeField]
        private float _fireDelay;
        [SerializeField]
        private SideType _side;
        public SideType Side => _side;
        public void OnFire()
        {
            if (!_canFire) return;
            StartCoroutine(Fire());
        }

        private IEnumerator Fire()
        {
            _canFire = false;
            var bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
            var dirType = transform.eulerAngles.ConvertRotationFromType();
            if (dirType == DirectionType.None)
            {
                bullet.SetParams(transform.up, _side);
            }
            else
            {
                bullet.SetParams(dirType, _side);
            }
            SoundManager.instance.Play(SoundManager.SoundType.Shoot);
            yield return new WaitForSeconds(_fireDelay);
            _canFire = true;

        }
    }
}