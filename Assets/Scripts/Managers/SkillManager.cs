using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class SkillManager : MonoBehaviour
    {
        private FillableBar[] _fillableBars;

        public void OnEnable()
        {
            GameEventReceiver.OnPotionCollectedEvent += OnPotionCollected;
            GameEventReceiver.OnSkillEndedEvent += OnSkillEnded;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnPotionCollectedEvent -= OnPotionCollected;
            GameEventReceiver.OnSkillEndedEvent -= OnSkillEnded;
        }

        private void OnPotionCollected(Potion potion)
        {
            if (potion.Skill == SkillType.Health)
            {
                GameEventCaller.Instance.OnSkillBarFilled(potion.Skill);
            }
            else
            {
                FillableBar fillableBar = GetFillableBar(potion.Skill);
                fillableBar.FillBar(potion.FillCount);
            }
        }

        private void OnSkillEnded(Skill skill)
        {
            //FillableBar fillableBar = FindFillableBar(skill);
            //fillableBar.ResetFilling();
            //E�er ResetFilling �al��t�r�rsak event devam etmiyor. Hata da vermiyor ama s�k�nt�l�
        }

        private void Awake()
        {
            _fillableBars = GetComponentsInChildren<FillableBar>();
        }

        private FillableBar GetFillableBar(SkillType skill)
        {
            for (int i = 0; i < _fillableBars.Length; i++)
            {
                if (_fillableBars[i].Skill == skill)
                    return _fillableBars[i];
            }
            return null;
        }


        private FillableBar FindFillableBar(Skill skill)
        {
            for (int i = 0; i < _fillableBars.Length; i++)
            {
                if (skill.IsEqualToSkill(_fillableBars[i].Skill))
                    return _fillableBars[i];
            }
            return null;
        }

    }
}
