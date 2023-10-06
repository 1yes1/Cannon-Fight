using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class FireController : ITickable
    {
        private Settings _settings;

        private ParticleSettings _particleSettings;

        private CannonSkillHandler.Settings _cannonSkillHandlerSettings;

        private Transform _ballSpawnPoint;

        private Transform _crosshair;

        private Cannon _cannon;

        private CannonBall.Factory _cannonBallFactory;

        private PhotonView _photonView;

        private float _frequencyStatus = 0;

        private bool _useMultiBallSkil = false;

        private float _damageMultiplier = 1;

        public FireController(Cannon cannon,CannonBall.Factory cannonBallFactory,Settings settings,CannonSkillHandler.Settings cannonSkillHandlerSettings, ParticleSettings particleSettings)
        {
            _cannonBallFactory = cannonBallFactory;
            _cannon = cannon;
            _settings = settings;
            _ballSpawnPoint = _cannon.CannonBallSpawnPoint;
            _cannonSkillHandlerSettings = cannonSkillHandlerSettings;
            _particleSettings = particleSettings;

            GameEventReceiver.OnMobileFireButtonClickedEvent += StartFire;
            GameEventReceiver.OnSkillBarFilledEvent += OnSkillBarFilled;
            GameEventReceiver.OnSkillEndedEvent += OnSkillEnded;
            
            _crosshair = GameObject.FindWithTag("Crosshair").transform;
        }


        public void Initialize()
        {
            _photonView = _cannon.OwnPhotonView;
        }

        public void Tick()
        {
            if (!_cannon.OwnPhotonView.IsMine)
                return;

            if (_cannon.IsDead)
                return;

            if (!GameManager.Instance.useAndroidControllers)
            {
                if (InputManager.IsFiring && _frequencyStatus <= 0)
                {
                    if (_useMultiBallSkil)
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

        private void StartFire()
        {
            if(_crosshair == null)
                _crosshair = GameObject.FindWithTag("Crosshair").transform;

            Ray ray = Camera.main.ScreenPointToRay(_crosshair.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,300, ~_settings.FireIgnoreLayers))
            {
                Vector3 direction = hit.point - _ballSpawnPoint.position;
                direction.Normalize();

                Fire(_cannon.OwnPhotonView.Owner, _ballSpawnPoint.position, direction, _settings.FireRange, _settings.FireDamage);


                //Burada oluþturulan toplar default olarak OwnCannonBall layer ýna sahip
                //CannonBall cannonBall = _cannonBallFactory.Create();
                ////CannonBall cannonBall = Instantiate(_cannonProperties.CannonBall, _ballSpawnPoint.position, _ballSpawnPoint.rotation);
                //cannonBall.Initialize(_settings.FireDamage * _damageMultiplier, _cannon, _cannon.OwnPhotonView.Owner);

                //cannonBall.transform.position = _ballSpawnPoint.position;
                //cannonBall.transform.rotation = _ballSpawnPoint.rotation;

                //Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
                //rigidbody.AddForce(direction * _settings.FireRange, ForceMode.Impulse);

                _frequencyStatus = _settings.FireFrequency;


                GameEventCaller.Instance.OnPlayerFired();

                _cannon.OwnPhotonView.RPC(nameof(RPC_StartFire), RpcTarget.Others, _ballSpawnPoint.position, direction, _settings.FireRange, _settings.FireDamage);
            }
        }

        [PunRPC]
        private void RPC_StartFire(Vector3 ballPosition,Vector3 ballDirection, float fireRange, int fireDamage,PhotonMessageInfo info)
        {
            if (_cannon.OwnPhotonView.IsMine)
                return;

            Player owner = info.Sender;

            //CannonBall cannonBall = _cannonBallFactory.Create();
            //cannonBall.Initialize(fireDamage,_cannon, owner);
            Fire(owner, ballPosition, ballDirection, fireRange, fireDamage);

            //cannonBall.transform.position = _ballSpawnPoint.position;
            //cannonBall.transform.rotation = _ballSpawnPoint.rotation;

            //ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.fireCannonBallParticle, _ballSpawnPoint, ballPosition);

            //Burada diðer kullanýcýlarýn toplarý oluþturulduðu için bu toplar bize hasar vermeli. Yani own deðil normal cannonball olmalý
            //LayerMask layerMask = LayerMask.NameToLayer("CannonBall");
            //cannonBall.gameObject.layer = layerMask;

            //Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
            //rigidbody.AddForce(ballDirection * fireRange, ForceMode.Impulse);
        }


        private void Fire(Player owner,Vector3 ballPosition, Vector3 ballDirection, float fireRange, int fireDamage)
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
            _useMultiBallSkil = true;
        }

        public void ResetMultiBallSkill()
        {
            _useMultiBallSkil = false;
        }


        public void SetDamageSkill()
        {
            _damageMultiplier = _cannonSkillHandlerSettings.DamageSkillSettings.DamageMultiplier;
        }


        public void ResetDamageSkill()
        {
            _damageMultiplier = 1;
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

