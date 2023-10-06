using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannonFightBase
{
    public class Skill : ISkill
    {
        private Action<Skill> _onTimeElapsed;

        private float _time;

        private SkillTimer _skillTimer;

        private Skills _skill;

        public Skill(float time, Action<Skill> onTimeElapsed,Skills skill)
        {
            _onTimeElapsed += onTimeElapsed;
            _time = time;
            _skill = skill;
        }

        public void Initialize()
        {
            _skillTimer = new SkillTimer(this, _time, _onTimeElapsed);
        }

        public void Reset()
        {

        }

        public bool IsEqualToSkill(Skills skill)
        {
            if(_skill == skill)
                return true;
            else
                return false;
        }
    }
}
