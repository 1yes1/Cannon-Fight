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
    public class IdleMoveState : AIState
    {
        private readonly AICarDriver _carDriver;

        private readonly NavMeshAgent _navMeshAgent;

        private readonly NavMeshSurface _navMeshSurface;

        private readonly AIEnemyDetector _enemyDetector;

        private Vector3 _moveTarget;

        private Agent _agent;

        public IdleMoveState(AICarDriver carDriver,Agent agent,NavMeshAgent navMeshAgent,NavMeshSurface navMeshSurface,AIEnemyDetector aiEnemyDetector)
        {
            _carDriver = carDriver;
            _agent = agent;
            _navMeshAgent = navMeshAgent;
            _navMeshSurface = navMeshSurface;
            _enemyDetector = aiEnemyDetector;
        }


        protected override void OnEnter(AIStateController aiStateController)
        {
            _carDriver.StartMoving();

        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _navMeshAgent.SetDestination(_moveTarget = GetRandomPosition());
            }

            if (_navMeshAgent.remainingDistance > 2f && _moveTarget != Vector3.zero)
            {

                Vector3 dirToTarget = _moveTarget - _agent.transform.position;

                float angle = Vector3.SignedAngle(_agent.transform.forward, dirToTarget, Vector3.up);


                _carDriver.SetForward(1);
                _carDriver.SetBreaking(false);

                if (Vector3.Distance(_agent.transform.position, _moveTarget) <= 10f)
                    _carDriver.SetSteeringAmount(0);
                else
                {

                    if (angle >= 4f)
                    {
                        _carDriver.SetSteeringAmount(1);
                    }
                    else if (angle <= -4f)
                    {
                        _carDriver.SetSteeringAmount(-1);
                    }
                    else
                    {
                        _carDriver.SetSteeringAmount(0);
                    }
                }



                //print(_navMeshAgent.remainingDistance);
            }
            else
            {
                _carDriver.SetBreaking(true);
                _carDriver.SetForward(0);
            }

            if (_enemyDetector.FindClosestEnemy() != null)
                _stateController.ChangeState<FireState>();

        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(-_navMeshSurface.size.x / 2f, _navMeshSurface.size.x / 2f), 0, Random.Range(-_navMeshSurface.size.z / 2f, _navMeshSurface.size.z / 2f));
        }

        protected override void OnExit()
        {
            _carDriver.StopMoving();
        }


    }
}
