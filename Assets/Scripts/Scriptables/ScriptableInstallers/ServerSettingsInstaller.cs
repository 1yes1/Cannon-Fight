using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "ServerSettingsInstaller", menuName = "CannonFight/ServerSettingsInstaller", order = 2)]
    public class ServerSettingsInstaller : ScriptableObjectInstaller<ServerSettingsInstaller>
    {
        public RoomManager.Settings ServerSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(ServerSettings);
            Container.BindInstance(ServerSettings.GameServerSettings);
            Container.BindInstance(ServerSettings.RoomServerSettings);
        }

    }
}
