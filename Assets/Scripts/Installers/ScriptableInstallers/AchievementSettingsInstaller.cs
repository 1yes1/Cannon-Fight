using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "AchievementSettingsInstaller", menuName = "CannonFight/AchievementSettingsInstaller", order = 7)]
    public class AchievementSettingsInstaller : ScriptableObjectInstaller<AchievementSettingsInstaller>
    {

        [Header("Achievement Settings")]
        [Space(1)]
        public AchievementManager.Settings AchievementManagerSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(AchievementManagerSettings.GameEndSettings);
        }
    }
}
