using AYellowpaper.SerializedCollections;
using CannonFightBase;
using CannonFightExtensions;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CannonFightUI
{
    public class UpgradeMenuView : UIView
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private UpgradePopupSubView _upgradePopupSubView;
        [SerializeField] private UpgradedSubView _upgradedSubView;

        [SerializedDictionary("Upgrade Type","Upgrade Item")] [SerializeField] private SerializedDictionary<UpgradeType, UpgradeItem> _upgradeItems;

        private BaseViewAnimationSettings _animationSettings;
        private UpgradeManager _upgradeManager;

        [Inject]
        private void Construct(UpgradeManager upgradeManager, BaseViewAnimationSettings animationSettings)
        {
            _upgradeManager = upgradeManager;
            _animationSettings = animationSettings;
        }

        public override void Initialize()
        {
            _upgradePopupSubView.Initialize();
            _upgradedSubView.Initialize();
            _upgradePopupSubView.OnClickUpgradeEvent += OnClickUpgrade;
            _backButton.onClick.AddListener(() =>{ UIManager.ShowLast(); });
        }

        public override void Show()
        {
            base.Show();

            transform.localScale = Vector3.zero;

            transform.DOScale(_animationSettings.Main.Value, _animationSettings.Main.Duration).SetEase(_animationSettings.Main.Ease).SetDelay(_animationSettings.Main.Delay);
        }

        public void SetUpgradeItemValues(UpgradeType upgradeType,CannonLevelSettings cannonLevelSettings)
        {
            UpgradeItem upgradeItem = null;
            if (_upgradeItems.TryGetValue(upgradeType,out upgradeItem))
            {
                upgradeItem.SetValues(cannonLevelSettings);
                upgradeItem.OnUpgradeClickEvent -= OnUpgradeItemOnClickUpgrade;
                upgradeItem.OnUpgradeClickEvent += OnUpgradeItemOnClickUpgrade;
            }
            else
                Debug.LogWarning("There is no key in UpgradeItems: " + upgradeType);
        }

        public void OnUpgradeItemOnClickUpgrade(CannonLevelSettings cannonLevelSettings,Image image)
        {
            _upgradePopupSubView.Show(0);

            _upgradePopupSubView.UpdateValues(cannonLevelSettings, image);
        }

        public void OnClickUpgrade(CannonLevelSettings cannonLevelSettings,Image image)
        {
            if (_upgradeManager.TryUpgradeLevel(cannonLevelSettings))
            {
                OnUpgrade(cannonLevelSettings);
                _upgradedSubView.Show(0);
                _upgradedSubView.UpdateValues(cannonLevelSettings, image);
            }
        }

        private void OnUpgrade(CannonLevelSettings cannonLevelSettings)
        {
            _upgradePopupSubView.Hide();
            SetUpgradeItemValues(cannonLevelSettings.UpgradeType, cannonLevelSettings);
        }

        [Serializable]
        public struct BaseViewAnimationSettings
        {
            public TweenSettings Main;
        }

        [Serializable]
        public struct AnimationSettings
        {
            public BaseViewAnimationSettings UpgradeMenuView;
            public UpgradePopupSubView.AnimationSettings UpgradePopupSubView;
            public UpgradedSubView.AnimationSettings UpgradedSubView;
        }

    }
}
