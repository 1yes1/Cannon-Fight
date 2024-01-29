using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class LookEnemyLogic : AILogic
    {
        private Agent _agent;
        private AgentDamageHandler _agentDamageHandler;

        public LookEnemyLogic(AIStateController aiStateController, Agent agent, AgentDamageHandler agentDamageHandler) : base(aiStateController)
        {
            _agent = agent;
            _agentDamageHandler = agentDamageHandler;
        }

        public override void Initialize()
        {
            _agentDamageHandler.OnTakeDamageEvent += OnTakeDamage;
            _controller.GetState<IdleMoveState>().OnArrivedEvent += OnArrived;
        }

        private void OnTakeDamage(Character attacker, int damage, int newHealth)
        {
            if(!_manager.GetLogic<RunLogic>().IsExecuting)
                LookAttackerDirection(attacker.transform);
        }

        private void LookAttackerDirection(Transform attackerTransform)
        {
            Vector3 target = _agent.transform.position + (attackerTransform.position - _agent.transform.position).normalized * 4;
            _controller.ChangeState<IdleMoveState>().GoCertainPosition(target);
        }

        private void OnArrived()
        {
            _isExecuting = false;
        }

        public override void Dispose()
        {
            _agentDamageHandler.OnTakeDamageEvent -= OnTakeDamage;
            _controller.GetState<IdleMoveState>().OnArrivedEvent -= OnArrived;
        }
    }
}
