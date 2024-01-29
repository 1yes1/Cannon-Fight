using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class TutorialInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AudioManager>().FromNewComponentOnNewGameObject().AsSingle();

            Container.BindInterfacesAndSelfTo<TutorialEventController>().AsSingle();
            Container.Bind<GameTutorialManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<TutorialEvent>().To<TMoveEvent>().AsSingle();
            Container.Bind<TutorialEvent>().To<TAimEvent>().AsSingle();
            Container.Bind<TutorialEvent>().To<TKillEvent>().AsSingle();
            Container.Bind<TutorialEvent>().To<TOpenChestDamageEvent>().AsSingle();
            Container.Bind<TutorialEvent>().To<TOpenChestHealthEvent>().AsSingle();
            Container.Bind<TutorialEvent>().To<TOpenChestMultiballEvent>().AsSingle();
            Container.Bind<TutorialEvent>().To<TDamagePotionKillEvent>().AsSingle();
            Container.Bind<TutorialEvent>().To<TMultiballPotionKillEvent>().AsSingle();
        }
    }
}