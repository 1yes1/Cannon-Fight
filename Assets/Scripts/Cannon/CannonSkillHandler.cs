using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class CannonSkillHandler
    {
        private Cannon _cannon;

        private List<Skill> _usingSkills;

        public CannonSkillHandler(Cannon cannon) 
        {
            _cannon = cannon;

            _usingSkills = new List<Skill>();
        }

        private void SetSkillProperty(Skills skill)
        {
            if (skill == Skills.Health)
            {
                _cannon.Health = 150;
            }
            else if (skill == Skills.Damage)
            {
                //_cannonProperties.SetSkillFireDamage();
                Skill damageSkill = new Skill(GameManager.DefaultSkillProperties.DamageSkillTime, ResetSkillFireDamage, OnSkillTimeElapsed, Skills.Damage);
                damageSkill.Initialize();
                _usingSkills.Add(damageSkill);
            }
            else if (skill == Skills.MultiBall)
            {
                //_fireController.SetMultiBallSkill();
                Skill multiBallSkill = new Skill(GameManager.DefaultSkillProperties.MultiBallSkillTime, ResetMultiBallSkill, OnSkillTimeElapsed, Skills.MultiBall);
                multiBallSkill.Initialize();
                _usingSkills.Add(multiBallSkill);
            }
        }

        private void ResetSkillFireDamage(Skill skill)
        {

        }

        private void ResetMultiBallSkill(Skill skill)
        {

        }

        public void OnSkillTimeElapsed(Skill skill)
        {
            skill.Reset();
            _usingSkills.Remove(skill);
            GameEventCaller.Instance.OnSkillEnded(skill);
        }



        public bool CanCollectPotion(Skills skill)
        {
            if (Cannon.PhotonView.IsMine)
                return false;

            for (int i = 0; i < _usingSkills.Count; i++)
            {
                if (_usingSkills[i].IsEqualToSkill(skill))
                    return false;
            }
            return true;
        }




        public class Settings
        {

        }

    }
}
