using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Settings;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Bot
{
    public class BotFactorty : MonoBehaviour
    {
        [SerializeField] private List<BotComponent> _botsPrefab = new();
        [SerializeField] private float _delayBetweenSpawn;
        [SerializeField] private int _maxCountOfBotsOnMap = 100;

        [SerializeField] private float _minNecessaryDistanceFromSpawnPointToBotToSpawn;

        [SerializeField] private List<Transform> _spawnPoints = new();

        [SerializeField]
        private bool _drawGizmoz, _drawMinNecessaryDistanceFromSpawnPointToTanksToSpawnNewTank, _drawSpawnPoint;

        private CancellationTokenSource _cancellationToken;

        private Transform _currentSpawnPoint;
        [Inject] private IObjectResolver _resolver;
        private float _spawnTimer;
        private int _startBotsHealth;
        private int _currentCountOfBotsOnMap => AllCreatedBots.Count;
        public List<BotComponent> AllCreatedBots { get; } = new();

        private void Start()
        {
            _startBotsHealth = GameSettings.Settings.BotHealth;
            AllCreatedBots.AddRange(FindObjectsOfType<BotComponent>());
            _cancellationToken = new CancellationTokenSource();
            _spawnTimer = _delayBetweenSpawn;
        }

        private void Update()
        {
            _spawnTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            AllCreatedBots.RemoveAll(t => t == null);
        }

        private void OnEnable()
        {
            _cancellationToken = new CancellationTokenSource();
            Spawn();
        }

        private void OnDisable()
        {
            _cancellationToken.Cancel();
        }

        private void OnDestroy()
        {
            _cancellationToken.Cancel();
        }

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

        private async UniTaskVoid Spawn()
        {
            while (true)
            {
                if (_cancellationToken.IsCancellationRequested) break;

                var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
                if (_currentCountOfBotsOnMap < _maxCountOfBotsOnMap &&
                    (NearestBotFromSpawn(out var distance, spawnPoint) == null ||
                     distance > _minNecessaryDistanceFromSpawnPointToBotToSpawn)
                    && _spawnTimer <= 0f)
                {
                    _currentSpawnPoint = spawnPoint;
                    var bot = _botsPrefab[Random.Range(0, _botsPrefab.Count)];
                    var createdBot = Instantiate(bot, spawnPoint.transform.position, Quaternion.identity, transform);
                    _resolver.InjectGameObject(createdBot.gameObject);
                    AllCreatedBots.Add(createdBot);
                    _spawnTimer = _delayBetweenSpawn;
                    createdBot.Health.SetHealth(_startBotsHealth);
                }

                await UniTask.Yield();
            }
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