using CartoonFX;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class Cannon : MonoBehaviour, IDamageable,ICannonBehaviour
    {
        public event Action OnDieEvent;

        [SerializeField] private GameObject _rotatorH;

        [SerializeField] private GameObject _rotatorV;

        [SerializeField] private GameObject _cannonBallSpawnPoint;

        [SerializeField] private float _health = 100;

        private PlayerManager _playerManager;

        private CannonController _cannonController;

        private CannonProperties _cannonProperties;

        private CannonSkillHandler _cannonSkillHandler;

        private FireController _fireController;

        private Animation _animation;

        private PhotonView _photonView;

        private bool _isDead = false;

        private int _killCount;

        public static Cannon Current
        {
            get
            {
                return GameManager.CurrentCannon;
            }
        }


        public static PhotonView PhotonView
        {
            get
            {
                return GameManager.CurrentCannon.GetComponent<PhotonView>();
            }
        }

        public PhotonView OwnPhotonView => _photonView;

        public float Health
        {
            get { return _health; }
            set
            {
                _health = (value < 0) ? 0 : value;

                //if (_photonView.IsMine)
                    GameEventCaller.Instance.OnOurPlayerHealthChanged();
            }
        }

        public bool IsDead => _isDead;

        public int KillCount => _killCount;

        public CannonSkillHandler CannonSkillHandler => _cannonSkillHandler;

        public GameObject CannonBallSpawnPoint => _cannonBallSpawnPoint;

        [Inject]
        public void Construct(FireController fireController, CannonSkillHandler cannonSkillHandler)
        {
            _fireController = fireController;
            _cannonSkillHandler = cannonSkillHandler;
        }

        public void OnSpawn(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();

            if (!_photonView.IsMine)
                return;

            LayerMask layerMask = LayerMask.NameToLayer("Player");
            gameObject.layer = layerMask;
            
            _cannonController = GetComponent<CannonController>();
            _cannonProperties = GetComponent<CannonProperties>();
            _animation = _rotatorV.GetComponent<Animation>();
        }



        private void OnEnable()
        {
            if (!_photonView.IsMine)
                return;

            _fireController.OnFireEvent += OnFire;

            //GameEventReceiver.OnSkillBarFilledEvent += SetSkillProperty;
        }

        private void OnDisable()
        {
            if (!_photonView.IsMine)
                return;

            //GameEventReceiver.OnSkillBarFilledEvent -= SetSkillProperty;
        }


        private void Start()
        {

        }

        public void GetRotators(out GameObject rotatorH, out GameObject rotatorV)
        {
            rotatorH = this._rotatorH;
            rotatorV = this._rotatorV;
        }

        public Vector3 GetFireForwardDirection()
        {
            return _rotatorV.transform.forward;
        }

        private void OnFire()
        {
            _animation.Stop();
            _animation.Play();
        }

        public void OnPotionCollected(Potion potion)
        {
            GameEventCaller.Instance.OnPotionCollected(potion);
        }



        public void TakeDamage(float damage, Vector3 hitPoint, Player attackerPlayer)
        {
            if (!_photonView.IsMine)
                return;

            Health -= damage;

            print("Get Damage: " + damage);

            ParticleSystem particleSystem = ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.takeDamageParticle, transform, hitPoint, false);
            particleSystem.GetComponent<CFXR_Effect>().cameraShake.enabled = true;

            if (Health <= 0)
            {
                Die(attackerPlayer);
            }

            _photonView.RPC(nameof(RPC_RunDamageParticle),RpcTarget.All,hitPoint);
        }

        private void Die(Player attackerPlayer)
        {
            //if (!_photonView.IsMine)
            //    return;
            if (_isDead)
                return;

            PlayerManager playerManager = PlayerManager.Find(attackerPlayer);
            playerManager.GetKill();

            _playerManager.OnDie(attackerPlayer);

            _isDead = true;
            OnDieEvent?.Invoke();
            print("---You Died!----");
        }




        [PunRPC]
        private void RPC_RunDamageParticle(Vector3 hitPoint)
        {
            if (_photonView.IsMine)
                return;

            //print(" Alaannnn: " + PhotonNetwork.NickName);

            ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.takeDamageParticle, null, hitPoint, false);
        }




    }

}
