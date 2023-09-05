using Game.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Button _quitButton, _playButton, _settingsButton;

        [SerializeField] private GameObject _panel;

        [SerializeField] private AudioClip _startMenuAudio;

        [SerializeField] private GameSettingsData _startGameSettings;

        private AudioSource _audioSource;

        private void Start()
        {
            GameSettings.Settings = _startGameSettings;
            _quitButton.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            });
            _playButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
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