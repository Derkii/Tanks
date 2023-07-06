using System;
using System.Collections.Generic;
using System.Linq;
using Game.Settings;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        [SerializeField]
        private AudioSource[] _audioSources;
        [SerializeField] private AudioClip _projectileCollisionSound,
            _playerTankDestroySound,
            _enemyTankDestroySound,
            _leaveFromGameSceneSound,
            _playerTankMovingSound,
            _playerTankCollisionWithBlockSound,
            _pressEscapeSound,
            _shootSound,
            _godModeSound;
        private Dictionary<SoundType, AudioClip> _sounds;
        private AudioSource _audioSource
        {
            get
            {
                var audioSource = _audioSources.FirstOrDefault(t => t.isPlaying == false);
                
                return  audioSource == null ? _audioSources[0] : audioSource;
            }
        }

        private void Awake()
        {
            instance = this;
            _sounds = new Dictionary<SoundType, AudioClip>()
            {
                {
                    SoundType.ProjectileCollision, _projectileCollisionSound
                },
                {
                    SoundType.DestroyBotTank, _enemyTankDestroySound
                },
                {
                    SoundType.DestroyPlayerTank, _playerTankDestroySound
                },
                {
                    SoundType.PlayerTankMoving, _playerTankMovingSound
                },
                {
                    SoundType.PlayerTankCollisionWithBlock, _playerTankCollisionWithBlockSound
                },
                {
                    SoundType.PressESC, _pressEscapeSound
                },
                {
                    SoundType.LeaveFromGame, _leaveFromGameSceneSound
                },
                {
                    SoundType.Shoot, _shootSound
                },
                {
                    SoundType.GodMode, _godModeSound
                }
            };
            int length = 6;
            _audioSources = new AudioSource[length];
            for (int i = 0; i < length; i++)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                _audioSources[i] = audioSource;
                audioSource.volume = GameSettings.Settings.Volume;
                audioSource.minDistance = 0f;
                audioSource.maxDistance = 30f;
            }
        }

        public void Play(SoundType type)
        {
            if (type == SoundType.None) throw new NotImplementedException();
            
            if (type == SoundType.PlayerTankMoving || type == SoundType.PlayerTankCollisionWithBlock)
            {
                _audioSource.volume = GameSettings.Settings.Volume / 3f;
            }
            
            Play(_sounds[type]);
        }

        private void Play(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public float GetAudioLength(SoundType type)
        {
            if (type == SoundType.None) return 0f;
            if (_audioSource != null)
            {
                return _audioSource.clip.length;
            }

            return 0f;
        }

        public enum SoundType
        {
            None,
            ProjectileCollision,
            DestroyPlayerTank,
            DestroyBotTank,
            LeaveFromGame,
            PlayerTankMoving,
            PlayerTankCollisionWithBlock,
            PressESC,
            Shoot,
            GodMode
        }
    }
}