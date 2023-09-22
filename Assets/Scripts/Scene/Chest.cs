using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class Chest : MonoBehaviour, IDamageable
    {
        [SerializeField] private Skills _skill;
        [SerializeField] private Transform _potionPlace;
        [SerializeField] private Transform _potionTarget;
        [SerializeField] private float _health = 500;

        private Animation _animation;
        private Potion _potion;
        private bool _isOpened = true;
        private bool _isRefilling = false;
        private bool _isAlreadyOpened = false;

        private const string AnimChestOpen = "ChestOpen";
        private const string AnimChestHit = "ChestHit";
        private const string AnimChestFill = "ChestFill";

        public float LastOpenedTime {  get; private set; }

        public bool IsOpened => _isOpened;

        public bool IsAlreadyOpened => _isAlreadyOpened;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        public void Refill()
        {
            ChooseSkillRandomly();
            _animation.Play(AnimChestFill);
            _isRefilling = true;
            _isAlreadyOpened = true;
        }

        public void OnFillAnimationEnded()
        {
            _isOpened = false;
            _isRefilling = false;
            _health = GameManager.DefaultChestProperties.Health;
            _potion = Instantiate(ChestManager.Instance.GetPotion(_skill), _potionPlace.transform.position,Quaternion.identity);
            _potion.transform.SetParent(transform);
        }

        public void TakeDamage(float damage, Vector3 hitPoint, Player attackerPlayer)
        {
            ParticleSystem particleSystem = ParticleManager.Instance.CreateAndPlay(ParticleManager.Instance.takeDamageParticle, transform, hitPoint, false);
            if (_isOpened)
                return;

            if (_animation.IsPlaying(AnimChestHit))
                _animation.Stop();

            _animation.Play(AnimChestHit);

            _health -= damage;

            if (_health <= 0)
                Open();
        }


        public bool CanRefill()
        {
            return IsOpened && LastOpenedTime != 0 && !_isRefilling && LastOpenedTime + GameManager.DefaultChestProperties.RefillTime <= Time.realtimeSinceStartup;
        }


        private void Open()
        {
            _isOpened = true;
            LastOpenedTime = Time.realtimeSinceStartup;
            _potion.ShowUp(GameManager.DefaultChestProperties.PotionFlightTime, _potionTarget);
            _animation.Play(AnimChestOpen);
            GameEventCaller.Instance.OnChestOpened(this);
        }

        private void ChooseSkillRandomly()
        {
            _skill = (Skills)Random.Range(0, 3);
        }

    }
}
