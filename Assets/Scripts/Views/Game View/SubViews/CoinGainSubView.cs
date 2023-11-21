using CannonFightBase;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CannonFightUI
{
    public class CoinGainSubView : UISubView
    {
        [SerializeField] private GameObject _background;
        [SerializeField] private GameObject _coin;
        [SerializeField] private TextMeshProUGUI _coinText;

        private AnimationSettings _animationSettings;

        private int _coinCount = 480;

        [Inject]
        private void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }


        public override void Show(float subViewDelay)
        {
            _coinText.text = "";
            _delay = subViewDelay;

            Vector3 coinScale = _coin.transform.localScale;
            _coin.transform.localScale = Vector3.zero;
            _background.transform.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence(this);

            Tweener backgroundAnimation = _background.transform.DOScale(_animationSettings.Background.Value,_animationSettings.Background.Duration).SetEase(_animationSettings.Background.Ease).SetDelay(_animationSettings.Background.Delay);

            Tweener coinAnimation = _coin.transform.DOScale(_animationSettings.Coin.Vector, _animationSettings.Coin.Duration).SetEase(_animationSettings.Coin.Ease).SetDelay(_animationSettings.Coin.Delay).OnComplete(SetCoin);
            sequence.Append(backgroundAnimation);
            sequence.Append(coinAnimation);
            sequence.Play().SetDelay(_delay);
        }


        public override void Initialize()
        {
        }


        private void SetCoin()
        {
            _coinText.text = "+0";
            _coinText.transform.localScale = Vector3.zero;

            _coinText.transform.DOScale(Vector3.one, _animationSettings.CoinText.Duration).SetEase(_animationSettings.CoinText.Ease);
            StartCoroutine(SetCoinCount());
        }


        private IEnumerator SetCoinCount()
        {
            yield return new WaitForSeconds(_animationSettings.CoinText.Delay);

            float time = 0;
            float timeCount = _animationSettings.CoinText.Value;
            float text;

            while (time < timeCount)
            {

                text = (int)Mathf.Lerp(0, _coinCount, time / timeCount);
                time += Time.deltaTime;

                _coinText.text = "+"+ text.ToString();
                yield return null;
            }

            _coinText.text = "+" + _coinCount.ToString();
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    _tweener.Complete();
            //}
        }

        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings Background;
            public TweenSettings Coin;
            public TweenSettings CoinText;
        }

    }
}
