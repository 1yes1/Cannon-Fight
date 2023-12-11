using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace CannonFightBase
{
    public class CannonInstaller : Installer<CannonInstaller>
    {
        public override void InstallBindings()
        {
            //Bu ikisi zenject binding ile bind ediliyordu normalde
            //Fakat buraya ald�k �imdilik, e�er multiplayer iken s�k�nt� ��kar�rsa yine zenjectbinding componenti ile yapar�z
            Container.BindInterfacesAndSelfTo<Cannon>().FromComponentOnRoot();
            Container.BindInterfacesAndSelfTo<CannonView>().FromComponentOnRoot();

            Container.BindInterfacesAndSelfTo<AimController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MovementController>().AsSingle();
            Container.BindInterfacesAndSelfTo<FireController>().AsSingle();
            Container.Bind<RPCMediator>().FromNewComponentOnRoot().AsSingle();
            Container.Bind<CannonTraits>().AsSingle();

            Container.BindInterfacesAndSelfTo<CannonDamageHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<CannonSkillHandler>().AsSingle();

            InstallExecutionOrder();
        }

        void InstallExecutionOrder()
        {
            Container.BindExecutionOrder<CannonDamageHandler>(50);
        }

    }
}
