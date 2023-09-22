using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class Hittable : MonoBehaviour, IHittable
    {
        public void OnHit(Vector3 hitPoint)
        {
            ParticleSystem particleSystem = ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.takeDamageParticle, null, hitPoint, false);
        }
    }
}
