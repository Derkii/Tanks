using System;
using Bot;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Components.Lose
{
    public class BotLoseComponent : MonoBehaviour, ILosable
    {
        private BotComponent _botComponent;

        private void Start()
        {
            _botComponent = GetComponent<BotComponent>();
        }

        public async UniTaskVoid Lose()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(SoundManager.instance.GetAudioLength(SoundManager.SoundType.DestroyBotTank)));
            Destroy(_botComponent.gameObject);
        }
    }
}