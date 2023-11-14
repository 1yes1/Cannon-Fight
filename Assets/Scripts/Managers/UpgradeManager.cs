using CannonFightBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static CannonFightBase.CannonLevelSettings;

namespace CannonFightUI
{
    public enum UpgradeType
    {
        Damage,
        Speed,
        FireRate,
        Health
    }

    public class UpgradeManager : MonoBehaviour
    {
        public event Action<CannonLevelSettings> OnUpgradeEvent;
        private DamageLevelSettings _damageLevelSettings;
        private HealthLevelSettings _healthLevelSettings;
        private FireRateLevelSettings _fireRateLevelSettings;
        private SpeedLevelSettings _speedLevelSettings;

        private UpgradeMenuView _upgradeMenuView;

        [Inject]
        public void Construct(DamageLevelSettings damageLevelSettings,HealthLevelSettings healthLevelSettings,FireRateLevelSettings fireRateLevelSettings,SpeedLevelSettings speedLevelSettings)
        {
            _damageLevelSettings = damageLevelSettings;
            _healthLevelSettings = healthLevelSettings;
            _fireRateLevelSettings = fireRateLevelSettings;
            _speedLevelSettings = speedLevelSettings;
        }

        private void Awake()
        {
        }

        private void Start()
        {

            //SaveManager.SetValue<int>("FireRate", 5);

            _upgradeMenuView = UIManager.GetView<UpgradeMenuView>();

            _upgradeMenuView.SetUpgradeItemValues(UpgradeType.Damage, _damageLevelSettings);
            _upgradeMenuView.SetUpgradeItemValues(UpgradeType.FireRate, _fireRateLevelSettings);
            _upgradeMenuView.SetUpgradeItemValues(UpgradeType.Speed, _speedLevelSettings);
            _upgradeMenuView.SetUpgradeItemValues(UpgradeType.Health, _healthLevelSettings);
        }

        public bool TryUpgradeLevel(CannonLevelSettings cannonLevelSettings)
        {
            if(CoinManager.CurrentCoin >= cannonLevelSettings.Price )
            {
                CoinManager.CurrentCoin -= cannonLevelSettings.Price;
                cannonLevelSettings.Upgrade();
                OnUpgradeEvent?.Invoke(cannonLevelSettings);
                return true;
            }
            else
            {
                UIManager.GetView<WarningView>().CreateWarning("No enough money to Upgrade!");
                return false;
            }
        }
    }
}
