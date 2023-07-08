using System;
using Bot;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using VContainer;

namespace Components.Lose
{
    public class BotLoseComponent : MonoBehaviour, ILosable
    {
        private BotComponent _botComponent;
        [Inject]
        private SoundManager _soundManager;

        private void Start()
        {
            _botComponent = GetComponent<BotComponent>();
        }

        public async UniTaskVoid Lose()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_soundManager.GetAudioLength(SoundManager.SoundType.DestroyBotTank)));
            Destroy(_botComponent.gameObject);
        }
    }
}