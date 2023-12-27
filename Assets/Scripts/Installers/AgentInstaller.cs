using CannonFightBase;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class AgentInstaller : Installer<AgentInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Agent>().FromComponentOnRoot();
        
        Container.BindInterfacesAndSelfTo<NavMeshAgent>().FromComponentOnRoot();

        Container.BindInterfacesAndSelfTo<AICarDriver>().AsSingle();

        Container.BindInterfacesAndSelfTo<AIStateController>().AsSingle();
        Container.BindInterfacesAndSelfTo<AgentDamageHandler>().AsSingle();

        Container.Bind<AgentView>().FromComponentOnRoot();

        Container.Bind<IdleMoveState>().FromNew().AsSingle();
        Container.Bind<FireState>().FromNew().AsSingle();
        Container.Bind<AIEnemyDetector>().FromNew().AsSingle();

    }
}