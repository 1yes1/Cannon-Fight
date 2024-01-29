using CannonFightBase;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class AgentInstaller : Installer<AgentInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Agent>().FromComponentOnRoot();
        
        Container.BindInterfacesAndSelfTo<NavMeshAgent>().FromComponentOnRoot();

        Container.BindInterfacesAndSelfTo<AgentCarDriver>().AsSingle();

        Container.BindInterfacesAndSelfTo<AIStateController>().AsSingle();
        Container.BindInterfacesAndSelfTo<AgentDamageHandler>().AsSingle();

        Container.Bind<AgentView>().FromComponentOnRoot();

        Container.Bind<AgentHealthView>().FromComponentOnRoot();

        Container.Bind<Canvas>().FromComponentInChildren();

        Container.Bind<CinemachineVirtualCamera>().FromComponentInChildren();


        Container.Bind<IdleMoveState>().FromNew().AsSingle();
        Container.Bind<FireState>().FromNew().AsSingle();
        Container.Bind<AIEnemyDetector>().FromNew().AsSingle();

        Container.BindInterfacesAndSelfTo<AILogicManager>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<RunLogic>().FromNew().AsSingle();
        Container.BindInterfacesAndSelfTo<LookEnemyLogic>().FromNew().AsSingle();
    }
}