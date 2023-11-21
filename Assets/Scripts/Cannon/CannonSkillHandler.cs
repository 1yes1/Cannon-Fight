using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CannonSkillHandler : IInitializable,IPotionCollector
    {
        private readonly Cannon _cannon;

        private readonly CannonView _cannonView;

        private readonly Settings _settings;

        private readonly DamageSkillParticle.Factory _damageSkillFactory;

        private readonly HealthSkillParticle.Factory _healthSkillFactory;

        private readonly MultiballSkillParticle.Factory _multiballSkillFactory;

        private Transform _skillParticlePoint;

        private List<Skill> _usingSkills;

        public CannonSkillHandler(Cannon cannon,
                                  CannonView cannonView,
                                  Settings settings,
                                  DamageSkillParticle.Factory damageSkillFactory,
                                  HealthSkillParticle.Factory healthSkillFactory,
                                  MultiballSkillParticle.Factory multiballSkillFactory) 
        {
            _cannon = cannon;
            _cannonView = cannonView;
            _settings = settings;
            _damageSkillFactory = damageSkillFactory;
            _healthSkillFactory = healthSkillFactory;
            _multiballSkillFactory = multiballSkillFactory;



            Debug.Log("CannonSkillHandler Constructor Çlaýþtý ");
        }

        public void Initialize()
        {
            _usingSkills = new List<Skill>();
            //OnSkillBarFilled(SkillType.Damage);
            GameEventReceiver.OnSkillBarFilledEvent += SetSkillProperty;
            Debug.Log("CannonSkillHandler Initialize Çlaýþtý ");


        }


        public void SetSkillProperty(SkillType skill)
        {
            OnSkillBarFilled(skill);

            if (skill == SkillType.Health)
            {
                _cannon.SetSkillHealth(150);
            }
            else if (skill == SkillType.Damage)
            {
                //_cannonProperties.SetSkillFireDamage();
                Skill damageSkill = new Skill(_settings.DamageSkillSettings.DamageSkillTime, OnSkillTimeElapsed, SkillType.Damage);
                damageSkill.Initialize();
                _usingSkills.Add(damageSkill);
            }
            else if (skill == SkillType.MultiBall)
            {
                //_fireController.SetMultiBallSkill();
                Skill multiBallSkill = new Skill(_settings.MultiBallSkillSettings.MultiBallSkillTime, OnSkillTimeElapsed, SkillType.MultiBall);
                multiBallSkill.Initialize();
                _usingSkills.Add(multiBallSkill);
            }
        }

        public void OnSkillTimeElapsed(Skill skill)
        {
            Debug.Log("Heyy: OnSkillTimeElapsed");

            skill.Reset();
            _usingSkills.Remove(skill);
            GameEventCaller.Instance.OnSkillEnded(skill);
        }



        public bool CanCollectPotion(SkillType skill)
        {
            //if (!Cannon.PhotonView.IsMine)
            //    return false;

            for (int i = 0; i < _usingSkills.Count; i++)
            {
                if (_usingSkills[i].IsEqualToSkill(skill))
                    return false;
            }
            return true;
        }


        private void OnSkillBarFilled(SkillType skill)
        {
            _skillParticlePoint = _cannonView.SkillParticlePoint;
            
            Debug.Log(_damageSkillFactory);
            switch (skill)
            {
                case SkillType.MultiBall:
                    ParticleManager.CreateWithFactory<MultiballSkillParticle>(_multiballSkillFactory, _skillParticlePoint.position, _skillParticlePoint, true);
                    GameEventCaller.Instance.OnBeforeSkillCountdownStarted(skill, _settings.MultiBallSkillSettings.MultiBallSkillTime);
                    break;
                case SkillType.Damage:
                    ParticleManager.CreateWithFactory<DamageSkillParticle>(_damageSkillFactory, _cannonView.SkillParticlePoint.position, _cannonView.SkillParticlePoint, true);
                    GameEventCaller.Instance.OnBeforeSkillCountdownStarted(skill, _settings.DamageSkillSettings.DamageSkillTime);
                    break;
                case SkillType.Health:
                    ParticleManager.CreateWithFactory<HealthSkillParticle>(_healthSkillFactory, _skillParticlePoint.position, _skillParticlePoint, false);
                    break;
                default:
                    break;
            }
        }


        public void Collect(Potion potion)
        {
            if (_cannon.OwnPhotonView.IsMine)
                GameEventCaller.Instance.OnPotionCollected(potion);

        }


        [Serializable]
        public class Settings
        {
            public MultiBallSkillSettings MultiBallSkillSettings;
            public DamageSkillSettings DamageSkillSettings;
        }

        [Serializable]
        public class MultiBallSkillSettings
        {
            public int MultiBallCount = 3;

            public float MultiBallFrequency = 0.1f;

            public int MultiBallSkillTime = 10;
        }

        [Serializable]
        public class DamageSkillSettings
        {
            public int DamageMultiplier = 2;

            public int DamageSkillTime = 15;
        }

        public class HealthSkillSettings
        {
            public int MaxHealth = 100;
        }
    }
}
