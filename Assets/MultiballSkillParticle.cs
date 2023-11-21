using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class MultiballSkillParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<MultiballSkillParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, MultiballSkillParticle>
        {
        }
    }
}
