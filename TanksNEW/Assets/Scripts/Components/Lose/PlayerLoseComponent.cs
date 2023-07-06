using System;
using Cheats;
using Components.Health;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Components.Lose
{
    [RequireComponent(typeof(InputComponent))]
    public class PlayerLoseComponent : MonoBehaviour, ILosable
    {
        [SerializeField] private float _timeForGodMode;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GodMode _godMode;
        private HealthComponent _healthComponent;

        private void Start()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _godMode = new GodMode();
            _godMode.Init(new GodMode.GodModeCheatInitParams(GetComponent<SpriteRenderer>(),
                _healthComponent));
            transform.position = _spawnPoint.position;
        }

        public async UniTaskVoid Lose()
        {
            _healthComponent.SetHealth(_healthComponent.StartHealh);
            _godMode.Turn(true);
            await UniTask.Delay(
                TimeSpan.FromSeconds(SoundManager.instance.GetAudioLength(SoundManager.SoundType.DestroyPlayerTank)));
            transform.position = _spawnPoint.position;
            await TurnGodMode();
        }

        public async UniTask TurnGodMode()
        {
            _godMode.Turn(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_timeForGodMode));
            _godMode.Turn(false);
        }
    }
}