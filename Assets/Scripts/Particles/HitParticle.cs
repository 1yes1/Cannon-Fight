using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class HitParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<HitParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, HitParticle>
        {
        }
    }
}
