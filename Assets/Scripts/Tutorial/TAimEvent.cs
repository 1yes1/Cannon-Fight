using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TAimEvent : TutorialEvent
    {
        private Transform _agentSpawnpoint;

        private Cannon _cannon;

        private GameObject _rotatorH;

        private GameObject _rotatorV;

        private Agent _agent;

        public override void SetParameters(params object[] objects)
        {
            _agentSpawnpoint = (Transform)objects[0];
        }

        protected override void OnEventStarted()
        {
            AgentManager agentManager = TutorialActivator.SpawnBots(1)[0];
            agentManager.Agent.transform.position = _agentSpawnpoint.position;
            agentManager.Agent.transform.rotation = _agentSpawnpoint.rotation;
            _agent = agentManager.Agent;

            _cannon = Cannon.Current;
            _cannon.CannonView.GetRotators(out _rotatorH,out _rotatorV);
        }

        protected override void OnEventUpdate()
        {
            _agent.StopMoving();

            float dot = Vector3.Dot(_rotatorV.transform.forward, (_agent.transform.position - _rotatorV.transform.position).normalized);
            if (dot >= 0.97f)
            {
                CompleteTutorial(this);
                _agent.Die(_cannon);
            } 
        }

    }
}
