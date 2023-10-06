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
        [SerializeField] private TextMeshProUGUI _countdownText;

        [SerializeField] private float _countdown = 3;

        private event Action _onCountdownFinishedAction;

        public override void Initialize()
        {

        }

        public void StartCountdown(float countdown,Action OnCountdownFinishedAction)
        {
            _onCountdownFinishedAction = OnCountdownFinishedAction;
            StartCoroutine(Countdown(countdown));
        }

        private IEnumerator Countdown(float countdown)
        {
            float time = countdown;
            while (time > 0)
            {
                _countdownText.text = Mathf.Ceil(time).ToString();
                time -= Time.deltaTime;

                if (time <= 0)
                    CountdownFinished();

                yield return null;
            }

        }

        private void CountdownFinished()
        {
            _onCountdownFinishedAction();
        }

    }
}
