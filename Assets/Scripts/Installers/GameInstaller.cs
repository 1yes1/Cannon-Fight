using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
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

            Container.BindInterfacesAndSelfTo<ParticleManager>().AsSingle();

            Container.Bind<Chest>().FromComponentsInHierarchy().AsSingle();

            Container.Bind<MainCamera>().FromComponentInHierarchy().AsSingle();

            Container.BindFactory<CannonManager, CannonManager.Factory>().FromComponentInNewPrefab(_prefabSettings.PlayerManagerPrefab).UnderTransformGroup("Cannons");

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

            Container.BindFactory<Agent, Agent.Factory>()
            .FromSubContainerResolve()
            .ByNewPrefabInstaller<AgentInstaller>(_prefabSettings.AgentPrefab)
            .UnderTransformGroup("Agents");

            Container.BindFactory<AgentManager, AgentManager.Factory>().FromComponentInNewPrefab(_prefabSettings.AgentManagerPrefab).UnderTransformGroup("Agents");

            Container.Bind<NavMeshSurface>().FromComponentInHierarchy().AsSingle();

            Container.Bind<TestTarget>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IEventSubscriber>().To<AimFireJoystick>().FromComponentInHierarchy().AsSingle();

            InstallExecutionOrder();
        }

        private void InstallExecutionOrder()
        {
            //Container.BindExecutionOrder<ParticleManager>(10);
        }

    }

}
