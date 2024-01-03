using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class HealthPotionParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<HealthPotionParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, HealthPotionParticle>
        {
        }
    }
}
