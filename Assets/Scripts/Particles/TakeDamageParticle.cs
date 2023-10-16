using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class TakeDamageParticle : PoolableParticleBase
    {
        public override void Dispose()
        {
            _particleSystem.GetComponent<CFXR_Effect>().enabled = false;
            base.Dispose();
        }

        public class Factory : PlaceholderFactory<TakeDamageParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, TakeDamageParticle>
        {
        }
    }
}
