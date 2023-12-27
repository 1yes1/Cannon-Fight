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
        public void OnUpdateState() { OnUpdate(); }
        public void OnExitState() { OnExit(); }


        protected abstract void OnEnter(AIStateController aiStateController);
        protected abstract void OnUpdate();
        protected abstract void OnExit();
    }
}
