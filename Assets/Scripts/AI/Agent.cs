using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CannonFightBase
{
    public class Agent : CannonBase,IDamageable
    {
        private AgentDamageHandler _agentDamageHandler;

        [Inject]
        private void Construct(AgentDamageHandler agentDamageHandler)
        {
            _agentDamageHandler = agentDamageHandler;
        }

        private void Awake()
        {
        }

        private void Update()
        {

        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer,CannonBase attackerCannon)
        {
            _agentDamageHandler.TakeDamage(damage, hitPoint, attackerPlayer, attackerCannon);
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
