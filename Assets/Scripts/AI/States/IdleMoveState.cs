using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace CannonFightBase
{
    public class IdleMoveState : AIState
    {
        public event Action OnArrivedEvent;

        private readonly AgentCarDriver _carDriver;

        private readonly NavMeshAgent _navMeshAgent;

        private readonly NavMeshSurface _navMeshSurface;

        private readonly AIEnemyDetector _enemyDetector;

        private readonly MovementSettings _movementSettings;

        private Vector3 _moveTarget;

        private Agent _agent;

        private TestTarget _testTarget;

        private bool _goCertainPosition;

        public IdleMoveState(AgentCarDriver carDriver,Agent agent,NavMeshAgent navMeshAgent,NavMeshSurface navMeshSurface,AIEnemyDetector aiEnemyDetector,MovementSettings movementSettings)
        {
            _carDriver = carDriver;
            _agent = agent;
            _navMeshAgent = navMeshAgent;
            _navMeshSurface = navMeshSurface;
            _enemyDetector = aiEnemyDetector;
            _movementSettings = movementSettings;
            //_testTarget = testTarget;
        }

        protected override void OnEnter(AIStateController aiStateController)
        {
            if(!_navMeshAgent.enabled)
                _navMeshAgent.enabled = true;

            if (!_navMeshAgent.isOnNavMesh)
                return;

            GoNewPosition();

            _navMeshAgent.speed = _movementSettings.Speed;

            //Debug.Log("On Enter Idle Moving");
        }

        protected override void OnUpdate()
        {
            if (_navMeshAgent.remainingDistance > 2f && _moveTarget != Vector3.zero)
            {
                Vector3 dirToTarget = _navMeshAgent.path.corners[1] - _agent.transform.position;

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
                _goCertainPosition = false;
                OnArrivedEvent?.Invoke();
                if (_navMeshAgent.remainingDistance <= 0.75f)
                    GoNewPosition();
            }

            //Debug.Log("IDLE MOVE STATE");

            if (_enemyDetector.FindClosestEnemy() != null && _enemyDetector.HasFireAngle())
                _stateController.ChangeState<FireState>();

        }

        private void GoNewPosition()
        {
            _navMeshAgent.SetDestination(_moveTarget = GetRandomPosition());
            _navMeshAgent.isStopped = false;
        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(-_navMeshSurface.size.x / 2f, _navMeshSurface.size.x / 2f), 0, Random.Range(-_navMeshSurface.size.z / 2f, _navMeshSurface.size.z / 2f));
        }

        public void GoCertainPosition(Vector3 position)
        {
            //Eðer çok hasar almýþsa yani kaçýyorsa belirli pozisyona gitmesin
            if (!_stateController.GetState<FireState>().IsAppropriate)
                return;

            _navMeshAgent.SetDestination(_moveTarget = position);
            _navMeshAgent.isStopped = false;
            _goCertainPosition = true;
        }

        protected override void OnExit()
        {
            _carDriver.SetBreaking(true);
            if(_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.isStopped = true;
        }



    }
}
