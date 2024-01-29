using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace CannonFightBase
{
    public class AIStateController:IInitializable,ITickable
    {
        private readonly Agent _agent;

        private readonly IdleMoveState _idleMoveState;

        private readonly FireState _fireState;

        private readonly NavMeshAgent _navMeshAgent;

        private AIState[] _states;

        private AIState _currentState;

        public AIStateController(IdleMoveState idleMovingState,
                               FireState shootingState,
                               NavMeshAgent navMeshAgent,
                               Agent agent)
        {
            _idleMoveState = idleMovingState;
            _fireState = shootingState;
            _agent = agent;
            _navMeshAgent = navMeshAgent;
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

        public void StopAction()
        {
            if (_navMeshAgent == null)
                return;

            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
            ChangeState<IdleMoveState>();
        }

    }
}
