using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "CannonBallSettingsInstaller", menuName = "CannonFight/CannonBallSettingsInstaller", order = 1)]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [Header("Prefabs")]
        [Space(1)]
        public GameInstaller.Settings Prefabs;
        
        [Header("Controllers")]
        [Space(1)]
        public FireController.Settings FireControllerSettings;
        public AimController.Settings AimControllerSettings;
        public CannonSkillHandler.Settings CannonSkillHandlerSettings;

        [Header("Level Settings")]
        [Space(1)]
        public Chest.Settings ChestSettings;
        public Ramp.Settings RampSettings;
        public Potion.Settings PotionSettings;

        [Header("Achievement Settings")]
        [Space(1)]
        public AchievementManager.Settings AchievementManagerSettings;


        public override void InstallBindings()
        {
            Container.BindInstance(Prefabs);
            Container.BindInstance(FireControllerSettings);
            Container.BindInstance(AimControllerSettings);
            Container.BindInstance(CannonSkillHandlerSettings);
            Container.BindInstance(ChestSettings);
            Container.BindInstance(RampSettings);
            Container.BindInstance(PotionSettings);
        }
    }
}
