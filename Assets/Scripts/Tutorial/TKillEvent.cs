using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TKillEvent : TutorialEvent
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
            _agent.IsStaticAgent = true;
            UIManager.GetSubView<AimFireJoystick>().Show(0);

            GameEventReceiver.OnAgentDiedEvent += OnAgentDied;
        }

        private void OnAgentDied(Agent obj)
        {
            Debug.Log("Kill Event Done");
            CompleteTutorial(this);
        }

        protected override void OnEventUpdate()
        {
           
        }

        public override void Dispose()
        {
            GameEventReceiver.OnAgentDiedEvent -= OnAgentDied;
        }

    }
}
