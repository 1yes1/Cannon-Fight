using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class CannonSkillHandler
    {
        private Cannon _cannon;

        private List<Skill> _usingSkills;

        private Settings _settings;

        public CannonSkillHandler(Cannon cannon,Settings settings) 
        {
            _cannon = cannon;
            _settings = settings;
            _usingSkills = new List<Skill>();

            GameEventReceiver.OnSkillBarFilledEvent += SetSkillProperty;
        }

        private void SetSkillProperty(Skills skill)
        {
            OnSkillBarFilled(skill);

            if (skill == Skills.Health)
            {
                _cannon.SetSkillHealth(150);
            }
            else if (skill == Skills.Damage)
            {
                //_cannonProperties.SetSkillFireDamage();
                Skill damageSkill = new Skill(_settings.DamageSkillSettings.DamageSkillTime, OnSkillTimeElapsed, Skills.Damage);
                damageSkill.Initialize();
                _usingSkills.Add(damageSkill);
            }
            else if (skill == Skills.MultiBall)
            {
                //_fireController.SetMultiBallSkill();
                Skill multiBallSkill = new Skill(_settings.MultiBallSkillSettings.MultiBallSkillTime, OnSkillTimeElapsed, Skills.MultiBall);
                multiBallSkill.Initialize();
                _usingSkills.Add(multiBallSkill);
            }
        }

        public void OnSkillTimeElapsed(Skill skill)
        {
            skill.Reset();
            _usingSkills.Remove(skill);
            GameEventCaller.Instance.OnSkillEnded(skill);
        }



        public bool CanCollectPotion(Skills skill)
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


        private void OnSkillBarFilled(Skills skill)
        {
            if (skill == Skills.MultiBall)
                GameEventCaller.Instance.OnBeforeSkillCountdownStarted(skill,_settings.MultiBallSkillSettings.MultiBallSkillTime);
            else if(skill == Skills.Damage)
                GameEventCaller.Instance.OnBeforeSkillCountdownStarted(skill,_settings.DamageSkillSettings.DamageSkillTime);
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
