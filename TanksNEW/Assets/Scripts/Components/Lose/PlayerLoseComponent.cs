using System;
using Cheats;
using Components.Health;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using VContainer;

namespace Components.Lose
{
    [RequireComponent(typeof(InputComponent))]
    public class PlayerLoseComponent : MonoBehaviour, ILosable
    {
        [SerializeField] private float _timeForGodMode;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _blinkingDelay;
        [Inject] private SoundManager _soundManager;
        private GodMode _godMode;
        private HealthComponent _healthComponent;
        
        private void Start()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _godMode = new GodMode();
            _godMode.Init(new GodModeCheatInitParams(GetComponent<SpriteRenderer>(),
                _healthComponent, _blinkingDelay, _soundManager));
            transform.position = _spawnPoint.position;
        }

        public async UniTaskVoid Lose()
        {
            _healthComponent.SetHealth(_healthComponent.StartHealth);
            _godMode.Turn(true);
            await UniTask.Delay(
                TimeSpan.FromSeconds(_soundManager.GetAudioLength(SoundManager.SoundType.DestroyPlayerTank)));
            transform.position = _spawnPoint.position;
            await TurnGodMode();
        }

        private void OnDestroy()
        {
            _godMode.OnDestroy();
        }

        public async UniTask TurnGodMode()
        {
            _godMode.Turn(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_timeForGodMode));
            _godMode.Turn(false);
        }
    }
}