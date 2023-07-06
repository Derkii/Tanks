using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks
{
    public class BotFactorty : MonoBehaviour
    {
        [SerializeField]
        private List<BotComponent> _botsPrefab = new List<BotComponent>();
        [SerializeField]
        private float _delayBetweenSpawn;
        [SerializeField]
        private int _maxCountOfBotsOnMap = 100;
        [SerializeField]
        private float _minNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank;
        [SerializeField]
        private List<Transform> _spawnPoints = new List<Transform>();
        private int _currentCountOfBotsOnMap => AllCreatedBots.Count;
        private float _spawnTimer;
        [SerializeField]
        private bool _drawGizmoz, _drawMinNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank, _drawSpawnPoint;
        private Transform _currentSpawnPoint;
        private int _startBotsHealth;
        public List<BotComponent> AllCreatedBots { get; private set; } = new List<BotComponent>();

        private void OnDrawGizmos()
        {
            if (!_drawGizmoz) return;

            foreach (var spawnPoint in _spawnPoints)
            {
                if (spawnPoint == _currentSpawnPoint) continue;
                if (_drawSpawnPoint)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(spawnPoint.position, spawnPoint.localScale);
                }
                if (_drawMinNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(spawnPoint.transform.position, _minNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank);
                }
            }
            if (_currentSpawnPoint == null) return;
            if (_drawMinNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_currentSpawnPoint.transform.position, _minNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank);
            }
            if (_drawSpawnPoint)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(_currentSpawnPoint.position, _currentSpawnPoint.localScale);
            }
        }

        private void Start()
        {
            _startBotsHealth = (int)GameSettings.Settings.BotHealth;
            AllCreatedBots.AddRange(FindObjectsOfType<BotComponent>());
            StartCoroutine(SpawnCoroutine());
            _spawnTimer = _delayBetweenSpawn;
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                var spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)];
                if (_currentCountOfBotsOnMap < _maxCountOfBotsOnMap && 
                    (NearestBotFromSpawn(out float distance, spawnPoint) == null ? true : 
                    distance > _minNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank) && _spawnTimer <= 0f)
                {
                    _currentSpawnPoint = spawnPoint;
                    var bot = _botsPrefab[UnityEngine.Random.Range(0, _botsPrefab.Count)];
                    var createdBot = Instantiate(bot);
                    createdBot.transform.position = spawnPoint.transform.position;
                    createdBot.transform.parent = transform;
                    AllCreatedBots.Add(createdBot);
                    _spawnTimer = _delayBetweenSpawn;
                    createdBot.ConditionComponent = createdBot.GetComponent<ConditionComponent>();
                    createdBot.ConditionComponent.SetHealth(_startBotsHealth);
                }
                yield return null;
            }
        }
        private void Update()
        {
            _spawnTimer -= Time.deltaTime;
        }
        private void FixedUpdate()
        {
            AllCreatedBots.RemoveAll(t => t == null);
        }
        private BotComponent NearestBotFromSpawn(out float distance, Transform spawnPoint)
        {
            distance = float.MaxValue;
            BotComponent nearestBot = null;
            foreach (var item in AllCreatedBots.Where(t => t != null))
            {
                var dist = Vector3.Distance(spawnPoint.position, item.transform.position);
                if (dist < distance)
                {
                    distance = dist;
                    nearestBot = item;
                }
            }
            return nearestBot;
        }
    }
}

