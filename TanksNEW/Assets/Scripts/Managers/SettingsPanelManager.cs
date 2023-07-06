using Game.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class SettingsPanelManager : MonoBehaviour
    {
        [SerializeField]
        private Slider _botHealthSlider, _playerHealthSlider, _soundVolumeSlider, _maxBotsCountSlider;
        [SerializeField]
        private Button _quitFromSettings;
        [SerializeField]
        private TextMeshProUGUI _botHealthText, _playerHealthText, _soundVolumeText, _maxBotsCountText;

        private void Start()
        {
            _botHealthSlider.value = GameSettings.Settings.BotHealth;
            _playerHealthSlider.value = GameSettings.Settings.PlayerHealth;
            _maxBotsCountSlider.value = GameSettings.Settings.MaxBotsCount;
            _soundVolumeSlider.value = GameSettings.Settings.Volume;

            _botHealthSlider.onValueChanged.AddListener((float value) => GameSettings.Settings.BotHealth = (int)value);
            _playerHealthSlider.onValueChanged.AddListener((float value) => GameSettings.Settings.PlayerHealth = (int)value);
            _soundVolumeSlider.onValueChanged.AddListener((float value) => GameSettings.Settings.Volume = value);
            _maxBotsCountSlider.onValueChanged.AddListener((float value) => GameSettings.Settings.MaxBotsCount = (int)value);

            _quitFromSettings.onClick.AddListener(() => gameObject.SetActive(false));
        }
        private void FixedUpdate()
        {
            _botHealthText.text = _botHealthSlider.value.ToString();
            _playerHealthText.text = _playerHealthSlider.value.ToString();
            _maxBotsCountText.text = _maxBotsCountSlider.value.ToString();
            _soundVolumeText.text = _soundVolumeSlider.value.ToString();
        }
    }
}
