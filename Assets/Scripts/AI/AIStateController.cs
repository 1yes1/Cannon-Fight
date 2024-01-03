using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AIStateController:IInitializable,ITickable
    {
        private Agent _agent;

        private AIState _currentState;

        private IdleMoveState _idleMoveState;

        private FireState _fireState;

        private AIEnemyDetector _enemyDetector;

        private AIState[] _states;

        public AIStateController(IdleMoveState idleMovingState,
                               FireState shootingState,
                               AIEnemyDetector enemyDetector,
                               Agent agent)
        {
            _idleMoveState = idleMovingState;
            _fireState = shootingState;
            _enemyDetector = enemyDetector;
            _agent = agent;
        }

        public void Initialize()
        {
            if (_agent.IsStaticAgent)
                return;

            _states = new AIState[]
            {
                _idleMoveState,
                _fireState
            };

            //ChangeState(_shootingState);
        }

        public void Tick()
        {
            if (!_agent.CanDoAction)
                return;
            
            if (_agent.IsStaticAgent)
                return;

            if (_currentState != null)
            {
                _currentState.OnUpdateState();
            }

        }

        public T ChangeState<T>() where T : AIState
        {
            if (_agent.IsStaticAgent)
                return null;

            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i] is T)
                {
                    if (!_states[i].IsAppropriate)
                    {
                        Debug.LogWarning(typeof(T) + " state is not appropriate!");
                        return null;
                    }

                    if (_currentState != null)
                        _currentState.OnExitState();

                    _currentState = _states[i];
                    _currentState.OnEnterState(this);
                    _agent.SetState(_currentState);
                    return (T)_states[i];
                }
            }
            Debug.LogWarning("State couldn't found");
            return null;
        }

        public T GetState<T>() where T : AIState
        {
            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i] is T)
                {
                    return (T)_states[i];
                }
            }
            Debug.LogWarning("State couldn't found");
            return null;
        }

        public void SetAppropriateState<T>(bool isAppropriate)
        {
            if (_agent.IsStaticAgent)
                return;

            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i] is T)
                {
                    _states[i].SetAppropriate(isAppropriate);
                    Debug.LogWarning(typeof(T) + " state setted appropriate!: " + isAppropriate);
                    return;
                }
            }
        }

    }
}
