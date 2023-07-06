using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LeaveFromGameManager : MonoBehaviour
    {
        [SerializeField]
        private float _timeToExitToPauseGameMenu;
        private float _timer;
        private static LeaveFromGameManager _instance;
        private bool _isLeft;

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
                SoundManager.instance.Play(SoundManager.SoundType.PressESC);
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
                SoundManager.instance.StartCoroutine(QuitCoroutine());
            }
        }

        public static void Quit()
        {
            SoundManager.instance.StartCoroutine(QuitCoroutine());
        }

        private static IEnumerator QuitCoroutine()
        {
            _instance._isLeft = true;
            SoundManager.instance.Play(SoundManager.SoundType.LeaveFromGame);
            yield return new WaitForSeconds(SoundManager.instance.GetAudioLength(SoundManager.SoundType.LeaveFromGame));
            SceneManager.LoadScene("MainMenu");
        }
    }
}