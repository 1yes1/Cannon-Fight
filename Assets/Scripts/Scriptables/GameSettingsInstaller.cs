using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "CannonBallSettingsInstaller", menuName = "CannonFight/CannonBallSettingsInstaller", order = 1)]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public GameInstaller.Settings Prefabs;
        public FireController.Settings FireControllerSettings;
        public AimController.Settings AimControllerSettings;
        public CannonSkillHandler.Settings CannonSkillHandlerSettings;
        public Chest.Settings ChestSettings;
        public Ramp.Settings RampSettings;
        public Potion.Settings PotionSettings;

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
