using System;
using UnityEngine;

namespace CannonFightBase
{
    [Serializable]
    public class ParticleTuple<T> where T : System.Enum
    {
        public T data;

        public ParticleSystem particleSystem;
    }
}
