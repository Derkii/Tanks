using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tanks
{
    public class GodModeCheat : MonoBehaviour
    {
        private GodModeComponent _playerGodModeComponent;
        private ConditionComponent _playerConditionComponent;
        [SerializeField]
        private TextMeshProUGUI _text;
        private bool _inProcess = false;
        public void Start()
        {
            _playerGodModeComponent = GameObject.Find("Player").GetComponent<GodModeComponent>();
            _playerConditionComponent = _playerGodModeComponent.gameObject.GetComponent<ConditionComponent>();
        }
        public void GodMode_UEE()
        {
            StartCoroutine(GodMode());
        }

        private IEnumerator GodMode()
        {
            if (_inProcess == false)
            {
                _inProcess = true;
                if (_playerGodModeComponent.IsGodmode == false)
                {
                    _playerGodModeComponent.GodMode(true);
                    _text.text = "Disable GodMode";
                }
                else
                {
                    _text.text = "Enable GodMode";
                    yield return new WaitForSeconds(_playerConditionComponent.GodModeTime);
                    _playerGodModeComponent.GodMode(false);
                }
                _inProcess = false;
            }
        }
    }
}
