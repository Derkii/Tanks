using System;
using System.Collections;
using UnityEngine;

namespace Tanks
{
    public class ConditionComponent : MonoBehaviour
    {
        [SerializeField]
        private int _health = 5;
        [SerializeField]
        private float _godModeTime;
        private GodModeComponent _godModeComponent;
        private SpawnComponent _spawnComponent;
        private InputComponent _inputComponent;
        private bool _played;

        public int Healh => _health;

        public float GodModeTime => _godModeTime;

        private void Start()
        {
            if (name == "Player")
            {
                _spawnComponent = GetComponent<SpawnComponent>();
                _godModeComponent = GetComponent<GodModeComponent>();
                _inputComponent = GetComponent<InputComponent>();
                _health = Mathf.RoundToInt(GameSettings.Settings.PlayerHealth);
        
            }
           
        }
        public void SetDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                StartCoroutine(OnLoseCoroutine());
                return;
            }
            if (name == "Player")
            {
                _spawnComponent.ToSpawnPoint();
                _inputComponent.SetCollision(false);
                _godModeComponent.GodMode(true, _godModeTime);
            }
        }
        public void SetHealth(int health)
        {
            _health = health;
        }

        private IEnumerator OnLoseCoroutine()
        {
            SoundManager.instance.Play(name.Contains("Bot") ? SoundManager.SoundType.DestroyEnemyTank : name == "Player" ? SoundManager.SoundType.DestroyPlayerTank : SoundManager.SoundType.None);
            if (name == "Player")
            {
                _inputComponent.IsPlaying = false;
                yield return new WaitForSeconds(SoundManager.instance.AudioLength(SoundManager.SoundType.DestroyPlayerTank) + 1f);
                LeaveFromGameManager.Quit();
            }
            else
            {
                GetComponent<BotComponent>().Lose = true;

                yield return new WaitForSeconds(SoundManager.instance.AudioLength(SoundManager.SoundType.DestroyEnemyTank));

                Destroy(gameObject);
            }
        }
    }
}