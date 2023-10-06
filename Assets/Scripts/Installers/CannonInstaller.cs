using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CannonInstaller : MonoInstaller
    {
        [SerializeField] private CannonController.Settings _cannonControllerSettings;

        [Inject] private Cannon _cannon;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CannonController>().AsSingle().WithArguments(_cannon,_cannonControllerSettings);
            InstallExecutionOrder();
        }

        void InstallExecutionOrder()
        {
            Container.BindExecutionOrder<CannonController>(-20);
        }

    }
}
