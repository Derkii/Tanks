using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Managers
{
    public class LeaveFromGameManager : MonoBehaviour
    {
        [SerializeField]
        private float _timeToExitToPauseGameMenu;
        private float _timer;
        private static LeaveFromGameManager _instance;
        private bool _isLeft;
        [Inject] private SoundManager _soundManager;
        private void Start()
        {
            _timer = _timeToExitToPauseGameMenu;
            _instance = this;
        }

        private void Update()
        {
            if (_isLeft) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _soundManager.Play(SoundManager.SoundType.PressESC);
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                _timer = _timeToExitToPauseGameMenu;
            }
            if (_timer <= 0f)
            {
                _soundManager.StartCoroutine(QuitCoroutine());
            }
        }
        private IEnumerator QuitCoroutine()
        {
            _instance._isLeft = true;
            _soundManager.Play(SoundManager.SoundType.LeaveFromGame);
            yield return new WaitForSeconds(_soundManager.GetAudioLength(SoundManager.SoundType.LeaveFromGame));
            SceneManager.LoadScene("MainMenu");
        }
    }
}