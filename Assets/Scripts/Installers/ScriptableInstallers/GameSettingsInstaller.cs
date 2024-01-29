using System;
using UnityEngine;
using Zenject;
using static CannonFightBase.AIManager;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "CannonBallSettingsInstaller", menuName = "CannonFight/CannonBallSettingsInstaller", order = 1)]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [Header("Prefabs")]
        [Space(1)]
        public PrefabSettings Prefabs;
        public UIPrefabSettings UIPrefabs;
        
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


        [Header("GameAgentSettings")]
        [Space(1)]
        public GameAgentSettings GameAgentSettings;


        public override void InstallBindings()
        {
            Container.BindInstance(Prefabs);
            Container.BindInstance(FireControllerSettings);
            Container.BindInstance(AimControllerSettings);
            Container.BindInstance(CannonSkillHandlerSettings);
            Container.BindInstance(ChestSettings);
            Container.BindInstance(RampSettings);
            Container.BindInstance(PotionSettings);
            Container.BindInstance(UIPrefabs.PlayerItem);
            Container.BindInstance(GameAgentSettings);
        }



        [Serializable]
        public class PrefabSettings
        {
            public Cannon CannonPrefab;
            public Agent AgentPrefab;
            public AgentManager AgentManagerPrefab;
            public CannonManager PlayerManagerPrefab;
            public CannonBall CannonBallPrefab;
            public Potion Potion;
        }

        [Serializable]
        public struct UIPrefabSettings
        {
            public PlayerItem PlayerItem;
        }
    }
}
