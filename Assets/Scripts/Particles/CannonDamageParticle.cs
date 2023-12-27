using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CannonDamageParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<CannonDamageParticle>
        {

        }


        public class Pool : MonoPoolableMemoryPool<IMemoryPool, CannonDamageParticle>
        {

        }

    }
}
