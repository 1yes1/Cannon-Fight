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
        private SignalBus _signalBus;

        private AnimationSettings _animationSettings;

        private CanvasGroup _canvasGroup;

        [Inject]
        public void Construct(AnimationSettings animationSettings,SignalBus signalBus)
        {
            _animationSettings = animationSettings;
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            _signalBus.Subscribe<OnCloudSavesLoadedSignal>(Hide);
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Hide()
        {
            _canvasGroup.DOFade(_animationSettings.FadeOut).OnComplete(() =>
            {
                gameObject.SetActive(false);
                UIManager.Show<MainMenuView>();
            });
        }

        public struct AnimationSettings
        {
            public TweenSettings FadeOut;
        }

    }
}
