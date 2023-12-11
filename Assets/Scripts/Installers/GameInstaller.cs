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
        private GameSettingsInstaller.PrefabSettings _prefabSettings;

        public override void InstallBindings()
        {
            //Container.Bind<Cannon>().FromComponentInHierarchy().AsSingle();

            Container.Bind<ParticleManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<Chest>().FromComponentsInHierarchy().AsSingle();

            Container.Bind<MainCamera>().FromComponentInHierarchy().AsSingle();

            Container.BindFactory<PlayerManager, PlayerManager.Factory>().FromComponentInNewPrefab(_prefabSettings.PlayerManagerPrefab).UnderTransformGroup("Cannons");

            Container.BindFactory<Cannon, Cannon.Factory>()
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<CannonInstaller>(_prefabSettings.CannonPrefab)
                .UnderTransformGroup("Cannons");

            Container.BindFactory<CannonBall, CannonBall.Factory>()
            .FromPoolableMemoryPool<CannonBall, CannonBall.Pool>(poolBinder => poolBinder
            .WithInitialSize(5)
            .FromComponentInNewPrefab(_prefabSettings.CannonBallPrefab)
            .UnderTransformGroup("CannonBallPool"));

            Container.BindFactory<Potion, Potion.Factory>()
            .FromPoolableMemoryPool<Potion, Potion.Pool>(poolBinder => poolBinder
            .WithInitialSize(5)
            .FromComponentInNewPrefab(_prefabSettings.Potion)
            .UnderTransformGroup("PotionPool"));

        }


    }
}
