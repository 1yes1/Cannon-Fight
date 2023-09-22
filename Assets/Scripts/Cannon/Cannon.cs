using CartoonFX;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class Cannon : MonoBehaviour, IDamageable,ICannonBehaviour
    {
        public event Action OnDieEvent;

        [SerializeField] private string denemeisim;

        [SerializeField] private GameObject _rotatorH;

        [SerializeField] private GameObject _rotatorV;

        [SerializeField] private float _health = 100;



        private CannonController _cannonController;

        private CannonProperties _cannonProperties;

        private FireController _fireController;

        private Animation _animation;

        private PhotonView _photonView;

        private bool _isDead = false;

        private int _killCount;

        private List<Skill> _usingSkills;


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


        public void OnSpawn()
        {
            denemeisim = PhotonNetwork.LocalPlayer.NickName;
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();

            if (!_photonView.IsMine)
                return;

            LayerMask layerMask = LayerMask.NameToLayer("Player");
            gameObject.layer = layerMask;

            _usingSkills = new List<Skill>();
            
            _cannonController = GetComponent<CannonController>();
            _cannonProperties = GetComponent<CannonProperties>();
            _animation = _rotatorV.GetComponent<Animation>();
            _fireController = GetComponent<FireController>();
            _fireController.OnFireEvent += OnFire;
        }

        private void OnEnable()
        {
            if (!_photonView.IsMine)
                return;

            GameEventReceiver.OnSkillBarFilledEvent += SetSkillProperty;
        }

        private void OnDisable()
        {
            if (!_photonView.IsMine)
                return;

            GameEventReceiver.OnSkillBarFilledEvent -= SetSkillProperty;
        }

        //private IEnumerator Start()
        //{
        //    yield return new WaitForSeconds(2);

        //    //Bu Gerekli sadece 1 kere çalýþmasý için
        //    if (_photonView.IsMine)
        //        print("Name: " + PhotonNetwork.LocalPlayer.NickName);

        //}

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

        private void SetSkillProperty(Skills skill)
        {
            if (skill == Skills.Health)
            {
                Health = 150;
            }
            else if (skill == Skills.Damage)
            {
                _cannonProperties.SetSkillFireDamage();
                Skill damageSkill = new Skill(GameManager.DefaultSkillProperties.DamageSkillTime,_cannonProperties.ResetSkillFireDamage, OnSkillTimeElapsed,Skills.Damage);
                damageSkill.Initialize();
                _usingSkills.Add(damageSkill);
            }
            else if (skill == Skills.MultiBall)
            {
                _fireController.SetMultiBallSkill();
                Skill multiBallSkill = new Skill(GameManager.DefaultSkillProperties.MultiBallSkillTime, _fireController.ResetMultiBallSkill, OnSkillTimeElapsed,Skills.MultiBall);
                multiBallSkill.Initialize();
                _usingSkills.Add(multiBallSkill);
            }
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

            _isDead = true;
            OnDieEvent?.Invoke();
            print("---You Died!----");
        }


        public void OnSkillTimeElapsed(Skill skill)
        {
            skill.Reset();
            print("Skill Sona Erdi");   
            _usingSkills.Remove(skill);
            GameEventCaller.Instance.OnSkillEnded(skill);
        }


        [PunRPC]
        private void RPC_RunDamageParticle(Vector3 hitPoint)
        {
            if (_photonView.IsMine)
                return;

            //print(" Alaannnn: " + PhotonNetwork.NickName);

            ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.takeDamageParticle, null, hitPoint, false);
        }


        public bool CanCollectPotion(Skills skill)
        {
            if (!_photonView.IsMine)
                return false;

            for (int i = 0; i < _usingSkills.Count; i++)
            {
                if (_usingSkills[i].IsEqualToSkill(skill))
                    return false;
            }
            return true;
        }

    }

}
