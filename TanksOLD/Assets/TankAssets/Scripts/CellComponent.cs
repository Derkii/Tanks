using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class CellComponent : MonoBehaviour
    {
        [SerializeField]
        private bool _isDestroyable, _isDestroyBullet;
        public bool IsDestroyable => _isDestroyable;
        public bool CanDestroyBullet => _isDestroyBullet;
    }
}