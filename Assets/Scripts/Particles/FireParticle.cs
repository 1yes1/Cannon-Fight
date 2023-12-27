using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class FireParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<FireParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, FireParticle>
        {
        }
    }
}
