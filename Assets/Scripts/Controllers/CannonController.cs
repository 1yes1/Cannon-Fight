using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CannonController : IFixedTickable,ITickable,IInitializable,ICannonBehaviour
    {
        private Settings  _settings;

        private Transform _transform;

        private Cannon _cannon;

        private Rigidbody _rigidbody;

        private float horizontalInput, verticalInput;

        private float currentSteerAngle, currentbreakForce;

        private bool isBreaking;

        private bool _isBoosting = false;

        public CannonController(Cannon cannon,Settings settings)
        {
            _cannon = cannon;
            _settings = settings;
        }

        public void Initialize()
        {
            _rigidbody = _cannon.Rigidbody;
            _transform = _cannon.transform;
            
            _rigidbody.maxLinearVelocity = _settings.MaxSpeed;

        }

        public void OnSpawn(PlayerManager playerManager)
        {
            if (!_cannon.OwnPhotonView.IsMine)
                return;

        }

        private void OnDisable()
        {
            if (!_cannon.OwnPhotonView.IsMine)
                return;

        }

        public void FixedTick()
        {
            if (!_cannon.OwnPhotonView.IsMine)
                return;

            if (_cannon.IsDead)
                return;

            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }


        public void Tick()
        {
            if (!_cannon.OwnPhotonView.IsMine)
                return;

            if (_cannon.IsDead)
                return;

            //print(_rigidbody.velocity.magnitude);
            CheckUpsideDown();
        }

        private void GetInput()
        {
            if(GameManager.Instance.useAndroidControllers)
            {
                // Steering Input
                horizontalInput = CannonJoystick.Horizontal;
                //print(horizontalInput);

                // Acceleration Input
                verticalInput = CannonJoystick.Vertical;

                float localForwardVelocity = Vector3.Dot(_rigidbody.velocity, _transform.forward);

                //print(localForwardVelocity);
                // Breaking Input
                if (verticalInput == -1 && localForwardVelocity > 0.1f)
                {
                    isBreaking = true;
                    verticalInput = 0;
                    //print("Breaking");
                }
                else
                {
                    //print("Not Breaking");
                    isBreaking = false;
                }
            }
            else
            {
                // Steering Input
                horizontalInput = Input.GetAxis("Horizontal");

                // Acceleration Input
                verticalInput = Input.GetAxis("Vertical");

                // Breaking Input
                isBreaking = Input.GetKey(KeyCode.Space);
            }


        }

        private void HandleMotor()
        {
            _settings.FrontLeftWheelCollider.motorTorque = verticalInput * _settings.MotorForce;
            _settings.FrontRightWheelCollider.motorTorque = verticalInput * _settings.MotorForce;
            currentbreakForce = isBreaking ? _settings.BreakForce : 0f;
            ApplyBreaking();
        }

        private void ApplyBreaking()
        {
            _settings.FrontRightWheelCollider.brakeTorque = currentbreakForce;
            _settings.FrontLeftWheelCollider.brakeTorque = currentbreakForce;
            _settings.RearLeftWheelCollider.brakeTorque = currentbreakForce;
            _settings.RearRightWheelCollider.brakeTorque = currentbreakForce;
        }

        private void HandleSteering()
        {
            currentSteerAngle = _settings.MaxSteerAngle * horizontalInput;
            _settings.FrontLeftWheelCollider.steerAngle = currentSteerAngle;
            _settings.FrontRightWheelCollider.steerAngle = currentSteerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(_settings.FrontLeftWheelCollider, _settings.FrontLeftWheelTransform);
            UpdateSingleWheel(_settings.FrontRightWheelCollider, _settings.FrontRightWheelTransform);
            UpdateSingleWheel(_settings.RearRightWheelCollider, _settings.RearRightWheelTransform);
            UpdateSingleWheel(_settings.RearLeftWheelCollider, _settings.RearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }

        public void OnDie()
        {
            _rigidbody.isKinematic = true;
        }

        public void Boost(float boostMultiplier)
        {
            _isBoosting = true;
            _rigidbody.maxLinearVelocity = boostMultiplier;
            _rigidbody.velocity = _transform.forward * boostMultiplier;
            _cannon.Invoke(nameof(StopBoosting), 3);
            GameEventCaller.Instance.OnBoostStarted(_cannon);
        }

        private void StopBoosting()
        {
            _isBoosting = false;
            _rigidbody.maxLinearVelocity = _settings.MaxSpeed;
            GameEventCaller.Instance.OnBoostEnded(_cannon);
        }

        private void CheckUpsideDown()
        {
            float val = Vector3.Dot(_transform.up, Vector3.down);

            if (val < 0)
                return;

            val = Vector3.Dot(_transform.right, Vector3.up);
            //print(val);
            if (val > 0.12f)
            {
                _rigidbody.AddTorque(-_transform.forward * _settings.UpsideDownTorque);
                //print("Right");
            }
            else
            {
                _rigidbody.AddTorque(_transform.forward * _settings.UpsideDownTorque);
                //print("Left");
            }

        }


        [Serializable]
        public class Settings
        {
            public float MotorForce, BreakForce, MaxSteerAngle, MaxSpeed;

            public float UpsideDownTorque = 5;

            public WheelCollider FrontLeftWheelCollider, FrontRightWheelCollider;

            public WheelCollider RearLeftWheelCollider, RearRightWheelCollider;

            public Transform FrontLeftWheelTransform, FrontRightWheelTransform;

            public Transform RearLeftWheelTransform, RearRightWheelTransform;
        }
        
    }

}
