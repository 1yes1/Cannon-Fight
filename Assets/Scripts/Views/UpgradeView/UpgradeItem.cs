using CannonFightUI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class UpgradeItem : MonoBehaviour
    {
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _priceText;

        private CannonLevelSettings _cannonLevelSettings;

        public event Action<CannonLevelSettings,Image> OnUpgradeClickEvent;

        public UpgradeType UpgradeType => _upgradeType;


        private void Awake()
        {
            _upgradeButton.onClick.AddListener(() => { OnUpgradeClickEvent?.Invoke(_cannonLevelSettings, _image); });
        }

        public void SetValues(CannonLevelSettings cannonLevelSettings)
        {
            _cannonLevelSettings = cannonLevelSettings;
            _levelText.text = _cannonLevelSettings.Level.ToString();
            _priceText.text = _cannonLevelSettings.Price.ToString();
        }
    }
}
