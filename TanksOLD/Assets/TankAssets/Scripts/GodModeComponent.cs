using System.Collections;
using UnityEngine;

namespace Tanks
{
    [RequireComponent(typeof(SpriteRenderer), typeof(ConditionComponent))]
    public class GodModeComponent : MonoBehaviour
    {
        private bool _useGodMode = false;
        private SpriteRenderer _spriteRenderer;
        private ConditionComponent _conditionComponent;
        private Color _startColor;
        private int _startHealth;
        private float _currentTime;
        public bool IsGodmode => _useGodMode;
        private Coroutine _lastCoroutine;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startColor = _spriteRenderer.color;
            _conditionComponent = GetComponent<ConditionComponent>();

        }
        private void Update()
        {
            _currentTime += Time.deltaTime;
        }
        private IEnumerator GodModeCoroutine(float time)
        {
            if (_conditionComponent.Healh != int.MaxValue)
            {
                _startHealth = _conditionComponent.Healh;
            }
            _currentTime = 0f;
            while (_currentTime < time)
            {
                if (_useGodMode)
                {
                    _spriteRenderer.color = Color.clear;
                    _conditionComponent.SetHealth(int.MaxValue);
                    yield return new WaitForSeconds(0.1f);
                    _spriteRenderer.color = _startColor;
                    _conditionComponent.SetHealth(int.MaxValue);
                    yield return new WaitForSeconds(0.1f);
                    yield return null;
                }
                else
                {
                    break;
                }
                yield return null;
            }
            _spriteRenderer.color = _startColor;
            _useGodMode = false;
            _conditionComponent.SetHealth(_startHealth);
            SoundManager.instance.Play(SoundManager.SoundType.GodMode);
        }

        public void GodMode(bool on)
        {
            GodMode(on, float.MaxValue);
        }
        public void GodMode(bool on, float time)
        {
            if (on == true)
            {
                SoundManager.instance.Play(SoundManager.SoundType.GodMode);
            }
            _useGodMode = on;
            _lastCoroutine = StartCoroutine(GodModeCoroutine(time));
        }
    }
}