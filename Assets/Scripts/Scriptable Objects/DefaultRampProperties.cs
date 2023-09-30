using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "DefaultRampProperties", menuName = "CannonFight/DefaultRampProperties", order = 1)]
    public class DefaultRampProperties : ScriptableObjectInstaller
    {
        [Header("Ramp")]

        public float BoostMultiplier = 25;

        public override void InstallBindings()
        {
            Container.Bind<DefaultRampProperties>().FromScriptableObjectResource("ScriptableObjects/DefaultRampProperties").AsSingle();
        }

    }
}
