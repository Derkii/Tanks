using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        [SerializeField]
        private AudioClip _projectileCollisionSound,
            _playerTankDestroySound,
            _enemyTankDestroySound,
            _leaveFromGameSceneSound,
            _playerTankMovingSound,
            _playerTankCollisionWithBlockSound,
            _pressEscapeSound,
            _shootSound,
            _godModeSound;
        private AudioSource[] _audioSources;
        private Dictionary<SoundType, AudioClip> _sounds;
        private float _startVolume;
        private void Awake()
        {
            _startVolume = GameSettings.Settings.Volume;
        }
        private void Start()
        {
            _sounds = new Dictionary<SoundType, AudioClip>()
            {
                {
                    SoundType.ProjectileCollision, _projectileCollisionSound
                },
                {
                    SoundType.DestroyEnemyTank, _enemyTankDestroySound
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
            instance = this;
            _audioSources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in _audioSources)
            {
                audioSource.volume = GameSettings.Settings.Volume;
                audioSource.minDistance = 0f;
                audioSource.maxDistance = 30f;
            }
        }

        public void Play(SoundType type)
        {
            var source = _audioSources.FirstOrDefault(t => t != null && t.clip == _sounds[type] ? true : t.isPlaying == false);
            if (type == SoundType.None) return;
            if (type == SoundType.PlayerTankMoving || type == SoundType.PlayerTankCollisionWithBlock)
            {
                source.volume = GameSettings.Settings.Volume / 3f;
            }
            else
            {
                if (source == null) return;
                source.volume = GameSettings.Settings.Volume;
            }
            if (type == SoundType.GodMode)
            {
                if (_audioSources.FirstOrDefault(t => t != null && t.clip == _sounds[type] && t.isPlaying == true) != null)
                {
                    return;
                }
            }

            Play(_sounds[type]);

        }
        public void PlayOneShot(SoundType type)
        {
            if (type == SoundType.None) return;
            if (type == SoundType.PlayerTankMoving || type == SoundType.PlayerTankCollisionWithBlock)
            {
                _audioSources.FirstOrDefault(t => t != null && t.clip == _sounds[type] ? true : t.isPlaying == false).volume = GameSettings.Settings.Volume / 3f;
            }
            else
            {
                _audioSources.FirstOrDefault(t => t != null && t.clip == _sounds[type] ? true : t.isPlaying == false).volume = GameSettings.Settings.Volume;
            }
            if (type == SoundType.GodMode)
            {
                if (_audioSources.FirstOrDefault(t => t != null && t.clip == _sounds[type] && t.isPlaying == true) != null)
                    {
                        return;
                }
            }

            PlayOneShot(_sounds[type]);
        }
        private void PlayOneShot(AudioClip clip)
        {
            var audioSource = _audioSources.FirstOrDefault(t => t != null && t.clip == clip ? true : t.isPlaying == false);
            audioSource.volume = _startVolume;
            if (clip == null || audioSource == null) return;

            audioSource.PlayOneShot(clip);
        }
        private void Play(AudioClip clip)
        {
            var audioSource = _audioSources.FirstOrDefault(t => t != null && t.clip == clip ? true : t.isPlaying == false);
            if (clip == null || audioSource == null) return;

            audioSource.clip = clip;
            audioSource.Play();
        }

        public float AudioLength(SoundType type)
        {
            if (type == SoundType.None) return 0f;
            var audioSource = _audioSources.FirstOrDefault(t => t != null && t.clip == _sounds[type]);
            if (audioSource != null)
            {
                return audioSource.clip.length;
            }
            return 0f;
        }
        public enum SoundType
        {
            None,
            ProjectileCollision,
            DestroyPlayerTank,
            DestroyEnemyTank,
            LeaveFromGame,
            PlayerTankMoving,
            PlayerTankCollisionWithBlock,
            PressESC,
            Shoot,
            GodMode
        }
    }
}
