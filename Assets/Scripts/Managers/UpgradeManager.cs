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
        private DamageLevelSettings _damageLevelSettings;
        private HealthLevelSettings _healthLevelSettings;
        private FireRateLevelSettings _fireRateLevelSettings;
        private SpeedLevelSettings _speedLevelSettings;
        private SignalBus _signalBus;

        private UpgradeMenuView _upgradeMenuView;

        [Inject]
        public void Construct(DamageLevelSettings damageLevelSettings,HealthLevelSettings healthLevelSettings,FireRateLevelSettings fireRateLevelSettings,SpeedLevelSettings speedLevelSettings,SignalBus signalBus)
        {
            _damageLevelSettings = damageLevelSettings;
            _healthLevelSettings = healthLevelSettings;
            _fireRateLevelSettings = fireRateLevelSettings;
            _speedLevelSettings = speedLevelSettings;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<OnCloudSavesLoadedSignal>(OnCloudSavesLoaded);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<OnCloudSavesLoadedSignal>(OnCloudSavesLoaded);
        }


        private void Start()
        {
            //SaveManager.SetValue<int>("FireRate", 5);

            _upgradeMenuView = UIManager.GetView<UpgradeMenuView>();
            SetUpgradeItemValues();
        }

        private void SetUpgradeItemValues()
        {
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

                _signalBus.Fire(new OnLevelUpgradedSignal() { cannonLevelSettings = cannonLevelSettings });

                return true;
            }
            else
            {
                UIManager.GetView<WarningView>().CreateWarning("No enough money to Upgrade!");
                return false;
            }
        }

        private void OnCloudSavesLoaded(OnCloudSavesLoadedSignal onCloudSavesLoadedSignal)
        {
            print("SetUpgradeItemValues");
            SetUpgradeItemValues();
        }

    }
}
