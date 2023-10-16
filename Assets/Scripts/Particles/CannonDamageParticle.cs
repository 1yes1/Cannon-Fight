using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CannonDamageParticle : PoolableParticleBase
    {
        public override void OnDespawned()
        {
        }

        public override void OnSpawned(IMemoryPool p1)
        {
        }

        public class Factory : PlaceholderFactory<CannonDamageParticle>
        {

        }


        public class Pool : MonoPoolableMemoryPool<IMemoryPool, CannonDamageParticle>
        {

        }

    }
}
