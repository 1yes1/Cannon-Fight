using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static CannonFightBase.GameLoadingView;

namespace CannonFightBase
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager _instance;


        private List<AgentManager> _agents;

        public static AIManager Instance => _instance;

        public int AgentCount { get => _agents.Count;}


        private void OnEnable()
        {
            GameEventReceiver.OnAgentSpawnedEvent += OnAgentSpawned;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnAgentSpawnedEvent -= OnAgentSpawned;
        }

        private void Awake()
        {
            if(_instance == null)
                _instance = this;

            _agents = new List<AgentManager>();
        }

        private void OnAgentSpawned(AgentManager agentManager)
        {
            _agents.Add(agentManager);
        }

        [Serializable]
        public struct GameAgentSettings
        {
            public int MaxAgentCount;
        }

    }
}
