using CannonFightUI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CannonFightUI
{
    public class DefeatedPanelView : UIView
    {
        [SerializeField] private KillCountSubView _killCountSubView;
        [SerializeField] private RankSubView _rankSubView;
        [SerializeField] private CoinGainSubView _coinGainSubView;
        [SerializeField] private Button _exitButton;

        private BaseViewAnimationSettings _animationSettings;


        [Inject]
        private void Construct(BaseViewAnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }

        public override void Initialize()
        {
            _killCountSubView.Initialize();
            _coinGainSubView.Initialize();
            _rankSubView.Initialize();
        }

        public override void Show()
        {
            base.Show();

            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, _animationSettings.Panel.Duration).SetEase(_animationSettings.Panel.Ease);
            _exitButton.transform.localScale = Vector3.zero;

            _killCountSubView.Show(_animationSettings.KillCountSubViewDelay);
            _rankSubView.Show(_animationSettings.RankSubViewDelay);
            _coinGainSubView.Show(_animationSettings.CoinGainSubViewDelay);
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
            public BaseViewAnimationSettings DefeatedPanelView;
        }

    }

}