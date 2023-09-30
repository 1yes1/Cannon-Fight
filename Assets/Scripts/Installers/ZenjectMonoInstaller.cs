using CannonFightBase;
using UnityEngine;
using Zenject;

public class ZenjectMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ParticleManager>().FromComponentInHierarchy().AsSingle();
        


    }
}