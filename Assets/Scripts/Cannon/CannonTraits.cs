using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class CannonTraits
    {
        private readonly CannonLevelSettings.DamageLevelSettings _damageLevelSettings;
        private readonly CannonLevelSettings.FireRateLevelSettings _fireRateLevelSettings;
        private readonly CannonLevelSettings.HealthLevelSettings _healthLevelSettings;
        private readonly CannonLevelSettings.SpeedLevelSettings _speedLevelSettings;

        public float FireRate => _fireRateLevelSettings.CurrentValue;

        public int Damage => (int)_damageLevelSettings.CurrentValue;

        public int Health => (int)_healthLevelSettings.CurrentValue;

        public float Speed => _speedLevelSettings.CurrentValue;

        public CannonTraits(
            CannonLevelSettings.DamageLevelSettings damageLevelSettings,
            CannonLevelSettings.FireRateLevelSettings fireRateLevelSettings,
            CannonLevelSettings.HealthLevelSettings healthLevelSettings,
            CannonLevelSettings.SpeedLevelSettings speedLevelSettings)
        {
            _damageLevelSettings = damageLevelSettings;
            _fireRateLevelSettings = fireRateLevelSettings;
            _healthLevelSettings = healthLevelSettings;
            _speedLevelSettings = speedLevelSettings;
        }
    }
}
