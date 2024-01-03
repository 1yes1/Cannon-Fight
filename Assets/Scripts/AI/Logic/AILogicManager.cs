using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AILogicManager : IInitializable
    {
        private RunLogic _runLogic;
        private LookEnemyLogic _lookEnemyLogic;

        private AILogic[] _logics;

        public AILogicManager(RunLogic runLogic,LookEnemyLogic lookEnemyLogic)
        {
            _runLogic = runLogic;
            _lookEnemyLogic = lookEnemyLogic;

            _logics = new AILogic[]
            {
                _runLogic,
                _lookEnemyLogic
            };
        }

        public void Initialize()
        {
            SetManagers();
        }

        public void SetManagers()
        {
            for (int i = 0; i < _logics.Length; i++)
            {
                _logics[i].SetAILogicManager(this);
            }
        }

        public T GetLogic<T>() where T : AILogic
        {
            T logic = null;

            for (int i = 0; i < _logics.Length; i++)
            {
                if (_logics[i] is T)
                {
                    logic = (T)_logics[i];
                    break;
                }
            }

            return logic;
        }

    }
}
