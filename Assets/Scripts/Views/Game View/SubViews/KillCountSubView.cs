using CannonFightBase;
using CannonFightExtensions;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace CannonFightUI
{
    public class KillCountSubView : UISubView
    {
        [SerializeField] private GameObject _background;
        [SerializeField] private GameObject _skull;
        [SerializeField] private TextMeshProUGUI _killText;

        private AnimationSettings _animationSettings;
        private int _killCount;

        [Inject]
        private void Construct(AnimationSettings animationSettings)
        {
            _animationSettings = animationSettings;
        }
        public override void Initialize()
        {
        }

        public override void SetParameters(params object[] objects)
        {
            _killCount = (int)objects[0];
        }

        public override void Show(float subViewDelay)
        {
            _delay = subViewDelay;
            
            _skull.transform.localScale = Vector3.zero;
            _background.transform.localScale = Vector3.zero;
            _killText.transform.localScale = Vector3.zero;
            _killText.text = "0";

            Tweener backgroundAnimation = _background.transform.DOScale(_animationSettings.Background.Value, _animationSettings.Background.Duration).SetEase(_animationSettings.Background.Ease).SetDelay(_animationSettings.Background.Delay);
            Tweener skullAnimation = _skull.transform.DOScale(_animationSettings.SkullSettings.Vector, _animationSettings.SkullSettings.Duration).SetEase(_animationSettings.SkullSettings.Ease).SetDelay(_animationSettings.SkullSettings.Delay);
            Tweener killTextAnimation = _killText.transform.DOScale(Vector3.one, _animationSettings.KillTextSettings.Duration).SetEase(_animationSettings.KillTextSettings.Ease).OnComplete(() => { StartCoroutine(SetKillCount()); });

            Sequence sequence = DOTween.Sequence(this);
            sequence.Append(backgroundAnimation);
            sequence.Append(skullAnimation);
            sequence.Append(killTextAnimation);
            sequence.Play().SetDelay(_delay);
        }

        private IEnumerator SetKillCount()
        {
            yield return new WaitForSeconds(_animationSettings.KillTextSettings.Delay);
            int text = 0;

            while (text < _killCount)
            {
                text++;
                _killText.text = text.ToString();
                yield return new WaitForSeconds(_animationSettings.KillTextSettings.Value);
            }
        }


        [Serializable]
        public struct AnimationSettings
        {
            public TweenSettings Background;
            public TweenSettings SkullSettings;
            //public Ease ShowSkullEase;
            //public float SkullShowDuration;
            //public float SkullShowDelay;

            public TweenSettings KillTextSettings;

            //public Ease KillTextEase;
            //public float KillTextShowDuration;
            //public float KillTextDelay;
            //public float KillTextCountWaitSeconds;
        }

    }
}
