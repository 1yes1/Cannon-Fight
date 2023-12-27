using CannonFightBase;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CannonLevelSettingsInstaller", menuName = "CannonFight/CannonLevelSettingsInstaller", order = 5)]
public class CannonLevelSettingsInstaller : ScriptableObjectInstaller<CannonLevelSettingsInstaller>
{
    public CannonLevelSettings.DamageLevelSettings DamageLevelSettings;

    public CannonLevelSettings.FireRateLevelSettings FireRateLevelSettings;

    public CannonLevelSettings.HealthLevelSettings HealthLevelSettings;

    public CannonLevelSettings.SpeedLevelSettings SpeedLevelSettings;


    public override void InstallBindings()
    {
        Container.BindInstance(DamageLevelSettings);
        Container.BindInstance(FireRateLevelSettings);
        Container.BindInstance(HealthLevelSettings);
        Container.BindInstance(SpeedLevelSettings);
    }
}