using Photon.Realtime;
using System;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AgentDamageHandler : IInitializable,IDamageable
    {
        public event Action<Character,int,int> OnTakeDamageEvent;

        private Agent _agent;

        private AgentHealthView _agentHealthView;

        private HealthSettings _healthSettings;

        private int _health;

        public int Health
        {
            get { return _health; }
            set 
            { 
                _health = value;
                _agentHealthView.SetHealth(_health);
            }
        }

        public AgentDamageHandler(Agent agent,HealthSettings healthSettings,AgentHealthView agentHealthView)
        {
            _healthSettings = healthSettings;
            _agentHealthView = agentHealthView;
            _agent = agent;
        }

        public void Initialize()
        {
            Health = _healthSettings.Health;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer, Character attackerCannon = null)
        {
            Health -= damage;

            if (Health <= 0)
                _agent.Die(attackerCannon);
            
            ParticleManager.CreateParticle<TakeDamageParticle>(hitPoint);

            if (_agent.IsStaticAgent)
                return;

            OnTakeDamageEvent?.Invoke(attackerCannon,damage, _health);

        }
    }
}
