using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public abstract class AILogic:IInitializable,IDisposable
    {
        protected AILogicManager _manager;

        protected AIStateController _controller;

        public bool _isExecuting = false;

        public bool IsExecuting => _isExecuting;

        public AILogic(AIStateController aiStateController)
        {
            _controller = aiStateController;
        }

        public abstract void Initialize();

        public abstract void Dispose();

        public void SetAILogicManager(AILogicManager aILogicManager)
        {
            _manager = aILogicManager;
        }
    }
}
