using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class CannonController : MonoBehaviour, ICannonBehaviour
    {
        [SerializeField] private float _motorForce, _breakForce, _maxSteerAngle, _maxSpeed;

        [SerializeField] private float _upsideDownTorque = 5;

        [SerializeField] private WheelCollider _frontLeftWheelCollider, _frontRightWheelCollider;

        [SerializeField] private WheelCollider _rearLeftWheelCollider, _rearRightWheelCollider;

        [SerializeField] private Transform _frontLeftWheelTransform, _frontRightWheelTransform;

        [SerializeField] private Transform _rearLeftWheelTransform, _rearRightWheelTransform;

        private PhotonView _photonView;

        private Cannon _cannon; 

        private Rigidbody _rigidbody;

        private float horizontalInput, verticalInput;

        private float currentSteerAngle, currentbreakForce;

        private bool isBreaking;

        private bool _isBoosting = false;

        public void OnSpawn(PlayerManager playerManager)
        {
            if (!_photonView.IsMine)
                return;

            _cannon.OnDieEvent += OnDie;
        }

        private void OnDisable()
        {
            if (!_photonView.IsMine)
                return;

            _cannon.OnDieEvent -= OnDie;
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();

            if (!_photonView.IsMine)
                return;

            _cannon = GetComponent<Cannon>();

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.maxLinearVelocity = _maxSpeed;
        }
        private void Start()
        {

        }

        private void FixedUpdate()
        {
            if (!_photonView.IsMine)
                return;

            if (_cannon.IsDead)
                return;

            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }

        private void Update()
        {
            if (!_photonView.IsMine)
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

                float localForwardVelocity = Vector3.Dot(_rigidbody.velocity, transform.forward);

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
            _frontLeftWheelCollider.motorTorque = verticalInput * _motorForce;
            _frontRightWheelCollider.motorTorque = verticalInput * _motorForce;
            currentbreakForce = isBreaking ? _breakForce : 0f;
            ApplyBreaking();
        }

        private void ApplyBreaking()
        {
            _frontRightWheelCollider.brakeTorque = currentbreakForce;
            _frontLeftWheelCollider.brakeTorque = currentbreakForce;
            _rearLeftWheelCollider.brakeTorque = currentbreakForce;
            _rearRightWheelCollider.brakeTorque = currentbreakForce;
        }

        private void HandleSteering()
        {
            currentSteerAngle = _maxSteerAngle * horizontalInput;
            _frontLeftWheelCollider.steerAngle = currentSteerAngle;
            _frontRightWheelCollider.steerAngle = currentSteerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(_frontLeftWheelCollider, _frontLeftWheelTransform);
            UpdateSingleWheel(_frontRightWheelCollider, _frontRightWheelTransform);
            UpdateSingleWheel(_rearRightWheelCollider, _rearRightWheelTransform);
            UpdateSingleWheel(_rearLeftWheelCollider, _rearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }

        private void OnDie()
        {
            _rigidbody.isKinematic = true;
        }

        public void Boost(float boostMultiplier)
        {
            _isBoosting = true;
            _rigidbody.maxLinearVelocity = boostMultiplier;
            _rigidbody.velocity = transform.forward * boostMultiplier;
            Invoke(nameof(StopBoosting), 3);
            GameEventCaller.Instance.OnBoostStarted(GetComponent<Cannon>());
        }

        private void StopBoosting()
        {
            _isBoosting = false;
            _rigidbody.maxLinearVelocity = _maxSpeed;
            GameEventCaller.Instance.OnBoostEnded(GetComponent<Cannon>());
        }

        private void CheckUpsideDown()
        {
            float val = Vector3.Dot(transform.up, Vector3.down);

            if (val < 0)
                return;

            val = Vector3.Dot(transform.right, Vector3.up);
            //print(val);
            if (val > 0.12f)
            {
                _rigidbody.AddTorque(-transform.forward * _upsideDownTorque);
                //print("Right");
            }
            else
            {
                _rigidbody.AddTorque(transform.forward * _upsideDownTorque);
                //print("Left");
            }

        }

    }

}
