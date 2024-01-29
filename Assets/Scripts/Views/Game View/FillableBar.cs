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
        [SerializeField] protected SkillType _skill;

        [SerializeField] protected Image _fillImage;

        [SerializeField] protected Image _skillIcon;

        [Range(1, 4)][SerializeField] private int _partCount = 2;

        public SkillType Skill => _skill;

        private Coroutine _coroutine;

        private void OnEnable()
        {
            GameEventReceiver.OnBeforeSkillCountdownStartedEvent += OnBeforeSkillCountdownStarted;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnBeforeSkillCountdownStartedEvent -= OnBeforeSkillCountdownStarted;
        }

        private void OnBeforeSkillCountdownStarted(SkillType skill, float time)
        {
            if(Skill == skill)
                _coroutine = StartCoroutine(SkillCountdown(time));
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
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _fillImage.fillAmount = 0;
            _fillImage.SetNativeSize();
        }

        public void FillBar(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _fillImage.fillAmount += 1f / _partCount;

                if (_fillImage.fillAmount >= 1)
                    OnSkillBarFilled();
            }
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
            _skillIcon.color = Color.gray;

        }


    }
}
