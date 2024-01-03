using CannonFightExtensions;
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
    public class RankSubView : UISubView
    {
        [SerializeField] private GameObject _background;
        [SerializeField] private TextMeshProUGUI _rankText;

        private AnimationSettings _animationSettings;

        private string _rank;


        [Inject]
        private void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }

        public override void Initialize()
        {
        }


        public override void Show(float subViewDelay)
        {
            _delay = subViewDelay;

            _background.GetComponent<Image>().DOFade(0, 0);
            _background.GetComponent<RectTransform>().DOAnchorPosX(-_animationSettings.Background.Value, 0);
            _rankText.transform.localScale = Vector3.zero;

            _background.GetComponent<RectTransform>().DOAnchorPosX(_animationSettings.Background.Value, _animationSettings.Background.Duration).SetEase(_animationSettings.Background.Ease)
                .SetDelay(_delay + _animationSettings.Background.Delay);

            _background.GetComponent<Image>().DOFade(1, _animationSettings.ShowBackgroundFadeDuration).SetDelay(_animationSettings.ShowBackgroundFadeDelay);

            _rankText.transform.DOScale(Vector3.one,_animationSettings.RankText.Duration).SetEase(_animationSettings.RankText.Ease)
                .SetDelay(_delay+_animationSettings.RankText.Delay);
        }

        void Start()
        {
            _rank = "You are #" + 6;
        }

        void Update()
        {
        
        }


        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings Background;
            public float ShowBackgroundFadeDelay;
            public float ShowBackgroundFadeDuration;
            
            [Space(10)]

            public TweenSettings RankText;
        }

        //[Serializable]
        //public partial struct BackgroundTweenSettings
        //{
        //    public TweenSettings Background;
        //    public float StartValue;
        //}

    }



}
