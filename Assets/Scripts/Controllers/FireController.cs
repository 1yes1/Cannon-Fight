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
        public event Action OnFireEvent;

        private Transform _ballSpawnPoint;

        private Transform _crosshair;

        private Settings _settings;
        
        private Cannon _cannon;

        private CannonBall.Factory _cannonBallFactory;

        private float _frequencyStatus = 0;

        private bool _useMultiBallSkil = false;



        public FireController(Cannon cannon,CannonBall.Factory cannonBallFactory,Settings settings)
        {
            _cannonBallFactory = cannonBallFactory;
            _cannon = cannon;
            _settings = settings;
            _ballSpawnPoint = _cannon.CannonBallSpawnPoint.transform;


            GameEventReceiver.OnMobileFireButtonClickedEvent += Fire;
            
            _crosshair = GameObject.FindWithTag("Crosshair").transform;
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
                        Fire();
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
            while ( count < GameManager.DefaultSkillProperties.MultiBallCount)
            {
                Fire();
                count++;
                yield return new WaitForSeconds(GameManager.DefaultSkillProperties.MultiBallFrequency);
            }
        }

        private void Fire()
        {
            if(_crosshair == null)
            {
                _crosshair = GameObject.FindWithTag("Crosshair").transform;
                Debug.Log("GÝRDÝ ABÝ");
            }
            Ray ray = Camera.main.ScreenPointToRay(_crosshair.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,300, ~_settings.FireIgnoreLayers))
            {
                Vector3 direction = hit.point - _ballSpawnPoint.position;
                direction.Normalize();
                //Burada oluþturulan toplar default olarak OwnCannonBall layer ýna sahip
                CannonBall cannonBall = _cannonBallFactory.Create();
                //CannonBall cannonBall = Instantiate(_cannonProperties.CannonBall, _ballSpawnPoint.position, _ballSpawnPoint.rotation);
                cannonBall.Initialize(_settings.FireDamage, _cannon, _cannon.OwnPhotonView.Owner);

                cannonBall.transform.position = _ballSpawnPoint.position;
                cannonBall.transform.rotation = _ballSpawnPoint.rotation;

                Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
                rigidbody.AddForce(direction * _settings.FireRange, ForceMode.Impulse);
                _frequencyStatus = _settings.FireFrequency;

                ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.fireCannonBallParticle, _ballSpawnPoint, Vector3.zero, false, true);

                OnFireEvent?.Invoke();

                _cannon.OwnPhotonView.RPC("RPC_Fire", RpcTarget.Others, _ballSpawnPoint.position, direction, _settings.FireRange, _settings.FireDamage);
            }
        }

        [PunRPC]
        private void RPC_Fire(Vector3 ballPosition,Vector3 ballDirection, float fireRange, float fireDamage,PhotonMessageInfo info)
        {
            if (_cannon.OwnPhotonView.IsMine)
                return;

            Player owner = info.Sender;

            CannonBall cannonBall = _cannonBallFactory.Create();
            cannonBall.Initialize(fireDamage,_cannon, owner);

            cannonBall.transform.position = _ballSpawnPoint.position;
            cannonBall.transform.rotation = _ballSpawnPoint.rotation;

            ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.fireCannonBallParticle, _ballSpawnPoint, ballPosition);

            //Burada diðer kullanýcýlarýn toplarý oluþturulduðu için bu toplar bize hasar vermeli. Yani own deðil normal cannonball olmalý
            LayerMask layerMask = LayerMask.NameToLayer("CannonBall");
            cannonBall.gameObject.layer = layerMask;

            Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
            rigidbody.AddForce(ballDirection * fireRange, ForceMode.Impulse);
            rigidbody.useGravity = false;
        }

        public void SetMultiBallSkill()
        {
            _useMultiBallSkil = true;
        }

        public void ResetMultiBallSkill(Skill skill)
        {
            _useMultiBallSkil = false;
        }

        [Serializable]
        public class Settings
        {
            public LayerMask FireIgnoreLayers;

            public float FireFrequency = 0.75f;

            public float FireDamage = 10;

            public float FireRange = 50;

            public float FireBallScale = 0.7f;
        }

    }
}

