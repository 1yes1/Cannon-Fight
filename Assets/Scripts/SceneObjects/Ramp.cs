using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class Ramp : MonoBehaviour
    {
        private Settings _settings;

        [Inject]
        public void Construct(Settings settings)
        {
            _settings = settings;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Cannon"))
            {
                Cannon cannon = other.GetComponent<Cannon>();
                cannon.Boost(_settings.BoostMultiplier);
            }
        }

        [Serializable]
        public class Settings
        {
            public float BoostMultiplier = 25;

        }

    }
}
