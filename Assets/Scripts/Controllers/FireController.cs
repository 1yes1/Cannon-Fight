using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

namespace CannonFightBase
{

    public class FireController : ITickable,IInitializable,IRpcMediator
    {
        private Settings _settings;

        private ParticleSettings _particleSettings;

        private RPCMediator _rpcMediator;

        private CannonSkillHandler.Settings _cannonSkillHandlerSettings;

        private Transform _ballSpawnPoint;

        private Transform _crosshair;

        private Cannon _cannon;

        private CannonView _cannonView;

        private CannonBall.Factory _cannonBallFactory;

        private PhotonView _photonView;

        private float _frequencyStatus = 0;

        private bool _useMultiBallSkill = false;

        private int _fireDamage = 1;

        private const byte RPC_FIRE = 1;

        public Cannon Cannon => _cannon;

        public FireController(Cannon cannon,CannonView cannonView,CannonBall.Factory cannonBallFactory,Settings settings,CannonSkillHandler.Settings cannonSkillHandlerSettings, ParticleSettings particleSettings,RPCMediator rpcMediator)
        {
            _cannonBallFactory = cannonBallFactory;
            _cannon = cannon;
            _settings = settings;
            _cannonView = cannonView;
            _ballSpawnPoint = cannonView.CannonBallSpawnPoint;
            _cannonSkillHandlerSettings = cannonSkillHandlerSettings;
            _particleSettings = particleSettings;
            _rpcMediator = rpcMediator;
        }

        public void Initialize()
        {
            _photonView = _cannonView.PhotonView;
            _rpcMediator.AddToRPC(RPC_FIRE, this);
            _crosshair = GameObject.FindWithTag("Crosshair").transform;

            ResetDamageSkill();

            AddEvents();
        }

        private void AddEvents()
        {
            GameEventReceiver.OnMobileFireButtonClickedEvent -= StartFire;
            GameEventReceiver.OnSkillBarFilledEvent -= OnSkillBarFilled;
            GameEventReceiver.OnSkillEndedEvent -= OnSkillEnded;

            GameEventReceiver.OnMobileFireButtonClickedEvent += StartFire;
            GameEventReceiver.OnSkillBarFilledEvent += OnSkillBarFilled;
            GameEventReceiver.OnSkillEndedEvent += OnSkillEnded;

        }

        public void Tick()
        {
            if (!_photonView.IsMine)
                return;

            if (_cannon.IsDead)
                return;

            if (!GameManager.Instance.useAndroidControllers)
            {
                if (InputManager.IsFiring && _frequencyStatus <= 0)
                {
                    if (_useMultiBallSkill)
                    {
                        _cannon.StartCoroutine(MultiBallFire());
                    }
                    else
                        StartFire();
                }
            }

            if (_frequencyStatus > 0)
            {
                _frequencyStatus -= Time.deltaTime;
            }
        }

        IEnumerator MultiBallFire()
        {
            int count = 0;
            while ( count < _cannonSkillHandlerSettings.MultiBallSkillSettings.MultiBallCount)
            {
                StartFire();
                count++;
                yield return new WaitForSeconds(_cannonSkillHandlerSettings.MultiBallSkillSettings.MultiBallFrequency);
            }
        }

        public void StartFire()
        {
            if(_crosshair == null)
                _crosshair = GameObject.FindWithTag("Crosshair").transform;

            Ray ray = Camera.main.ScreenPointToRay(_crosshair.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,300, ~_settings.FireIgnoreLayers))
            {
                Vector3 direction = hit.point - _ballSpawnPoint.position;
                direction.Normalize();

                Fire(_cannon.OwnPhotonView.Owner, _ballSpawnPoint.position, direction, _settings.FireRange, _fireDamage);

                _frequencyStatus = _settings.FireFrequency;

                GameEventCaller.Instance.OnPlayerFired();

                //_cannon.OwnPhotonView.RPC(nameof(_rpcMediator.RPC_StartFire), RpcTarget.Others, new object[] { _ballSpawnPoint.position, direction, _settings.FireRange, _settings.FireDamage });

                _cannon.OwnPhotonView.RPC(nameof(_rpcMediator.RpcForwarder), RpcTarget.Others, RPC_FIRE, new object[] {_ballSpawnPoint.position, direction, _settings.FireRange, _fireDamage });
            }
        }

        public void RpcForwarder(object[] objects, PhotonMessageInfo info)
        {
            Fire(info.Sender, (Vector3)objects[0], (Vector3)objects[1], (float)objects[2], (int)objects[3]);
        }

        public void Fire(Player owner,Vector3 ballPosition, Vector3 ballDirection, float fireRange, int fireDamage)
        {
            CannonBall cannonBall = _cannonBallFactory.Create();
            cannonBall.Initialize(fireDamage, _cannon, owner);

            cannonBall.transform.position = ballPosition;
            cannonBall.transform.rotation = _ballSpawnPoint.rotation;

            ParticleManager.CreateAndPlay(_particleSettings.FireCannonBallParticle, _ballSpawnPoint, ballPosition);
            //ParticleManager.Instance.CreateWithFactory<CannonDamageParticle>(_particleSettings.ParticleFactory, ballPosition, null, false);

            Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
            rigidbody.AddForce(ballDirection * fireRange, ForceMode.Impulse);
            rigidbody.useGravity = false;
        }

        private void OnSkillBarFilled(Skills skill)
        {
            if (skill == Skills.MultiBall)
                SetMultiBallSkill();
            else if (skill == Skills.Damage)
                SetDamageSkill();
        }


        private void OnSkillEnded(Skill skill)
        {
            if (skill.IsEqualToSkill(Skills.MultiBall))
                ResetMultiBallSkill();
            else if (skill.IsEqualToSkill(Skills.Damage))
                ResetDamageSkill();
        }

        public void SetMultiBallSkill()
        {
            _useMultiBallSkill = true;
        }

        public void ResetMultiBallSkill()
        {
            _useMultiBallSkill = false;
        }


        public void SetDamageSkill()
        {
            _fireDamage = _settings.FireDamage * _cannonSkillHandlerSettings.DamageSkillSettings.DamageMultiplier;
        }


        public void ResetDamageSkill()
        {
            _fireDamage = _settings.FireDamage;
        }


        [Serializable]
        public class Settings
        {
            public LayerMask FireIgnoreLayers;

            public float FireFrequency = 0.75f;

            public int FireDamage = 10;

            public float FireRange = 50;

            public float FireBallScale = 0.7f;
        }

        [Serializable]
        public class ParticleSettings
        {
            public ParticleSystem FireCannonBallParticle;

            [Inject]
            public CannonDamageParticle.Factory ParticleFactory;

        }


    }
}

