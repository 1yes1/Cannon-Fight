using Photon.Pun;
using System;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class MovementController : IFixedTickable,ITickable,IInitializable,ICannonBehaviour
    {
        private readonly Settings  _settings;

        private readonly Cannon _cannon;

        private readonly CannonView _cannonView;

        private readonly CannonTraits _cannonTraits;

        private PhotonView _photonView;

        private Transform _transform;

        private Rigidbody _rigidbody;

        private float _horizontalInput, _verticalInput;

        private float _currentSteerAngle, _currentbreakForce;

        private float _maxSpeed;

        private bool _isBreaking;

        private bool _isBoosting = false;

        public MovementController(Cannon cannon,CannonTraits cannonTraits,CannonView view)
        {
            _cannon = cannon;
            _settings = view.CannonControllerSettings;
            _cannonTraits = cannonTraits;
            _cannonView = view;
        }

        public void Initialize()
        {
            _photonView = _cannonView.PhotonView;
            _rigidbody = _cannonView.Rigidbody;
            _transform = _cannon.transform;

            
            _maxSpeed = _cannonTraits.Speed;

            _rigidbody.maxLinearVelocity = _maxSpeed;
        }

        public void OnSpawn(CannonManager playerManager)
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
            if (!_cannon.CanDoAction && !TestManager.IsTesting)
                return;

            if (!_cannon.OwnPhotonView.IsMine && !TestManager.IsTesting)
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

            if (!_cannon.CanDoAction)
                return;

            CheckUpsideDown();
        }

        private void GetInput()
        {
#if !UNITY_EDITOR
            // Steering Input
            _horizontalInput = CannonJoystick.Horizontal;
            //Debug.Log(_horizontalInput);

            // Acceleration Input
            _verticalInput = CannonJoystick.Vertical;

            float localForwardVelocity = Vector3.Dot(_rigidbody.velocity, _transform.forward);

            //print(localForwardVelocity);
            // Breaking Input
            if (_verticalInput == -1 && localForwardVelocity > 0.1f)
            {
                _isBreaking = true;
                _verticalInput = 0;
                //print("Breaking");
            }
            else
            {
                //print("Not Breaking");
                _isBreaking = false;
            }
#else

            // Steering Input
            _horizontalInput = Input.GetAxis("Horizontal");

            // Acceleration Input
            _verticalInput = Input.GetAxis("Vertical");

            // Breaking Input
            _isBreaking = Input.GetKey(KeyCode.Space);
#endif
        }

        private void HandleMotor()
        {
            _settings.FrontLeftWheelCollider.motorTorque = _verticalInput * _settings.MotorForce;
            _settings.FrontRightWheelCollider.motorTorque = _verticalInput * _settings.MotorForce;
            _currentbreakForce = _isBreaking ? _settings.BreakForce : 0f;
            ApplyBreaking();
        }

        private void ApplyBreaking()
        {
            _settings.FrontRightWheelCollider.brakeTorque = _currentbreakForce;
            _settings.FrontLeftWheelCollider.brakeTorque = _currentbreakForce;
            _settings.RearLeftWheelCollider.brakeTorque = _currentbreakForce;
            _settings.RearRightWheelCollider.brakeTorque = _currentbreakForce;
        }

        private void HandleSteering()
        {
            _currentSteerAngle = _settings.MaxSteerAngle * _horizontalInput;
            _settings.FrontLeftWheelCollider.steerAngle = _currentSteerAngle;
            _settings.FrontRightWheelCollider.steerAngle = _currentSteerAngle;
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
            _rigidbody.maxLinearVelocity = _maxSpeed;
            GameEventCaller.Instance.OnBoostEnded(_cannon);
        }

        private void CheckUpsideDown()
        {
            float val = Vector3.Dot(_transform.up, Vector3.down);

            if (val < 0)
                return;

            val = Vector3.Dot(_transform.right, Vector3.up);
            if (val > 0.12f)
            {
                _rigidbody.AddTorque(-_transform.forward * _settings.UpsideDownTorque);
            }
            else
            {
                _rigidbody.AddTorque(_transform.forward * _settings.UpsideDownTorque);
            }
        }

        [Serializable]
        public class Settings
        {
            public float MotorForce, BreakForce, MaxSteerAngle;

            public float UpsideDownTorque = 5;

            public WheelCollider FrontLeftWheelCollider, FrontRightWheelCollider;

            public WheelCollider RearLeftWheelCollider, RearRightWheelCollider;

            public Transform FrontLeftWheelTransform, FrontRightWheelTransform;

            public Transform RearLeftWheelTransform, RearRightWheelTransform;
        }
        
    }

}
