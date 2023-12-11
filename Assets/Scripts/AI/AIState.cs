using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public abstract class AIState
    {
        protected AIStateController _stateController;

        public void OnEnterState(AIStateController aiStateController) 
        {
            _stateController = aiStateController;

            OnEnter(aiStateController);

        }
        public void OnUpdateState() { }
        public void OnExitState() { }


        protected virtual void OnEnter(AIStateController aiStateController) { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }
    }
}
