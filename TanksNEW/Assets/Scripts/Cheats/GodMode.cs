using System;
using System.Threading;
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
        public GodModeCheatInitParams(SpriteRenderer spriteRenderer, HealthComponent healthComponent, float blinkingDelay, SoundManager soundManager)
        {
            BlinkingDelay = blinkingDelay;
            SoundManager = soundManager;
            SpriteRenderer = spriteRenderer;
            HealthComponent = healthComponent;
        }
    }

    [Serializable]
    public class GodMode : IInitializableCheat<GodModeCheatInitParams>, ITurnableCheat
    {
        private float _blinkingDelay;
        private SpriteRenderer _spriteRenderer;
        private HealthComponent _health;
        private Color _startColor;
        private CancellationTokenSource _cancellationTokenSource;
        private SoundManager _soundManager;
        public bool IsActive { get; private set; }

        public void Init(GodModeCheatInitParams initParams)
        {
            _soundManager = initParams.SoundManager;
            _blinkingDelay = initParams.BlinkingDelay;
            _spriteRenderer = initParams.SpriteRenderer;
            _health = initParams.HealthComponent;
            _cancellationTokenSource = new CancellationTokenSource();
            _startColor = _spriteRenderer.color;
        }

        public void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
        }
        public void Turn(bool turn)
        {
            TurnGodMode(turn);
        }

        private async UniTask TurnGodMode(bool turn)
        {
            if (IsActive == turn) return;
            IsActive = turn;
            if (turn)
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }
            else
            {
                _spriteRenderer.color = _startColor;
                _health.SetHealth(_health.StartHealth);
                _soundManager.Play(SoundManager.SoundType.GodMode);
                _cancellationTokenSource.Cancel();
                return;
            }

            while (true)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    _cancellationTokenSource.Dispose();
                    break;
                }

                _spriteRenderer.color = Color.clear;
                _health.SetHealth(int.MaxValue);
                await UniTask.Delay(TimeSpan.FromSeconds(_blinkingDelay), DelayType.UnscaledDeltaTime,
                    PlayerLoopTiming.Update, _cancellationTokenSource.Token);
                _spriteRenderer.color = _startColor;
                await UniTask.Delay(TimeSpan.FromSeconds(_blinkingDelay), DelayType.UnscaledDeltaTime,
                    PlayerLoopTiming.Update, _cancellationTokenSource.Token);
            }
        }
    }
}