using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace CannonFightBase
{
    public class Agent : Character,IDamageable
    {
        [SerializeField] private string _currentState;

        [SerializeField] private int _health;
        
        private AIStateController _controller;

        private AgentDamageHandler _agentDamageHandler;

        private AgentManager _agentManager;

        private AgentCarDriver _carDriver;


        public bool IsStaticAgent { get; set; }

        [Inject]
        private void Construct(AIStateController aiStateController,AgentDamageHandler agentDamageHandler, AgentCarDriver agentCarDriver)
        {
            _agentDamageHandler = agentDamageHandler;
            _controller = aiStateController;
            _carDriver = agentCarDriver;
        }


        public void Initialize(AgentManager agentManager)
        {
            _agentManager = agentManager;
            name = "Enemy ("+Random.Range(0,15)+")";
            _health = _agentDamageHandler.Health;
        }

        public void SetState(AIState aiState)
        {
            _currentState = aiState.ToString();
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer,Character attackerCannon)
        {
            _agentDamageHandler.TakeDamage(damage, hitPoint, attackerPlayer, attackerCannon);
            _health = _agentDamageHandler.Health;
        }

        public void Die(Character attackerAgent)
        {
            attackerAgent.GetKill();
            _agentManager.OnDie(attackerAgent);

            Debug.Log(attackerAgent + " killed " + name);

            Destroy(gameObject);
        }

        public void StopMoving()
        {
            _carDriver.SetBreaking(true);
        }

        protected override void OnGameStart()
        {
            _controller.ChangeState<IdleMoveState>();
        }

        public void StopAction()
        {
            CanDoAction = false;
            _controller.StopAction();
            GetComponent<Rigidbody>().isKinematic = true;
        }

        [Serializable]
        public struct RotateSettings
        {
            public float TurnSpeed;
            public float MaxHorizontalAngle;
            public float MaxVerticalAngle;
        }

        [Serializable]
        public struct ViewSettings
        {
            public LayerMask EnemyLayer;
            public float ViewRadius;
        }



        public class Factory : PlaceholderFactory<Agent>
        {
        }
    }
}
