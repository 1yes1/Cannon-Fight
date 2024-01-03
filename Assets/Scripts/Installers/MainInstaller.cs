using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            SignalInstaller.Install(Container);

            Container.BindInterfacesAndSelfTo<JsonSaver>().FromNewComponentOnNewGameObject().AsSingle();
            //Container.Bind<ICannonDataLoader>().To<>
        }

    }
}
