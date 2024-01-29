using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static CannonFightBase.CarController;

namespace CannonFightBase
{
    public abstract class CarController : IInitializable,IFixedTickable
    {
        protected AgentView _agentView;

        protected MovementSettings _movementSettings;

        private bool _isBreaking;

        private float _horizontalInput, _verticalInput;

        private float _currentSteerAngle, _currentbreakForce;


        public CarController(AgentView agentView)
        {
            _agentView = agentView;
        }

        public void Initialize()
        {
            _movementSettings = _agentView.MovementSettings;
            _horizontalInput = 0;
            _verticalInput = 0;
            InitializeAI();
        }

        public abstract void InitializeAI();

        public void FixedTick()
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();

            FixedTickAI();
        }

        public abstract void FixedTickAI();

        private void GetInput()
        {

            //if (Input.GetKey(KeyCode.B))
            //{
            //    SetForward(1);
            //    SetSteeringAmount(1);
            //}

            // Breaking Input
            //_isBreaking = Input.GetKey(KeyCode.Space);
        }

        private void HandleMotor()
        {
            _movementSettings.FrontLeftWheelCollider.motorTorque = _verticalInput * _movementSettings.MotorForce;
            _movementSettings.FrontRightWheelCollider.motorTorque = _verticalInput * _movementSettings.MotorForce;

            _movementSettings.RearRightWheelCollider.motorTorque = _verticalInput * _movementSettings.MotorForce;
            _movementSettings.RearLeftWheelCollider.motorTorque = _verticalInput * _movementSettings.MotorForce;

            _currentbreakForce = _isBreaking ? _movementSettings.BreakForce : 0f;
            ApplyBreaking();
        }

        private void ApplyBreaking()
        {
            _movementSettings.FrontRightWheelCollider.brakeTorque = _currentbreakForce;
            _movementSettings.FrontLeftWheelCollider.brakeTorque = _currentbreakForce;
            _movementSettings.RearLeftWheelCollider.brakeTorque = _currentbreakForce;
            _movementSettings.RearRightWheelCollider.brakeTorque = _currentbreakForce;
        }

        private void HandleSteering()
        {
            _currentSteerAngle = _movementSettings.MaxSteerAngle * _horizontalInput;
            _movementSettings.FrontLeftWheelCollider.steerAngle = _currentSteerAngle;
            _movementSettings.FrontRightWheelCollider.steerAngle = _currentSteerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(_movementSettings.FrontLeftWheelCollider, _movementSettings.FrontLeftWheelTransform);
            UpdateSingleWheel(_movementSettings.FrontRightWheelCollider, _movementSettings.FrontRightWheelTransform);
            UpdateSingleWheel(_movementSettings.RearRightWheelCollider, _movementSettings.RearRightWheelTransform);
            UpdateSingleWheel(_movementSettings.RearLeftWheelCollider, _movementSettings.RearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }

        public void SetSteeringAmount(float steeringAmount)
        {
            // Steering Input
            _horizontalInput = steeringAmount;

        }

        public void SetForward(float forwardAmount)
        {
            // Acceleration Input
            _verticalInput = forwardAmount;
        }

        public void SetBreaking(bool isBreaking)
        {
            _isBreaking = isBreaking;
        }

        [Serializable]
        public struct MovementSettings
        {
            public float ForwardSpeed;

            public float TurnSpeed;

            public float MaxSteerAngle;

            public float MotorForce;

            public float BreakForce;

            public Transform FrontLeftWheelTransform;

            public Transform FrontRightWheelTransform;

            public Transform RearLeftWheelTransform;

            public Transform RearRightWheelTransform;

            public WheelCollider FrontLeftWheelCollider;

            public WheelCollider FrontRightWheelCollider;

            public WheelCollider RearLeftWheelCollider;

            public WheelCollider RearRightWheelCollider;
        }

    }
}
