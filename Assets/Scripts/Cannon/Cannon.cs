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
    public class Cannon : MonoBehaviour,ICannonBehaviour, IDamageable
    {
        private PlayerManager _playerManager;

        private CannonView _cannonView;

        private CannonController _cannonController;

        private CannonSkillHandler _cannonSkillHandler;

        private CannonDamageHandler _cannonDamageHandler;

        private FireController _fireController;

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

        public Rigidbody Rigidbody => _cannonView.Rigidbody;

        [Inject]
        public void Construct(
            FireController fireController, 
            CannonSkillHandler cannonSkillHandler,
            CannonController cannonController,
            CannonDamageHandler cannonDamageHandler,
            CannonView cannonView)
        {
            _fireController = fireController;
            _cannonSkillHandler = cannonSkillHandler;
            _cannonController = cannonController;
            _cannonDamageHandler = cannonDamageHandler;
            _cannonView = cannonView;
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



        public class Factory : PlaceholderFactory<Cannon>
        {
        }


    }

}
