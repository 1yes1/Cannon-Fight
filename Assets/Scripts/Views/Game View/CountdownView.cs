using CannonFightExtensions;
using CannonFightUI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CountdownView : UIView
    {
        [SerializeField] private TextMeshProUGUI _countdownText;

        private AnimationSettings _animationSettings;

        private float _countdown = 0;

        [Inject]
        public void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }

        public float Countdown
        {
            get { return (int)_countdown; }
            set
            {
                _countdown = value;
            }
        }

        public override void Initialize()
        {
        }

        public void StartCountdown(int time)
        {
            _countdown = time;
            StartCoroutine(IECountdown());

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_countdownText.transform.DOScale(_animationSettings.CountdownFadeIn,true));
            sequence.Append(_countdownText.DOFade(_animationSettings.CountdownFadeOut.Value, _animationSettings.CountdownFadeOut.Duration).SetDelayAndEase(_animationSettings.CountdownFadeOut));
            sequence.SetLoops(time + 1);

        }

        private IEnumerator IECountdown()
        {
            while (_countdown > 0)
            {
                _countdown -= Time.deltaTime;

                _countdownText.text = (Mathf.CeilToInt(_countdown)).ToString();

                if ((Mathf.CeilToInt(_countdown)) == 0)
                    _countdownText.text = "FIGHT";

                yield return null;
            }

            yield return new WaitForSeconds(1f);

            Hide();
        }

        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings CountdownFadeIn;
            public TweenSettings CountdownFadeOut;
        }

    }
}
