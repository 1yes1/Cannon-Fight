using System;
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

            Container.Bind<AchievementManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<CoinManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<UserManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<CloudSaveManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<AudioEventListener>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadSceneManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<JsonSaver>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<AudioManager>().FromNewComponentOnNewGameObject().AsSingle();

            ExecutionOrder();

            //Container.Bind<ICannonDataLoader>().To<>
        }

        private void ExecutionOrder()
        {
            //Container.BindExecutionOrder<CoinManager>(-100);
            //Container.BindExecutionOrder<CloudSaveManager>(100);
        }
    }
}
