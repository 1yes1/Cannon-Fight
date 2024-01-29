using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static CannonFightBase.GameLoadingView;
using Random = UnityEngine.Random;

namespace CannonFightBase
{
    public class AgentManager : CharacterManager
    {
        private Agent.Factory _agentFactory;

        private Agent _agent;  
        
        public Agent Agent => _agent;

        public bool IsStaticAgent { get; set; }

        [Inject]
        public void Construct(Agent.Factory factory)
        {
            _agentFactory = factory;
        }

        public void Initialize()
        {
            SpawnAgent();
        }

        private void SpawnAgent()
        {
            _agent = _agentFactory.Create();
            _agent.Initialize(this);
            _agent.SetCharacterManager(this);

            SpawnPoint spawnPoint = SpawnManager.GetSpawnPoint();

            _agent.IsStaticAgent = IsStaticAgent;
            _agent.transform.position = spawnPoint.Position;
            _agent.transform.rotation = spawnPoint.Rotation;

            GameEventCaller.Instance.OnAgentSpawned(this);
        }

        public void OnDie(Character attackerAgent)
        {
            GameEventCaller.Instance.OnKill(attackerAgent, _agent);
            GameEventCaller.Instance.OnAgentDied(_agent);
            Destroy(gameObject);
        }

        public void StopAction()
        {
            IsStaticAgent = true;
            if (_agent == null)
                return;

            _agent.IsStaticAgent = IsStaticAgent;
            _agent.StopAction();
        }

        public class Factory : PlaceholderFactory<AgentManager>
        {
        }
    }
}
