using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class MultiballPotionParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<MultiballPotionParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, MultiballPotionParticle>
        {
        }
    }
}
