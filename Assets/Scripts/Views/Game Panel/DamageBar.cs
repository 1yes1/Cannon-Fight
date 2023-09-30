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
            base.OnSkillBarFilled();
            GameEventCaller.Instance.OnSkillBarFilled(Skill);
            StartCoroutine(SkillCountdown());
        }


    }
}
