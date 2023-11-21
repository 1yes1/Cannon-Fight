using CartoonFX;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class Cannon : MonoBehaviour,IInitializable,ICannonBehaviour, IDamageable,IPotionCollector
    {
        private PlayerManager _playerManager;

        private CannonView _cannonView;

        private CannonController _cannonController;

        private CannonSkillHandler _cannonSkillHandler;

        private IPotionCollector _potionCollector;

        private CannonDamageHandler _cannonDamageHandler;

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
            CannonSkillHandler cannonSkillHandler,
            CannonController cannonController,
            CannonDamageHandler cannonDamageHandler,
            CannonView cannonView)
        {
            _cannonSkillHandler = cannonSkillHandler;
            _potionCollector = _cannonSkillHandler;
            _cannonController = cannonController;
            _cannonDamageHandler = cannonDamageHandler;
            _cannonView = cannonView;
        }

        public void OnSpawn(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }

        public void Initialize()
        {
            //Eðer Awake kullanýrsak,
            //IInitializable olan class larýn Initialize() Metodu
            //Normalde Startta çaðrýlmasý gerekirken Awake den önce çaðrýlýyor o yüzden hata alabiliyoruz

            _photonView = GetComponent<PhotonView>();
            print("Photon View Atandý");

            if (!_photonView.IsMine)
                return;

            LayerMask layerMask = LayerMask.NameToLayer("Player");
            gameObject.layer = layerMask;
        }

        public void Boost(float multiplier)
        {
            _cannonController.Boost(multiplier);
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer)
        {
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

            _isDead = true;

        }

        public bool CanCollectPotion(SkillType skill)
        {
            return _potionCollector.CanCollectPotion(skill);
        }

        public void Collect(Potion potion)
        {
            _potionCollector.Collect(potion);
        }

        [Serializable]
        public struct ParticleSettings
        {
            public ParticleSystem DamageSkillParticle;
            public ParticleSystem MultiBallSkillParticle;
            public ParticleSystem HealthSkillParticle;
        }

        public class Factory : PlaceholderFactory<Cannon>
        {
        }


    }

}
