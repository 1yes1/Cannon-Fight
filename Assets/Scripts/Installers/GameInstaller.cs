using System;
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
            //Container.Bind<CannonBall>().FromComponentInNewPrefab(_settings.CannonBallPrefab);

            Container.Bind<CannonSkillHandler>().AsTransient();

            Container.BindInterfacesAndSelfTo<FireController>().AsTransient();

            Container.BindFactory<CannonBall, CannonBall.Factory>()
            // We could just use FromMonoPoolableMemoryPool here instead, but
            // for IL2CPP to work we need our pool class to be used explicitly here
            .FromPoolableMemoryPool<CannonBall, CannonBallPool>(poolBinder => poolBinder
            // Spawn 20 right off the bat so that we don't incur spikes at runtime
            .WithInitialSize(5)
            .FromComponentInNewPrefab(_settings.CannonBallPrefab)
            .UnderTransformGroup(nameof(CannonBallPool)));

        }

        [Serializable]
        public class Settings
        {
            public CannonBall CannonBallPrefab;
        }

        public class CannonBallPool : MonoPoolableMemoryPool<IMemoryPool, CannonBall>
        {
        }

    }
}
