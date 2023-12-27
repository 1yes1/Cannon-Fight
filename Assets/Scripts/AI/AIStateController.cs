using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AIStateController:IInitializable,ITickable
    {
        private Agent _agent;

        private AIState _currentState;

        private IdleMoveState _idleMoveState;

        private FireState __fireState;

        private AIEnemyDetector _enemyDetector;

        private AIState[] _states;

        public AIStateController(IdleMoveState idleMovingState,
                               FireState shootingState,
                               AIEnemyDetector enemyDetector,
                               Agent agent)
        {
            _idleMoveState = idleMovingState;
            __fireState = shootingState;
            _enemyDetector = enemyDetector;
            _agent = agent;
        }

        public void Initialize()
        {
            _states = new AIState[]
            {
                _idleMoveState,
                __fireState
            };

            ChangeState<IdleMoveState>();
            //ChangeState(_shootingState);
        }

        public void Tick()
        {
            if(_currentState != null)
                _currentState.OnUpdateState();

        }

        public void ChangeState<T>() where T : AIState
        {
            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i] is T)
                {
                    if (_currentState != null)
                        _currentState.OnExitState();
                    _currentState = _states[i];
                    _currentState.OnEnterState(this);
                    return;
                }
            }
            Debug.LogWarning("State couldn't found");

        }

    }
}
