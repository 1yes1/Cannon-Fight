using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class Hittable : MonoBehaviour, IHittable
    {
        private HitParticle.Factory _particleFactory;

        [Inject]
        public void Construct(HitParticle.Factory factory)
        {
            _particleFactory = factory;
        }

        public void OnHit(Vector3 hitPoint)
        {
            //ParticleManager.Instance.CreateAndPlay(_particleSettings.HitParticle, null, hitPoint, false);
            ParticleManager.CreateParticle<HitParticle>(hitPoint, null, false);
        }

        [Serializable]
        public class ParticleSettings
        {
            public ParticleSystem HitParticle;

        }
    }
}
