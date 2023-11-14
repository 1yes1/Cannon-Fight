using CannonFightUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
using UnityEngine.UI;

namespace CannonFightUI
{
    public class VictoryPanelView : UIView
    {
        private BaseViewAnimationSettings _animationSettings;

        [SerializeField] private KillCountSubView _killCountSubView;
        [SerializeField] private CoinGainSubView _coinGainSubView;
        [SerializeField] private RankSubView _rankSubView;
        [SerializeField] private ShineSubView _shineSubView;
        [SerializeField] private Button _exitButton;

        [Inject]
        private void Construct(BaseViewAnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }


        private void Awake()
        {

        }

        public override void Initialize()
        {
            _killCountSubView.Initialize();
            _coinGainSubView.Initialize();
            _rankSubView.Initialize();
            _shineSubView.Initialize();
        }

        public override void Show()
        {
            base.Show();

            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, _animationSettings.Panel.Duration).SetEase(_animationSettings.Panel.Ease);
            _exitButton.transform.localScale = Vector3.zero;

            _killCountSubView.Show(_animationSettings.KillCountSubViewDelay);
            _coinGainSubView.Show(_animationSettings.CoinGainSubViewDelay);
            _rankSubView.Show(_animationSettings.RankSubViewDelay);
            _shineSubView.Show(_animationSettings.ShineSubViewDelay);

            _exitButton.transform.DOScale(_animationSettings.ExitButton.Value, _animationSettings.ExitButton.Duration).SetEase(_animationSettings.ExitButton.Ease).SetDelay(_animationSettings.ExitButton.Delay);
        }


        [Serializable]
        public struct BaseViewAnimationSettings
        {
            [Header("Sub View Delays")]
            public float KillCountSubViewDelay;
            public float CoinGainSubViewDelay;
            public float RankSubViewDelay;
            public float ShineSubViewDelay;

            public TweenSettings Panel;

            public TweenSettings ExitButton;
        }

        [Serializable]
        public struct AnimationSettings
        {
            public BaseViewAnimationSettings VictoryPanelView;
            [Space(10)]
            public KillCountSubView.AnimationSettings KillCountSubView;
            [Space(10)]
            public CoinGainSubView.AnimationSettings CoinGainSubView;
            [Space(10)]
            public RankSubView.AnimationSettings RankSubView;
            [Space(10)]
            public ShineSubView.AnimationSettings ShineSubView;
        }

    }
}
