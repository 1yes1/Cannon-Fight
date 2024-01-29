using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class TMoveEvent : TutorialEvent
    {
        private Transform _targetTransform;
        private Cannon _cannon;

        public override void SetParameters(params object[] objects)
        {
            _targetTransform = ((GameObject)objects[0]).transform;
        }

        protected override void OnEventStarted()
        {
            _cannon = GameManager.CurrentCannon;
        }

        protected override void OnEventUpdate()
        {
            if(Vector3.Distance(_cannon.transform.position,_targetTransform.position) <= 2f)
            {
                CompleteTutorial(this);
            }
        }


    }
}
