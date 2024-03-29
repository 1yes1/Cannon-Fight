using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TMultiballPotionKillEvent : TutorialEvent
    {
        private Transform _agentSpawnTransform;

        private Agent _agent;

        public override void SetParameters(params object[] objects)
        {
            _agentSpawnTransform = (Transform)objects[0];
        }

        protected override void OnEventStarted()
        {
            AgentManager agentManager = TutorialActivator.SpawnBots(1)[0];
            agentManager.Agent.transform.position = _agentSpawnTransform.position;
            agentManager.Agent.transform.rotation = _agentSpawnTransform.rotation;
            _agent = agentManager.Agent;
            _agent.CanDoAction = true;
            //_agent.IsStaticAgent = true;

            GameEventReceiver.OnAgentDiedEvent += OnAgentDied;
        }
        private void OnAgentDied(Agent obj)
        {
            Debug.Log("Multiball Kill Event Completed");
            CompleteTutorial(this);
        }

        protected override void OnEventUpdate()
        {
            _agent.StopMoving();
        }

        public override void Dispose()
        {
            GameEventReceiver.OnAgentDiedEvent -= OnAgentDied;
        }
    }
}
