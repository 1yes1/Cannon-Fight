using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AgentDamageHandler : IInitializable,IDamageable
    {
        private HealthSettings _healthSettings;

        private int _health;

        public int Health => _health;

        public AgentDamageHandler(HealthSettings healthSettings)
        {
            _healthSettings = healthSettings;
        }

        public void Initialize()
        {
            _health = _healthSettings.Health;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer, CannonBase attackerCannon = null)
        {
            _health -= damage;
            ParticleManager.CreateParticle<TakeDamageParticle>(hitPoint);
            Debug.Log("Vurulduk: "+_health);
        }
    }
}
