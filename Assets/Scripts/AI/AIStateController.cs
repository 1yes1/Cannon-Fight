using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class AIStateController : MonoBehaviour
    {
        private AIState _currentState;


        private void Update()
        {
            if(_currentState != null)
                _currentState.OnUpdateState();
        }

        public void ChangeState(AIState state)
        {
            _currentState.OnExitState();
            _currentState = state;
            _currentState.OnEnterState(this);
        }
    }
}
