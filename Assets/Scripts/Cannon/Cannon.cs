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
    public class Cannon : MonoBehaviour,ICannonBehaviour
    {
        [SerializeField] private GameObject _rotatorH;

        [SerializeField] private GameObject _rotatorV;

        [SerializeField] private Transform _cannonBallSpawnPoint;

        private PlayerManager _playerManager;

        private CannonController _cannonController;

        private CannonSkillHandler _cannonSkillHandler;

        private CannonDamageHandler _cannonDamageHandler;

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

        public PlayerManager PlayerManager => _playerManager;

        public PhotonView OwnPhotonView => _photonView;

        public bool IsDead => _isDead;

        public int KillCount => _killCount;

        public int Health => _cannonDamageHandler.Health;

        public CannonSkillHandler CannonSkillHandler => _cannonSkillHandler;

        public Transform CannonBallSpawnPoint => _cannonBallSpawnPoint;

        public Rigidbody Rigidbody => GetComponent<Rigidbody>();

        [Inject]
        public void Construct(
            FireController fireController, 
            CannonSkillHandler cannonSkillHandler,
            CannonController cannonController,
            CannonDamageHandler cannonDamageHandler)
        {
            _fireController = fireController;
            _cannonSkillHandler = cannonSkillHandler;
            _cannonController = cannonController;
            _cannonDamageHandler = cannonDamageHandler;
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
            
            _animation = _rotatorV.GetComponent<Animation>();

        }

        private void OnEnable()
        {
            if (!_photonView.IsMine)
                return;

            GameEventReceiver.OnPlayerFiredEvent += OnFire;

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

        public void Boost(float multiplier)
        {
            _cannonController.Boost(multiplier);
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer)
        {
            if (!_photonView.IsMine)
                return;

            _cannonDamageHandler.TakeDamage(damage,hitPoint,attackerPlayer);
        }


        public void SetSkillHealth(int health)
        {
            _cannonDamageHandler.Health = health;
        }

        public void Die(Player attackerPlayer)
        {
            PlayerManager playerManager = PlayerManager.Find(attackerPlayer);
            playerManager.GetKill();

            _playerManager.OnDie(attackerPlayer);

            _cannonController.OnDie();
        }

    }

}
