using CannonFightUI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CannonFightBase
{
    public class UpgradedSubView : UISubView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private GameObject _valueNameCouple;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Button _continueButton;

        private AnimationSettings _animationSettings;

        [Inject]
        private void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }

        public override void Initialize()
        {
            _continueButton.onClick.AddListener(Hide);
        }

        public override void Show(float subViewDelay)
        {
            base.Show(subViewDelay);

            _image.transform.localScale = Vector3.zero;
            _levelText.transform.localScale = Vector3.zero;
            _continueButton.transform.localScale = Vector3.zero;
            _valueNameCouple.transform.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence();
            
            Tweener image = _image.transform.DOScale(_animationSettings.Image.Value,_animationSettings.Image.Duration).SetEase(_animationSettings.Image.Ease);
            Tweener levelText = _levelText.transform.DOScale(_animationSettings.LevelText.Value, _animationSettings.LevelText.Duration).SetEase(_animationSettings.LevelText.Ease).SetDelay(_animationSettings.LevelText.Delay);
            Tweener valueTextCouple = _valueNameCouple.transform.DOScale(_animationSettings.ValueNameCouple.Value, _animationSettings.ValueNameCouple.Duration).SetEase(_animationSettings.ValueNameCouple.Ease)
                .SetDelay(_animationSettings.ValueNameCouple.Delay);
            Tweener continueButton = _continueButton.transform.DOScale(_animationSettings.ContinueButton.Value, _animationSettings.ContinueButton.Duration).SetEase(_animationSettings.ContinueButton.Ease)
                .SetDelay(_animationSettings.ContinueButton.Delay);

            sequence.Append(image);
            sequence.Append(levelText);
            sequence.Append(valueTextCouple);
            sequence.Append(continueButton);
        }

        internal void UpdateValues(CannonLevelSettings cannonLevelSettings,Image image)
        {
            _image.sprite = image.sprite;
            _levelText.text = "Level " + cannonLevelSettings.Level;
            _valueText.text = cannonLevelSettings.CurrentValue.ToString();
            _nameText.text = cannonLevelSettings.UpgradeTypeTextFormat;
        }

        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings Image;
            public TweenSettings LevelText;
            public TweenSettings ValueNameCouple;
            public TweenSettings ContinueButton;
        }
    }
}
