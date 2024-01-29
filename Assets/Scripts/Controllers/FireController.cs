using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace CannonFightBase
{

    public class FireController : IFixedTickable,IInitializable,IRpcMediator,IDisposable
    {
        private readonly Settings _settings;

        private readonly ParticleSettings _particleSettings;

        private readonly RPCMediator _rpcMediator;

        private readonly CannonSkillHandler.Settings _cannonSkillHandlerSettings;

        private readonly Cannon _cannon;

        private readonly CannonView _cannonView;

        private readonly CannonBall.Factory _cannonBallFactory;

        private readonly CannonTraits _cannonTraits;

        private GameObject _rotatorH;

        private GameObject _rotatorV;

        private Transform _ballSpawnPoint;

        private Transform _crosshair;

        private PhotonView _photonView;

        private float _fireRateStatus = 0;

        private bool _useMultiBallSkill = false;

        private int _fireDamage = 1;

        private const byte RPC_FIRE = 1;

        public Cannon Cannon => _cannon;

        public FireController(Cannon cannon,CannonView cannonView,CannonBall.Factory cannonBallFactory,Settings settings,CannonSkillHandler.Settings cannonSkillHandlerSettings, 
            ParticleSettings particleSettings,CannonTraits cannonTraits,RPCMediator rpcMediator)
        {
            _cannonBallFactory = cannonBallFactory;
            _cannon = cannon;
            _settings = settings;
            _cannonView = cannonView;
            _ballSpawnPoint = cannonView.CannonBallSpawnPoint;
            _cannonSkillHandlerSettings = cannonSkillHandlerSettings;
            _particleSettings = particleSettings;
            _rpcMediator = rpcMediator;
            _cannonTraits = cannonTraits;
        }

        public void Initialize()
        {
            _photonView = _cannonView.PhotonView;
            _rpcMediator.AddToRPC(RPC_FIRE, this);
            _crosshair = UIManager.GetView<CrosshairView>().transform;

            _cannonView.GetRotators(out _rotatorV, out _rotatorV);

            ResetDamageSkill();
            AddEvents();
        }

        public void LoadCannonTraits()
        {

        }

        private void AddEvents()
        {
            GameEventReceiver.OnMobileFireButtonClickedEvent += ReadyToFire;
            GameEventReceiver.OnSkillBarFilledEvent += OnSkillBarFilled;
            GameEventReceiver.OnSkillEndedEvent += OnSkillEnded;
        }

        public void Dispose()
        {
            GameEventReceiver.OnMobileFireButtonClickedEvent -= ReadyToFire;
            GameEventReceiver.OnSkillBarFilledEvent -= OnSkillBarFilled;
            GameEventReceiver.OnSkillEndedEvent -= OnSkillEnded;
        }

        public void FixedTick()
        {
            if (!_photonView.IsMine)
                return;

            if (!_cannon.CanDoAction)
                return;


            if(_fireRateStatus <= 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(_crosshair.position);
                //Debug.DrawRay(ray.origin, ray.direction * _settings.FireRange, Color.black);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin,ray.direction,out hit, _settings.FireRange))
                {
                    int layer = hit.transform.gameObject.layer;
                    if ((_settings.EnemyLayers.value & 1 << layer) > 0)
                        ReadyToFire(ray.direction);
                }

            #if UNITY_EDITOR
                if(InputManager.IsFiring)
                {
                    ReadyToFire();
                }
            #endif
            }


            if (_fireRateStatus > 0)
            {
                _fireRateStatus -= Time.deltaTime;
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


        private void ReadyToFire()
        {
            if (_fireRateStatus <= 0)
                ReadyToFire(Vector3.zero);
        }

        private void ReadyToFire(Vector3 direction = default)
        {
            if (_useMultiBallSkill)
            {
                _cannon.StartCoroutine(MultiBallFire());
            }
            else
                StartFire(direction);
        }

        private void StartFire(Vector3 direction = default)
        {
            if(direction == Vector3.zero)
                GetRayDirection(out direction);

            Fire(_cannon.OwnPhotonView.Owner, _ballSpawnPoint.position, direction, _settings.FireRange, _fireDamage);
            AudioManager.PlaySound(GameSound.CannonFire, _ballSpawnPoint.position);

            _fireRateStatus = _cannonTraits.FireRate;

            GameEventCaller.Instance.OnPlayerFired();

            //_cannon.OwnPhotonView.RPC(nameof(_rpcMediator.RPC_StartFire), RpcTarget.Others, new object[] { _ballSpawnPoint.position, direction, _settings.FireRange, _settings.FireDamage });

            _cannon.OwnPhotonView.RPC(nameof(_rpcMediator.RpcForwarder), RpcTarget.Others, RPC_FIRE, new object[] {_ballSpawnPoint.position, direction, _settings.FireRange, _fireDamage });
        }

        private void GetRayDirection(out Vector3 direction)
        {
            Ray ray = Camera.main.ScreenPointToRay(_crosshair.position);
            RaycastHit hit;
            direction = Vector3.negativeInfinity;

            if (Physics.Raycast(ray, out hit, 300, ~_settings.FireIgnoreLayers))
            {
                direction = hit.point - _ballSpawnPoint.position;
                direction.Normalize();
            }
        }

        public void RpcForwarder(object[] objects, PhotonMessageInfo info)
        {
            Fire(info.Sender, (Vector3)objects[0], (Vector3)objects[1], (float)objects[2], (int)objects[3]);
            AudioManager.PlaySound(GameSound.CannonFire, (Vector3)objects[0]);
        }

        public void Fire(Player owner,Vector3 ballPosition, Vector3 ballDirection, float fireRange, int fireDamage)
        {
            CannonBall cannonBall = _cannonBallFactory.Create();
            cannonBall.Initialize(fireDamage, _cannon, owner);

            cannonBall.transform.position = ballPosition;
            cannonBall.transform.rotation = _ballSpawnPoint.rotation;

            ParticleManager.CreateParticle<FireParticle>(ballPosition,_ballSpawnPoint, false);
            //ParticleManager.CreateAndPlay(_particleSettings.FireCannonBallParticle, _ballSpawnPoint, ballPosition);
            //ParticleManager.Instance.CreateWithFactory<CannonDamageParticle>(_particleSettings.ParticleFactory, ballPosition, null, false);

            Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
            rigidbody.AddForce(ballDirection * fireRange, ForceMode.Impulse);
            rigidbody.useGravity = false;
        }

        private void OnSkillBarFilled(SkillType skill)
        {
            if (skill == SkillType.MultiBall)
                SetMultiBallSkill();
            else if (skill == SkillType.Damage)
                SetDamageSkill();
        }


        private void OnSkillEnded(Skill skill)
        {
            if (skill.IsEqualToSkill(SkillType.MultiBall))
                ResetMultiBallSkill();
            else if (skill.IsEqualToSkill(SkillType.Damage))
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
            _fireDamage = _cannonTraits.Damage * _cannonSkillHandlerSettings.DamageSkillSettings.DamageMultiplier;
        }


        public void ResetDamageSkill()
        {
            _fireDamage = _cannonTraits.Damage;
            //Debug.Log("Damage: "+_cannonTraits.Damage);
        }

        [Serializable]
        public class Settings
        {
            public LayerMask EnemyLayers;

            public LayerMask FireIgnoreLayers;

            public float FireRange = 50;

            public float FireBallScale = 0.7f;
        }

        [Serializable]
        public struct ParticleSettings
        {
            public FireParticle FireParticle;
        }


    }
}

