using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "DefaultFireSettingsInstaller", menuName = "CannonFight/DefaultFireSettingsInstaller", order = 1)]
    public class DefaultFireSettingsInstaller : ScriptableObjectInstaller<DefaultFireSettingsInstaller>
    {
        public FireController.Settings FireControllerSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(FireControllerSettings);
        }
    }
}
