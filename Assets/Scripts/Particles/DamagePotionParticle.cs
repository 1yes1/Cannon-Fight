using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class DamagePotionParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<DamagePotionParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, DamagePotionParticle>
        {
        }
    }
}
