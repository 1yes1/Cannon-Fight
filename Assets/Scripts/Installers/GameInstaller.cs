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
            Container.Bind<ParticleManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<Chest>().FromComponentsInHierarchy().AsSingle();

            Container.Bind<CannonSkillHandler>().AsSingle();
            Container.Bind<CannonDamageHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<FireController>().AsSingle();
            Container.BindInterfacesAndSelfTo<AimController>().AsSingle();

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

        [Serializable]
        public class Settings
        {
            public CannonBall CannonBallPrefab;
            public Potion Potion;
        }

    }
}
