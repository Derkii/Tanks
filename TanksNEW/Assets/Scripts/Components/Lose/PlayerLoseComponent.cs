using System;
using System.Threading;
using Cheats;
using Components.Health;
using Cysharp.Threading.Tasks;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Components.Lose
{
    [RequireComponent(typeof(InputComponent))]
    public class PlayerLoseComponent : MonoBehaviour, ILosable
    {
        [SerializeField] private float _timeForGodMode;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _blinkingDelay;
        [SerializeField] private string _active;
        [SerializeField] private string _notActive;
        private CancellationTokenSource _cancellationTokenSource;
        private HealthComponent _healthComponent;
        [Inject] private SoundManager _soundManager;
        private GodModeCheat godModeCheat;

        private void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _healthComponent = GetComponent<HealthComponent>();
            godModeCheat = new GodModeCheat();
            godModeCheat.Init(new GodModeCheatInitParams(GetComponent<SpriteRenderer>(),
                _healthComponent, _blinkingDelay, _soundManager));
            transform.position = _spawnPoint.position;
        }

        private void OnDestroy()
        {
            godModeCheat.Dispose();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public async UniTaskVoid Lose()
        {
            _healthComponent.SetHealth(_healthComponent.StartHealth);
            await UniTask.Delay(
                TimeSpan.FromSeconds(_soundManager.GetAudioLength(SoundManager.SoundType.DestroyPlayerTank)));
            transform.position = _spawnPoint.position;
            await TurnGodMode();
        }

        public void TurnGodModeUnityEditor(Button button)
        {
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (godModeCheat.IsActive && text.text != _active) return;
            text.SetText(!godModeCheat.IsActive ? _active : _notActive);
            godModeCheat.Turn(!godModeCheat.IsActive);
        }

        private async UniTask TurnGodMode()
        {
            if (godModeCheat.IsActive) return;
            godModeCheat.Turn(true);
            await UniTask.Delay(TimeSpan.FromSeconds(_timeForGodMode), DelayType.UnscaledDeltaTime,
                PlayerLoopTiming.Update, _cancellationTokenSource.Token);
            godModeCheat.Turn(false);
        }
    }
}