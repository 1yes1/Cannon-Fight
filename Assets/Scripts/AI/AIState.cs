using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public abstract class AIState
    {
        protected AIStateController _stateController;

        private bool _isAppropriate = true;

        public bool IsAppropriate
        {
            get { return _isAppropriate; }
            set { _isAppropriate = value;}
        }

        public void OnEnterState(AIStateController aiStateController) 
        {
            _stateController = aiStateController;

            OnEnter(aiStateController);

        }
        public void OnUpdateState() { OnUpdate(); }
        public void OnExitState() { OnExit(); }

        public void SetAppropriate(bool isAppropriate)
        {
            IsAppropriate = isAppropriate;
        }

        protected abstract void OnEnter(AIStateController aiStateController);
        protected abstract void OnUpdate();
        protected abstract void OnExit();
    }
}
