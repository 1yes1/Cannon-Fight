using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProfileImagePresenter>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}
