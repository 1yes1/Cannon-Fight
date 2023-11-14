using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static CannonFightUI.VictoryPanelView;

namespace CannonFightUI
{
    public class ShineSubView : UISubView
    {
        [SerializeField] private GameObject _shine;
        [SerializeField] private GameObject _radialShine;

        private CanvasGroup _canvasGroup;
        private AnimationSettings _animationSettings;


        [Inject]
        private void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }

        public override void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Show(float subViewDelay)
        {

            _delay = subViewDelay;

            Tween scaleUp = _shine.transform.DOScale(_animationSettings.ShineScaleUp.Value, _animationSettings.ShineScaleUp.Duration)
                .SetEase(_animationSettings.ShineScaleUp.Ease)
                .SetDelay(_delay + _animationSettings.ShineScaleUp.Delay);


            Tween scaleDown = _shine.transform.DOScale(_animationSettings.ShineScaleDown.Value, _animationSettings.ShineScaleDown.Duration)
                .SetEase(_animationSettings.ShineScaleDown.Ease)
                .SetDelay(_animationSettings.ShineScaleDown.Delay);

            Tween fadeOut = _canvasGroup.DOFade(_animationSettings.ShineFade.Value, _animationSettings.ShineFade.Duration)
                .SetDelay(_animationSettings.ShineFade.Delay);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(scaleUp);
            sequence.Append(scaleDown);
            sequence.Join(fadeOut);

            _shine.transform.DOLocalRotate(_animationSettings.ShineRotate.Vector, _animationSettings.ShineRotate.Duration, RotateMode.FastBeyond360)
                .SetRelative()
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);

            _radialShine.transform.DOScale(_animationSettings.RadialShine.Vector, _animationSettings.RadialShine.Duration)
                .SetEase(_animationSettings.RadialShine.Ease)
                .SetDelay(_animationSettings.RadialShine.Delay);
        }


        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings ShineScaleUp;

            public TweenSettings ShineScaleDown;

            public TweenSettings ShineRotate;

            public TweenSettings ShineFade;

            public TweenSettings RadialShine;

        }

    }
}
