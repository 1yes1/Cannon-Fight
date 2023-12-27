using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class DamageSkillParticle : PoolableParticleBase
    {
        public class Factory : PlaceholderFactory<DamageSkillParticle>
        {
        }

        public class Pool : MonoPoolableMemoryPool<IMemoryPool, DamageSkillParticle>
        {
        }
    }
}
