using EasyButtons;
using System;
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
        [SerializeField] private Skills _skill;

        [SerializeField] private Image _fillImage;

        [SerializeField] protected Image _skillIcon;

        [Range(1, 4)][SerializeField] private int _partCount = 2;

        public Skills Skill => _skill;

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

        public abstract void OnSkillBarFilled();

    }
}
