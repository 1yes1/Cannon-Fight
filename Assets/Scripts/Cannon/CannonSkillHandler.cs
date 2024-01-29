using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CannonSkillHandler : IInitializable,IPotionCollector,IDisposable,ITickable
    {
        private readonly Cannon _cannon;

        private readonly CannonView _cannonView;

        private readonly Settings _settings;

        private readonly DamageSkillParticle.Factory _damageSkillFactory;

        private readonly HealthSkillParticle.Factory _healthSkillFactory;

        private readonly MultiballSkillParticle.Factory _multiballSkillFactory;

        private readonly Transform _skillParticlePoint;

        private readonly CannonTraits _cannonTraits;

        private PoolableParticleBase _skillParticle;

        private List<Skill> _usingSkills;

        public CannonSkillHandler(Cannon cannon,
                                  CannonView cannonView,
                                  CannonTraits cannonTraits,
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
            _cannonTraits = cannonTraits;
            _skillParticlePoint = _cannonView.SkillParticlePoint;

        }

        public void Initialize()
        {
            _usingSkills = new List<Skill>();

            //OnSkillBarFilled(SkillType.Damage);
            GameEventReceiver.OnSkillBarFilledEvent += SetSkillProperty;

        }


        public void SetSkillProperty(SkillType skill)
        {
            OnSkillBarFilled(skill);
            AudioManager.PlaySound(GameSound.SkillStarted);

            if (skill == SkillType.Health)
            {
                _cannon.SetSkillHealth(_cannonTraits.Health);
            }
            else if (skill == SkillType.Damage)
            {
                //_cannonProperties.SetSkillFireDamage();
                Skill damageSkill = new Skill(_settings.DamageSkillSettings.DamageSkillTime, OnSkillTimeElapsed, SkillType.Damage);
                _usingSkills.Add(damageSkill);
            }
            else if (skill == SkillType.MultiBall)
            {
                //_fireController.SetMultiBallSkill();
                Skill multiBallSkill = new Skill(_settings.MultiBallSkillSettings.MultiBallSkillTime, OnSkillTimeElapsed, SkillType.MultiBall);
                _usingSkills.Add(multiBallSkill);
            }
        }

        public void OnSkillTimeElapsed(Skill skill)
        {
            Debug.Log("OnSkillTimeElapsed: "+skill.SkillType.ToString());
            
            _skillParticle.Dispose();

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
            switch (skill)
            {
                case SkillType.MultiBall:
                    _skillParticle = ParticleManager.CreateParticle<MultiballSkillParticle>( _skillParticlePoint.position, _skillParticlePoint, true);
                    GameEventCaller.Instance.OnBeforeSkillCountdownStarted(skill, _settings.MultiBallSkillSettings.MultiBallSkillTime);
                    break;
                case SkillType.Damage:
                    _skillParticle = ParticleManager.CreateParticle<DamageSkillParticle>(_skillParticlePoint.position, _skillParticlePoint, true);
                    GameEventCaller.Instance.OnBeforeSkillCountdownStarted(skill, _settings.DamageSkillSettings.DamageSkillTime);
                    break;
                case SkillType.Health:
                    _skillParticle = ParticleManager.CreateParticle<HealthSkillParticle>(_skillParticlePoint.position, _skillParticlePoint, false);
                    break;
                default:
                    break;
            }
        }


        public void Collect(Potion potion)
        {
            if (_cannon.OwnPhotonView.IsMine)
            {
                GameEventCaller.Instance.OnPotionCollected(potion);
                AudioManager.PlaySound(GameSound.CollectPoison);
            }

        }

        public void Dispose()
        {
            //Dispose etmemizin sebebi ayný OnDisable gibi.
            //Eðer burada unsubscribe etmezsek, derlendikten sonra bir kere çalýþýyor tüm bu GameEvent ile çalýþanlar,
            //daha sonra oyunu tekrar baþlatýnca çalýþmýyor
            GameEventReceiver.OnSkillBarFilledEvent -= SetSkillProperty;
        }

        public void FinishAllSkills()
        {
            for (int i = 0; i < _usingSkills.Count; i++)
            {
                OnSkillTimeElapsed(_usingSkills[i]);
            }
            _usingSkills.Clear();
        }

        public void Tick()
        {
            if(_usingSkills.Count > 0)
            {
                for (int i = 0; i < _usingSkills.Count; i++)
                    _usingSkills[i].Tick();
            }
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
