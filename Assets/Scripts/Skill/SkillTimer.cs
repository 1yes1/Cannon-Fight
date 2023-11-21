using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace CannonFightBase
{
    public class SkillTimer:IDisposable
    {
        public bool Started = false;

        private float _time;

        private readonly Timer _timer;

        public SkillTimer(Skill skill,float time,Action action) 
        {
            Debug.Log("Skill Başladı");
            _time = time;

            _timer = new Timer()
            {
                Interval = _time * 1000,
                AutoReset = false,
            };

            _timer.Elapsed += (o, args) =>
            {
                Debug.Log("Skill Bitti: " + skill.ToString());

                action?.Invoke();
                //Dispose();
            };

            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }


    }
}
