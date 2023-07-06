using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Components.Health;
using Cysharp.Threading.Tasks;
using Game.Settings;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Bot
{
    public class BotFactorty : MonoBehaviour
    {
        [SerializeField] private List<BotComponent> _botsPrefab = new();
        [SerializeField] private float _delayBetweenSpawn;
        [SerializeField] private int _maxCountOfBotsOnMap = 100;

        [SerializeField]
        private float _minNecessaryDistanceFromSpawnPointToBotToSpawn;

        [SerializeField] private List<Transform> _spawnPoints = new();
        private int _currentCountOfBotsOnMap => AllCreatedBots.Count;
        private float _spawnTimer;

        [SerializeField]
        private bool _drawGizmoz, _drawMinNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank, _drawSpawnPoint;

        private Transform _currentSpawnPoint;
        private int _startBotsHealth;
        public List<BotComponent> AllCreatedBots { get; } = new();
        private CancellationTokenSource _cancellationToken;

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
                    Gizmos.DrawSphere(spawnPoint.transform.position, _minNecessaryDistanceFromSpawnPointToBotToSpawn);
                }
            }

            if (_currentSpawnPoint == null) return;
            if (_drawMinNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_currentSpawnPoint.transform.position,
                    _minNecessaryDistanceFromSpawnPointToBotToSpawn);
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
            _cancellationToken = new CancellationTokenSource();
            _spawnTimer = _delayBetweenSpawn;
        }

        private void OnEnable()
        {
            _cancellationToken = new CancellationTokenSource();
            Spawn();
        }

        private async UniTaskVoid Spawn()
        {
            while (true)
            {
                if (_cancellationToken.IsCancellationRequested) break;

                var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
                if (_currentCountOfBotsOnMap < _maxCountOfBotsOnMap &&
                    (NearestBotFromSpawn(out float distance, spawnPoint) == null ||
                     distance > _minNecessaryDistanceFromSpawnPointToBotToSpawn)
                    && _spawnTimer <= 0f)
                {
                    _currentSpawnPoint = spawnPoint;
                    var bot = _botsPrefab[Random.Range(0, _botsPrefab.Count)];
                    var createdBot = Instantiate(bot, spawnPoint.transform.position, Quaternion.identity, transform);
                    AllCreatedBots.Add(createdBot);
                    _spawnTimer = _delayBetweenSpawn;
                    createdBot.health.SetHealth(_startBotsHealth);
                }

                await UniTask.Yield();
            }
        }

        private void OnDisable()
        {
            _cancellationToken.Cancel();
        }

        private void OnDestroy()
        {
            _cancellationToken.Cancel();
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