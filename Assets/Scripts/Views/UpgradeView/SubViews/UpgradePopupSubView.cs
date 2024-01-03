using CannonFightBase;
using CannonFightExtensions;
using CannonFightUI;
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
    public class UpgradePopupSubView : UISubView
    {
        public event Action<CannonLevelSettings,Image> OnClickUpgradeEvent;

        [SerializeField] private Button _upgradeButton;

        [SerializeField] private Button _closeButton;

        [SerializeField] private Image _focus;
        [SerializeField] private Image _popupBackground;

        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _upgradeTypeText;
        [SerializeField] private TextMeshProUGUI _currentValueText;
        [SerializeField] private TextMeshProUGUI _plusValueText;
        [SerializeField] private TextMeshProUGUI _priceText;
        
        private AnimationSettings _animationSettings;
        private CannonLevelSettings _cannonLevelSettings;

        [Inject]
        private void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }

        public override void Initialize()
        {
            _closeButton.onClick.AddListener(Hide);
            _upgradeButton.onClick.AddListener(Upgrade);
        }

        public override void Show(float subViewDelay)
        {
            _focus.DOFade(0, 0);
            _popupBackground.transform.localScale = Vector3.zero;
            gameObject.SetActive(true);

            _focus.DOFade(_animationSettings.Focus.Value, _animationSettings.Focus.Duration).SetEase(_animationSettings.Focus.Ease).SetDelay(_animationSettings.Focus.Delay);
            _popupBackground.transform.DOScale(_animationSettings.Popup.Vector, _animationSettings.Popup.Duration).SetEase(_animationSettings.Popup.Ease).SetDelay(_animationSettings.Popup.Delay);
        }

        public void UpdateValues(CannonLevelSettings cannonLevelSettings,Image image)
        {
            _cannonLevelSettings = cannonLevelSettings;
            _levelText.text = "Level "+ cannonLevelSettings.Level.ToString();
            _upgradeTypeText.text = cannonLevelSettings.UpgradeTypeTextFormat;
            _currentValueText.text = (Mathf.Abs(cannonLevelSettings.CurrentValue)).ToString();
            _plusValueText.text = (cannonLevelSettings.AddValueForNextLevel > 0) ? "+"+ cannonLevelSettings.AddValueForNextLevel.ToString() : cannonLevelSettings.AddValueForNextLevel.ToString();
            _priceText.text = cannonLevelSettings.Price.ToString();
            _image.sprite = image.sprite;
        }

        public void Upgrade()
        {
            OnClickUpgradeEvent.Invoke(_cannonLevelSettings,_image);
        }

        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings Focus;
            public TweenSettings Popup;
        }

    }
}
