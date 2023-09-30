using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "CannonBallSettingsInstaller", menuName = "CannonFight/CannonBallSettingsInstaller", order = 1)]
    public class CannonBallSettingsInstaller : ScriptableObjectInstaller<CannonBallSettingsInstaller>
    {
        public CannonBall.Settings CannonBallSettings;
        public GameInstaller.Settings GameInstallerSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(CannonBallSettings);
            Container.BindInstance(GameInstallerSettings);
        }
    }
}
