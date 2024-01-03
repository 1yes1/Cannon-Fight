using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class ChestHitParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<ChestHitParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, ChestHitParticle>
        {
        }
    }
}
