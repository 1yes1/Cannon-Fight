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
    public class Cannon : Character, ICannonBehaviour, IDamageable,IPotionCollector
    {
        private CannonManager _playerManager;

        private CannonView _cannonView;

        private MovementController _cannonController;

        private CannonSkillHandler _cannonSkillHandler;

        private IPotionCollector _potionCollector;

        private CannonDamageHandler _cannonDamageHandler;

        private PhotonView _photonView;

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

        public CannonManager PlayerManager => _playerManager;

        public PhotonView OwnPhotonView => _photonView;


        public int KillCount => _killCount;

        public int Health => _cannonDamageHandler.Health;

        public CannonSkillHandler CannonSkillHandler => _cannonSkillHandler;

        public Rigidbody Rigidbody => _cannonView.Rigidbody;


        [Inject]
        public void Construct(
            CannonSkillHandler cannonSkillHandler,
            MovementController cannonController,
            CannonDamageHandler cannonDamageHandler,
            CannonView cannonView)
        {
            _cannonSkillHandler = cannonSkillHandler;
            _potionCollector = _cannonSkillHandler;
            _cannonController = cannonController;
            _cannonDamageHandler = cannonDamageHandler;
            _cannonView = cannonView;
        }

        public void OnSpawn(CannonManager playerManager)
        {
            _playerManager = playerManager;
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        public void Start()
        {
            //Eðer Awake kullanýrsak,
            //IInitializable olan class larýn Initialize() Metodu
            //Normalde Startta çaðrýlmasý gerekirken Awake den önce çaðrýlýyor o yüzden hata alabiliyoruz

            CanDoAction = false;

            if (!_photonView.IsMine)
                return;

            LayerMask layerMask = LayerMask.NameToLayer("Player");
            gameObject.layer = layerMask;

        }


        public void Boost(float multiplier)
        {
            _cannonController.Boost(multiplier);
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer,Character attackerCannon)
        {
            _cannonDamageHandler.TakeDamage(damage,hitPoint,attackerPlayer, attackerCannon);
        }


        public void SetSkillHealth(int health)
        {
            _cannonDamageHandler.Health = health;
        }

        public void Die(Player attackerPlayer)
        {
            CannonManager playerManager = CannonManager.Find(attackerPlayer);
            playerManager.GetKill();

            _playerManager.OnDie(attackerPlayer);

            _cannonController.OnDie();

            IsDead = true;
        }

        public void Die(Character attackerCharacter)
        {
            _playerManager.OnDie(attackerCharacter);

            _cannonController.OnDie();

            IsDead = true;
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
            public DamageSkillParticle DamageSkillParticle;
            public MultiballSkillParticle MultiBallSkillParticle;
            public HealthSkillParticle HealthSkillParticle;
        }

        //Sahneden sürüklenecek bir þey varsa diye GameInstaller içinde Bind edilebilir
        [Serializable]
        public struct SceneSettings
        {
        }

        public class Factory : PlaceholderFactory<Cannon>
        {
        }


    }

}
