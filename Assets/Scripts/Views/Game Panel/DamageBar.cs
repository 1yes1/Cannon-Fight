using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class DamageBar : FillableBar
    {
        public override void OnSkillBarFilled()
        {
            GameEventCaller.Instance.OnSkillBarFilled(Skill);
            _skillIcon.color = Color.white;
        }
    }
}
