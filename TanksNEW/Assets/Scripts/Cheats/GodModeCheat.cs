using System;
using System.Threading;
using Cheats.Abstraction;
using Components.Health;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Cheats
{
    public struct GodModeCheatInitParams
    {
        public readonly SpriteRenderer SpriteRenderer;
        public readonly HealthComponent HealthComponent;
        public readonly float BlinkingDelay;
        public readonly SoundManager SoundManager;

        public GodModeCheatInitParams(SpriteRenderer spriteRenderer, HealthComponent healthComponent,
            float blinkingDelay, SoundManager soundManager)
        {
            BlinkingDelay = blinkingDelay;
            SoundManager = soundManager;
            SpriteRenderer = spriteRenderer;
            HealthComponent = healthComponent;
        }
    }

    [Serializable]
    public class GodModeCheat : IInitializableCheat<GodModeCheatInitParams>, ITurnableCheat, IDisposable
    {
        private float _blinkingDelay;
        private CancellationTokenSource _cancellationTokenSource;
        private HealthComponent _health;
        private SoundManager _soundManager;
        private SpriteRenderer _spriteRenderer;
        private Color _startColor;
        public bool IsActive { get; private set; }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Init(GodModeCheatInitParams initParams)
        {
            _soundManager = initParams.SoundManager;
            _blinkingDelay = initParams.BlinkingDelay;
            _spriteRenderer = initParams.SpriteRenderer;
            _health = initParams.HealthComponent;
            _cancellationTokenSource = new CancellationTokenSource();
            _startColor = _spriteRenderer.color;
        }

        public void Turn(bool turn)
        {
            TurnGodMode(turn);
        }

        private async UniTask TurnGodMode(bool turn)
        {
            if (IsActive == turn) return;
            IsActive = turn;
            if (!turn)
            {
                _spriteRenderer.color = _startColor;
                _health.SetHealth(_health.StartHealth);
                _soundManager.Play(SoundManager.SoundType.GodMode);
                _cancellationTokenSource.Cancel();
            }
            else
            {
                while (true)
                {
                    if (_cancellationTokenSource.IsCancellationRequested)
                    {
                        _spriteRenderer.color = _startColor;
                        _cancellationTokenSource = new CancellationTokenSource();
                        break;
                    }

                    _spriteRenderer.color = Color.clear;
                    _health.SetHealth(int.MaxValue);
                    await UniTask.Delay(TimeSpan.FromSeconds(_blinkingDelay));
                    _spriteRenderer.color = _startColor;
                    await UniTask.Delay(TimeSpan.FromSeconds(_blinkingDelay));
                }
            }
        }
    }
}