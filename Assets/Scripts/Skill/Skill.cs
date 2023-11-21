using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class Skill : ISkill
    {
        private Action<Skill> _onTimeElapsed;

        private float _timeElapsed;

        private float _time;

        private SkillTimer _skillTimer;

        private SkillType _skill;

        public Skill(float time, Action<Skill> onTimeElapsed,SkillType skill)
        {
            _onTimeElapsed = onTimeElapsed;
            _time = time;
            _skill = skill;
        }

        private void OnSkillTimeElapsed()
        {
            _onTimeElapsed?.Invoke(this);
        }

        public void Reset()
        {
            Debug.Log("Skill reset");
        }

        public bool IsEqualToSkill(SkillType skill)
        {
            if(_skill == skill)
                return true;
            else
                return false;
        }

        public void Tick()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed >= _time)
                OnSkillTimeElapsed();
        }
    }
}
