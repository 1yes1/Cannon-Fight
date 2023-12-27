using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CannonFightBase
{
    public class AgentManager : MonoBehaviour
    {
        private Agent.Factory _agentFactory;

        private Agent _agent;   

        [Inject]
        public void Construct(Agent.Factory factory)
        {
            _agentFactory = factory;
        }

        public void Initialize()
        {
            SpawnBot();
        }

        private void SpawnBot()
        {
            _agent = _agentFactory.Create();
            SpawnPoint spawnPoint = SpawnManager.GetSpawnPoint();

            _agent.transform.position = spawnPoint.Position;
            _agent.transform.rotation = spawnPoint.Rotation;
        }

        public class Factory : PlaceholderFactory<AgentManager>
        {
        }
    }
}
