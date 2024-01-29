using CannonFightBase;
using CannonFightExtensions;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightUI
{
    public class MenuLoadingView : UIView
    {
        private AnimationSettings _animationSettings;

        private CanvasGroup _canvasGroup;

        [Inject]
        public void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }

        public override void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void AddSubViews()
        {
        }

        public override void Show()
        {
            base.Show();
            _canvasGroup.alpha = 1;
        }

        public override void Hide()
        {
            _canvasGroup.DOFade(_animationSettings.FadeOut).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings FadeOut;
        }

    }
}
