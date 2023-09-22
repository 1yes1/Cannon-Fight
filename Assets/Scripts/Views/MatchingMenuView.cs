using CannonFightUI;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannonFightBase
{
    public class MatchingMenuView : UIView
    {
        private event Action _onCountdownFinishedAction; 
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private float _countdown = 3;

        private bool _isCountdownStarted = false;

        public override void Initialize()
        {

        }

        private void Update()
        {
            if (_isCountdownStarted && _countdown > 0)
            {
                _countdown -= Time.deltaTime;
                _countdownText.text = Mathf.Ceil(_countdown).ToString();
                if (_countdown <= 0)
                    CountdownFinished();
            }
        }

        public void StartCountdown(Action OnCountdownFinishedAction)
        {
            _onCountdownFinishedAction = OnCountdownFinishedAction;
            _isCountdownStarted =true;
        }

        private void CountdownFinished()
        {
            _onCountdownFinishedAction();
            _isCountdownStarted = false;
        }

    }
}
