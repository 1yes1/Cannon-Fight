using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "ServerSettingsInstaller", menuName = "CannonFight/ServerSettingsInstaller", order = 2)]
    public class ServerSettingsInstaller : ScriptableObjectInstaller<ServerSettingsInstaller>
    {
        public Launcher.Settings LauncherSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(LauncherSettings);
        }

    }
}
