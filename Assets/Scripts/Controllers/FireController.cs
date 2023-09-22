using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class FireController : MonoBehaviour
    {
        public event Action OnFireEvent;

        [SerializeField] private LayerMask _fireIgnoreLayers;

        [SerializeField] private Transform _ballSpawnPoint;

        [SerializeField] private Transform _crosshair;
        
        private GamePanelView _gamePanel;//Bunun yerine On Fire metodu oluþtur

        private CannonProperties _cannonProperties;

        private PhotonView _photonView;

        private Cannon _cannon;

        private float _frequencyStatus = 0;


        private bool _useMultiBallSkil = false;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();

            _cannonProperties = GetComponent<CannonProperties>();
            _cannon = GetComponent<Cannon>();


            if (!_photonView.IsMine)
                return;

            _crosshair = GameObject.FindWithTag("Crosshair").transform;

        }

        void Start()
        {
            //_bottomPanel = UIManager.GetView<BottomPanelView>();

        }

        private void OnEnable()
        {
            GameEventReceiver.OnMobileFireButtonClickedEvent += Fire;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnMobileFireButtonClickedEvent -= Fire;
        }

        void Update()
        {
            if (!_photonView.IsMine)
                return;

            if (_cannon.IsDead)
                return;

            if(!GameManager.Instance.useAndroidControllers)
            {
                if (InputManager.IsFiring && _frequencyStatus <= 0)
                {
                    if(_useMultiBallSkil)
                    {
                        StartCoroutine("MultiBallFire");
                    }
                    else
                        Fire();
                }
            }

            if (_frequencyStatus > 0)
            {
                _frequencyStatus -= Time.deltaTime;
                float val = (_frequencyStatus / _cannonProperties.FireFrequency);
            }
        }

        //private void OnDrawGizmos()
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(_crosshair.position);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit, 300, ~_fireIgnoreLayers))
        //    {
        //        Vector3 direction = hit.point - _ballSpawnPoint.position;
        //        Debug.DrawRay(_ballSpawnPoint.position, direction * 5, Color.red);
        //    }
        //}

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
            Ray ray = Camera.main.ScreenPointToRay(_crosshair.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit,300, ~_fireIgnoreLayers))
            {
                Vector3 direction = hit.point - _ballSpawnPoint.position;
                direction.Normalize();
                //Burada oluþturulan toplar default olarak OwnCannonBall layer ýna sahip
                CannonBall cannonBall = Instantiate(_cannonProperties.CannonBall, _ballSpawnPoint.position, _ballSpawnPoint.rotation);
                cannonBall.Initialize(_cannonProperties.FireDamage, _cannon, _photonView.Owner);

                Rigidbody rigidbody = cannonBall.GetComponent<Rigidbody>();
                rigidbody.AddForce(direction * _cannonProperties.FireRange, ForceMode.Impulse);
                _frequencyStatus = _cannonProperties.FireFrequency;

                ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.fireCannonBallParticle, _ballSpawnPoint, Vector3.zero, false, true);

                OnFireEvent?.Invoke();

                _photonView.RPC("RPC_Fire", RpcTarget.Others, _ballSpawnPoint.position, direction, _cannonProperties.FireRange, _cannonProperties.FireDamage);
            }
        }

        [PunRPC]
        private void RPC_Fire(Vector3 ballPosition,Vector3 ballDirection, float fireRange, float fireDamage,PhotonMessageInfo info)
        {
            if (_photonView.IsMine)
                return;

            Player owner = info.Sender;

            print("Find Player: " + owner.NickName);

            CannonBall cannonBall = Instantiate(_cannonProperties.CannonBall, ballPosition, Quaternion.identity);
            cannonBall.Initialize(fireDamage,_cannon, owner);

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
            print("SetMultiBallSkill");
            _useMultiBallSkil = true;
        }

        public void ResetMultiBallSkill(Skill skill)
        {
            _useMultiBallSkil = false;
        }

    }
}

