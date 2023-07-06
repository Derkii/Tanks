using System;
using System.Threading;
using Components.Health;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEditor;
using UnityEngine;

namespace Cheats
{
    [Serializable]
    public class GodMode : IInitializableCheat<GodMode.GodModeCheatInitParams>, ITurnableCheat
    {
        public struct GodModeCheatInitParams
        {
            public readonly SpriteRenderer SpriteRenderer;
            public readonly HealthComponent HealthComponent;

            public GodModeCheatInitParams(SpriteRenderer spriteRenderer, HealthComponent healthComponent)
            {
                SpriteRenderer = spriteRenderer;
                HealthComponent = healthComponent;
            }
        }

        [SerializeField] private float _flashingDelay;
        private SpriteRenderer _spriteRenderer;
        private HealthComponent _health;
        private Color _startColor;
        private CancellationTokenSource _cancellationTokenSource;
        public bool IsActive { get; private set; }

        public void Init(GodModeCheatInitParams initParams)
        {
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
                _cancellationTokenSource.Cancel();
                _spriteRenderer.color = _startColor;
                _health.SetHealth(_health.StartHealh);
                SoundManager.instance.Play(SoundManager.SoundType.GodMode);
                return;
            }

            while (true)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    break;
                }

                _spriteRenderer.color = Color.clear;
                _health.SetHealth(int.MaxValue);
                await UniTask.Delay(TimeSpan.FromSeconds(_flashingDelay), DelayType.UnscaledDeltaTime,
                    PlayerLoopTiming.Update, _cancellationTokenSource.Token);
                _spriteRenderer.color = _startColor;
                await UniTask.Delay(TimeSpan.FromSeconds(_flashingDelay), DelayType.UnscaledDeltaTime,
                    PlayerLoopTiming.Update, _cancellationTokenSource.Token);
            }
        }
    }
}