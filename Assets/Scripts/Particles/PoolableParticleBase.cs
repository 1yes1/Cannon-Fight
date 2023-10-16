using CartoonFX;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public enum AllParticles
    {
        CannonTakeDamage
    }

    public abstract class PoolableParticleBase : MonoBehaviour, IPoolable<IMemoryPool>,IDisposable
    {
        protected IMemoryPool _pool;

        protected ParticleSystem _particleSystem;

        public virtual void Dispose()
        {
            _pool.Despawn(this);
        }
        
        public virtual void OnDespawned()
        {

        }

        public virtual void OnSpawned(IMemoryPool p1)
        {
            _pool = p1;
            _particleSystem = GetComponent<ParticleSystem>();
            Invoke(nameof(Dispose), _particleSystem.main.duration);
        }
    }
}
