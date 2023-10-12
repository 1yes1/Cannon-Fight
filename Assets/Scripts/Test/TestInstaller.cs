using UnityEngine;
using Zenject;

namespace CannonFightBase
{ 
    public class TestInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<TestClass>().AsSingle();        
        }

    }


}