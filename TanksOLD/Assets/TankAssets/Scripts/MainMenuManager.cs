using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tanks
{
    [RequireComponent(typeof(AudioSource))]
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private Button _quitButton, _playButton, _settingsButton;
        [SerializeField]
        private GameObject _panel;
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _startMenuAudio;
        [SerializeField]
        private GameSettingsStruct _startGameSettings;
        private void Start()
        {
            GameSettings.Settings = _startGameSettings;
            _quitButton.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
            });
            _playButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene"));
            _settingsButton.onClick.AddListener(OnSettingsButton);
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = _startMenuAudio;
            _audioSource.Play();
        }

        private void OnSettingsButton()
        {
            _panel.SetActive(true);
        }
    }
}