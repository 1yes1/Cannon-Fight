using EasyButtons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public abstract class FillableBar: MonoBehaviour
    {
        [SerializeField] protected Skills _skill;

        [SerializeField] protected Image _fillImage;

        [SerializeField] protected Image _skillIcon;

        [Range(1, 4)][SerializeField] private int _partCount = 2;

        public Skills Skill => _skill;


        private void OnEnable()
        {
            GameEventReceiver.OnBeforeSkillCountdownStartedEvent += OnBeforeSkillCountdownStarted;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnBeforeSkillCountdownStartedEvent -= OnBeforeSkillCountdownStarted;
        }

        private void OnBeforeSkillCountdownStarted(Skills skill, float time)
        {
            if(Skill == skill)
                StartCoroutine(SkillCountdown(time));
        }

        private void Start()
        {
            ResetFilling();
        }


        //[Button]
        //public void Fill()
        //{
        //    FillOne();
        //}
        public void ResetFilling()
        {
            _fillImage.fillAmount = 0;
            _fillImage.SetNativeSize();
        }

        public void FillOne()
        {
            _fillImage.fillAmount += 1f/_partCount;

            if (_fillImage.fillAmount >= 1)
                OnSkillBarFilled();
        }

        public virtual void OnSkillBarFilled()
        {
            _skillIcon.color = Color.white;
            GameEventCaller.Instance.OnSkillBarFilled(Skill);
        }


        protected IEnumerator SkillCountdown(float time)
        {
            float skillTime = time;
            float count = skillTime;
            while (count > 0)
            {
                _fillImage.fillAmount = count / skillTime;
                count -= Time.deltaTime;
                yield return null;
            }

            _fillImage.fillAmount = 0;
        }


    }
}
