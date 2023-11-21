using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class HealthSkillParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<HealthSkillParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, HealthSkillParticle>
        {
        }
    }
}
