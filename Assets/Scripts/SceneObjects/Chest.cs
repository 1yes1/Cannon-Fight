using EasyButtons;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CannonFightBase
{
    public class Chest : MonoBehaviour, IDamageable
    {
        public event Action<Chest> OnChestFilled;

        [SerializeField] private Skills _skill;

        [SerializeField] private Transform _potionPlace;

        [SerializeField] private Transform _potionTarget;

        [SerializeField] private float _health = 500;

        private Settings _settings;

        private ParticleSettings _particleSettings;

        private AnimatorSettings _animatorSettings;

        private Animator _animator;

        private Potion _potion;

        private Potion.Factory _potionFactory;

        private Potion.Settings _potionSettings;

        private bool _isOpened = false;

        private bool _isRefilling = false;

        private bool _isFilled = false;

        private const string AnimChestOpen = "ChestOpen";

        private const string AnimChestHit = "ChestHit";

        private const string AnimChestFill = "ChestFill";

        public float LastOpenedTime {  get; private set; }

        public bool IsOpened => _isOpened;

        public bool IsFilled => _isFilled;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        [Inject]
        public void Construct(Settings settings, Potion.Factory potionFactory, Potion.Settings potionSettings,ParticleSettings particleSettings, AnimatorSettings animatorSettings)
        {
            _settings = settings;
            _potionFactory = potionFactory;
            _potionSettings = potionSettings;
            _particleSettings = particleSettings;
            _animatorSettings = animatorSettings;
        }

        private void Open()
        {
            _isOpened = true;
            LastOpenedTime = Time.realtimeSinceStartup;

            if(_potion != null)
                _potion.ShowUp(_settings.PotionFlightTime, _potionTarget);

            _animator.SetTrigger(_animatorSettings.OpenChest);
            GameEventCaller.Instance.OnChestOpened(this);
        }

        public void Refill()
        {
            ChooseSkillRandomly();
            _animator.SetTrigger(_animatorSettings.FillChest);
            _isRefilling = true;
            _isOpened = true; //Ýlk baþta açýlmýþ olsun sonradan doldurulsun
            _isFilled = true;
        }

        public void OnFillAnimationEnded()
        {
            _isOpened = false;
            _isRefilling = false;
            _health = _settings.Health;
            _potion = _potionFactory.Create();
            _potion.Initialize(GetRandomPotionProperties());
            //_potion = Instantiate(ChestManager.Instance.GetPotion(_skill), _potionPlace.transform.position,Quaternion.identity);
            _potion.transform.position = _potionPlace.transform.position;
            _potion.transform.rotation = Quaternion.identity;
            _potion.transform.SetParent(transform);
            OnChestFilled?.Invoke(this);
        }

        [Button]
        public void SetOpenState(bool isOpened)
        {
            if(isOpened)
            {
                Open();

                if (_potion == null)
                    return;
                _potion.Dispose();
                _potion = null;
            }
            else
            {
                //_animator.speed = 99;
                //_animator.Play(_animatorSettings.OpenChest, -1,100);
                //_animation.Sample();
                //_animation.Stop();
                //Potion da atanmalý
            }
        }

        private Potion.PotionType GetRandomPotionProperties()
        {
            int rnd = Random.Range(0, _potionSettings.PotionTypes.Count);

            return _potionSettings.PotionTypes[rnd];
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Player attackerPlayer)
        {
            ParticleSystem particleSystem = ParticleManager.CreateAndPlay(_particleSettings.TakeDamageParticle, transform, hitPoint, false);
            if (_isOpened)
                return;

            //if (_animation.IsPlaying(AnimChestHit))
            //    _animation.Stop();

            //_animation.Play(AnimChestHit);

            _health -= damage;

            if (_health <= 0)
                Open();
        }


        public bool CanRefill()
        {
            return IsOpened && LastOpenedTime != 0 && !_isRefilling && LastOpenedTime + _settings.RefillTime <= Time.realtimeSinceStartup;
        }



        private void ChooseSkillRandomly()
        {
            _skill = (Skills)Random.Range(0, 3);
        }


        [Serializable]
        public class Settings
        {
            [Header("Chest")]

            public float Health = 50;

            [Header("Chest Potion")]

            public float PotionFlightTime = 0.8f;

            [Header("Fill Chests")]

            public float StartFillTime = 3;

            public float StartFillFrequency = 2;

            public float RefillTime = 5;

        }

        [Serializable]
        public class ParticleSettings
        {
            public ParticleSystem TakeDamageParticle;
        }

        [Serializable]
        public class AnimatorSettings
        {
            public string OpenChest = "OpenChest";
            public string HitChest = "HitChest";
            public string FillChest = "FillChest";
        }

    }
}
