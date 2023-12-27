using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CannonFightBase
{
    public class AICarDriver : CarController
    {
        private bool _isMoving;

        public AICarDriver(AgentView agentView) : base(agentView)
        {
            
        }
        public override void InitializeAI()
        {

        }

        public override void FixedTickAI()
        {
            if(!_isMoving)
                StopRotatingWheels();
        }

        private void StopRotatingWheels()
        {
            _movementSettings.FrontLeftWheelCollider.brakeTorque = Mathf.Infinity;
            _movementSettings.FrontRightWheelCollider.brakeTorque = Mathf.Infinity;
            _movementSettings.RearLeftWheelCollider.brakeTorque = Mathf.Infinity;
            _movementSettings.RearRightWheelCollider.brakeTorque = Mathf.Infinity;
        }

        public void StopMoving()
        {
            _isMoving = false;
        }

        public void StartMoving()
        {
            _isMoving =true;
        }

    }
}
