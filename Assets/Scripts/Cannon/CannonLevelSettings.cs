using CannonFightUI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CannonFightBase.CoinManager;
using static Cinemachine.DocumentationSortingAttribute;

namespace CannonFightBase
{
    [Serializable]
    public class CannonLevelSettings
    {
        [SerializeField] private UpgradeType _upgradeType;

        [Header("Value")]
        [SerializeField] private float _startValue;
        [SerializeField] private float _valueMultiplierPerLevel;

        [Header("Price")]
        [SerializeField] private int _startPrice;
        [SerializeField] private float _priceMultiplierPerLevel;

        public UpgradeType UpgradeType => _upgradeType;

        public string UpgradeTypeTextFormat => string.Concat(_upgradeType.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

        public int Level => CloudSaveManager.GetValue<int>(_upgradeType.ToString(), 1);

        public int Price => _startPrice + Mathf.FloorToInt((Level - 1) * _priceMultiplierPerLevel);

        public float CurrentValue => _startValue + ((Level-1) * _valueMultiplierPerLevel);

        public float AddValueForNextLevel => _valueMultiplierPerLevel;

        public void Upgrade()
        {
            CloudSaveManager.SetValue<int>(_upgradeType.ToString(), Level + 1);
        }

        public void SetDefaultValue()
        {
            CloudSaveManager.SetValue<int>(_upgradeType.ToString(), 1);
        }

        public void SetLevel(int level)
        {
            CloudSaveManager.SetValue<int>(_upgradeType.ToString(), level);
        }

        [Serializable]
        public class DamageLevelSettings : CannonLevelSettings
        {
        }

        [Serializable]
        public class FireRateLevelSettings : CannonLevelSettings
        {
        }

        [Serializable]
        public class HealthLevelSettings: CannonLevelSettings
        {
        }

        [Serializable]
        public class SpeedLevelSettings : CannonLevelSettings
        {
        }

    }
}
