using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CannonFightBase
{
    public class DenemeEvent
    {
        public event Action<SkillType> Merhaba;

        public DenemeEvent()
        {
            Start();
        }

        private void Start()
        {
            DenemeEventCaller denemeEventCaller = new DenemeEventCaller(this);
        }


        public void RunEvent()
        {
            Debug.Log("Heyyy  RunEvent");
            Merhaba?.Invoke(SkillType.MultiBall);
            Debug.Log("Heyyy RunEvnt Deneme Çalýþtý");
        }

    }

    public class DenemeEventCaller
    {
        private DenemeEvent _denemeEvent;

        public DenemeEventCaller(DenemeEvent denemeEvent)
        {
            _denemeEvent = denemeEvent;

            //SkillTimer skillTimer = new SkillTimer(new Skill(4, OnTimeElapsed, Skills.MultiBall), 4, OnTimeElapsed);

        }

        private void OnTimeElapsed(Skill skill)
        {
            Debug.Log("Heeyy OnTimeElapsed");

            _denemeEvent.RunEvent();
        }


    }
}
