using CannonFightBase;
using CannonFightExtensions;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace CannonFightUI
{
    public class WarningView : UIView
    {
        [SerializeField] private TextMeshProUGUI _warningText;

        private BaseViewAnimationSettings _animationSettings;
        private Sequence _warningSequence;


        [Inject]
        private void Construct(BaseViewAnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }


        public override void Initialize()
        {

        }

        public override void Show()
        {
            base.Show();

            transform.localScale = Vector3.zero;
            _warningText.DOFade(1,0);

            _warningSequence = DOTween.Sequence();

            Tweener main = transform.DOScale(_animationSettings.WarningView.Value, _animationSettings.WarningView.Duration).SetEase(_animationSettings.WarningView.Ease).SetDelay(_animationSettings.WarningView.Delay);
            Tweener text = _warningText.DOFade(_animationSettings.WarningText.Value, _animationSettings.WarningText.Duration).SetEase(_animationSettings.WarningText.Ease).SetDelay(_animationSettings.WarningText.Delay);
            _warningSequence.Append(main);
            _warningSequence.Append(text);

            _warningSequence.OnComplete(Hide);
        }

        public void CreateWarning(string text)
        {
            _warningSequence.Complete();
            _warningText.text = text;

            Show();
        }

        public override void Hide()
        {
            base.Hide();
            _warningText.text = "";
        }

        [Serializable]
        public struct BaseViewAnimationSettings
        {
            public TweenSettings WarningView;
            public TweenSettings WarningText;
        }


        [Serializable]
        public struct AnimationSettings
        {
            public BaseViewAnimationSettings WarningView;
        }
    }
}
