using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CannonFightBase
{
    public class FireState : AIState
    {
        private readonly CannonBall.Factory _cannonBallFactory;

        private readonly AIEnemyDetector _enemyDetector;

        private readonly AgentView _agentView;

        private readonly Settings _settings;

        private readonly Agent.RotateSettings _rotateSettings;

        private readonly FireSettings _fireSettings;

        private readonly FireController.ParticleSettings _particleSettings;

        private Transform _targetEnemy;

        private bool _isFiring = false;

        private float _fireRate;


        public FireState(AgentView agentView,Agent.RotateSettings rotateSettings, CannonBall.Factory cannonBallFactory, AIEnemyDetector enemyDetector,FireSettings fireSettings,
            FireController.ParticleSettings particleSettings)
        {
            _cannonBallFactory = cannonBallFactory;
            _settings = agentView.AimSettings;
            _agentView = agentView;
            _rotateSettings = rotateSettings;
            _enemyDetector = enemyDetector;
            _fireSettings = fireSettings;
            _particleSettings = particleSettings;
        }

        protected override void OnEnter(AIStateController aiStateController)
        {
            _fireRate = _fireSettings.Rate;
        }

        protected override void OnExit()
        {
        }

        protected override void OnUpdate()
        {
            if (!CheckEnemy())
                return;
            TurnHorizontal();
            TurnVertical();
            CheckFire();
        }

        private bool CheckEnemy()
        {
            _targetEnemy = _enemyDetector.FindClosestEnemy();
            bool hasAngle = _enemyDetector.HasFireAngle();

            if (_targetEnemy == null || !hasAngle)
            {
                ChangeState();
                //Debug.Log("No Target In Area");
                StopFire();

                return false;
            }

            StartFire();

            //Debug.Log("Target Found");

            return true;
        }

        private void TurnHorizontal()
        {
            Vector3 dir = _targetEnemy.position - _settings.RotatorH.position;
            dir.y = 0;
            _settings.RotatorH.rotation = Quaternion.RotateTowards(_settings.RotatorH.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotateSettings.TurnSpeed);
            float clampedAngle = ClampAngle(_settings.RotatorH.transform.localEulerAngles.y, -_rotateSettings.MaxHorizontalAngle, _rotateSettings.MaxHorizontalAngle);

            //Debug.Log("Dot: "+dot);
            
            _settings.RotatorH.localEulerAngles = new Vector3(_settings.RotatorH.localEulerAngles.x, clampedAngle, _settings.RotatorH.localEulerAngles.z);

            //_settings.RotatorH.transform.Rotate(Vector3.up * 5 * Time.deltaTime);
        }



        private void TurnVertical()
        {
            //Debug.DrawRay(_targetEnemy, projectedAngle * 15, Color.blue);

            Vector3 dir = _targetEnemy.position - _settings.RotatorV.position;
            Vector3 euler = _settings.RotatorV.localEulerAngles;
            Quaternion quaternion = Quaternion.LookRotation(dir);
            _settings.RotatorV.rotation = Quaternion.RotateTowards(_settings.RotatorV.rotation, quaternion, Time.deltaTime * _rotateSettings.TurnSpeed * 2f);
            float clampedAngle = ClampAngle(_settings.RotatorV.transform.localEulerAngles.x - 0.75f, -_rotateSettings.MaxVerticalAngle, _rotateSettings.MaxVerticalAngle);
            _settings.RotatorV.localEulerAngles = new Vector3(clampedAngle,euler.y,euler.z);
            //Debug.Log(angle);

        }
        private void StartFire()
        {
            //Debug.Log("Start Fire");
            _isFiring = true;
        }

        private void StopFire()
        {
            //Debug.Log("Stop Fire");
            _settings.RotatorH.forward = _agentView.transform.forward;
            _settings.RotatorV.forward = _agentView.transform.forward;
            _isFiring = false;
        }

        private void ChangeState()
        {
            _stateController.ChangeState<IdleMoveState>();
        }

        private void CheckFire()
        {
            if (!_isFiring)
                return;

            if (_fireRate > 0)
            {
                _fireRate -= Time.deltaTime;

                if(_fireRate <= 0)
                {
                    Fire();
                    _fireRate = _fireSettings.Rate;
                }
            }
        }

        private void Fire()
        {
            CannonBall cannonBall = _cannonBallFactory.Create();
            cannonBall.Initialize(_fireSettings.Damage, _agentView.GetComponent<Agent>());

            cannonBall.transform.position = _agentView.CannonBallSpawnTransform.position;
            cannonBall.transform.rotation = _agentView.CannonBallSpawnTransform.rotation;
            cannonBall.SetLayer(LayerMask.NameToLayer("CannonBall"));

            Vector3 target = _targetEnemy.position;
            target.y += 0.75f;
            Vector3 direction = target - _agentView.CannonBallSpawnTransform.position;

            ParticleManager.CreateParticle<FireParticle>(_agentView.CannonBallSpawnTransform.position, null, false);

            //ParticleManager.CreateAndPlay(_particleSettings.FireCannonBallParticle, _agentView.CannonBallSpawnTransform, _agentView.CannonBallSpawnTransform.position);
            //ParticleManager.Instance.CreateWithFactory<CannonDamageParticle>(_particleSettings.ParticleFactory, ballPosition, null, false);

            Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
            rigidbody.AddForce(direction * _fireSettings.Range, ForceMode.Impulse);
            rigidbody.useGravity = false;

            //Debug.Log("ShootTo: "+_targetEnemy.position);
        }


        private float ClampAngle(float angle, float from, float to)
        {
            // accepts e.g. -80, 80
            if (angle < 0f) angle = 360 + angle;
            if (angle > 180f) return Mathf.Max(angle, 360 + from);
            return Mathf.Min(angle, to);
        }

        [Serializable]
        public struct Settings
        {
            public Transform RotatorH;
            public Transform RotatorV;
        }

    }
}
