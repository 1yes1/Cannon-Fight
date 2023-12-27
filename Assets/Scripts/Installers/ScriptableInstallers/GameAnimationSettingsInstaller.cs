using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "GameAnimationSettingsInstaller", menuName = "CannonFight/GameAnimationSettingsInstaller")]
    public class GameAnimationSettingsInstaller : ScriptableObjectInstaller<GameAnimationSettingsInstaller>
    {
        public GameLoadingManager.AnimationSettings GameLoadingManager;

        public override void InstallBindings()
        {
            Container.BindInstance(GameLoadingManager);
        }
    }
}