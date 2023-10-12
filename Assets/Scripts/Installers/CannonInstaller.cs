using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace CannonFightBase
{
    public class CannonInstaller : Installer<CannonInstaller>
    {
        //[SerializeField] private Cannon _cannon;
        //[SerializeField] private CannonController.Settings _cannonControllerSettings;

        public override void InstallBindings()
        {
            //Container.BindInstance(_cannon);
            
            //Container.Bind<Cannon>().AsSingle();
            Container.Bind<CannonSkillHandler>().AsSingle();
            Container.Bind<CannonDamageHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<AimController>().AsSingle();
            Container.BindInterfacesAndSelfTo<CannonController>().AsSingle();
            Container.BindInterfacesAndSelfTo<FireController>().AsSingle();
            Container.Bind<RPCMediator>().FromNewComponentOnRoot().AsSingle();

            //Container.BindInterfacesAndSelfTo<CannonController>().AsSingle().WithArguments(_cannon, _cannonControllerSettings);
            //Container.BindInstance(_cannonControllerSettings);

            InstallExecutionOrder();
        }

        void InstallExecutionOrder()
        {
            //Container.BindExecutionOrder<CannonController>(-20);
        }

    }
}
