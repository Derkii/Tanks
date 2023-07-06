using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField]
        private Transform _spawnPoint;
        private void Start()
        {
            ToSpawnPoint();
        }
        public void ToSpawnPoint()
        {
            transform.position = _spawnPoint.position;
        }
    }
}
