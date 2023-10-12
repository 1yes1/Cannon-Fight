using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace CannonFightBase
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private Settings _settings;

        public override void InstallBindings()
        {
            //Container.Bind<Cannon>().FromComponentInHierarchy().AsSingle();

            Container.Bind<ParticleManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<Chest>().FromComponentsInHierarchy().AsSingle();

            Container.BindFactory<PlayerManager, PlayerManager.Factory>().FromComponentInNewPrefab(_settings.PlayerManagerPrefab).UnderTransformGroup("Cannons");

            Container.BindFactory<Cannon, Cannon.Factory>()
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<CannonInstaller>(_settings.CannonPrefab)
                .UnderTransformGroup("Cannons");

            Container.BindFactory<CannonBall, CannonBall.Factory>()
            .FromPoolableMemoryPool<CannonBall, CannonBall.Pool>(poolBinder => poolBinder
            .WithInitialSize(5)
            .FromComponentInNewPrefab(_settings.CannonBallPrefab)
            .UnderTransformGroup("CannonBallPool"));

            Container.BindFactory<Potion, Potion.Factory>()
            .FromPoolableMemoryPool<Potion, Potion.Pool>(poolBinder => poolBinder
            .WithInitialSize(5)
            .FromComponentInNewPrefab(_settings.Potion)
            .UnderTransformGroup("PotionPool"));


        }


        void InstallExecutionOrder()
        {
            //Container.BindExecutionOrder<AimController>(-30);
            //Container.BindExecutionOrder<FireController>(-40);
        }


        [Serializable]
        public class Settings
        {
            public Cannon CannonPrefab;
            public PlayerManager PlayerManagerPrefab;
            public CannonBall CannonBallPrefab;
            public Potion Potion;
        }

    }
}
