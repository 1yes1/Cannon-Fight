using CannonFightBase;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    [Serializable]
    public struct TweenSettings
    {
        public Ease Ease;
        public float Delay;
        public float Duration;
        public float Value;
        public Vector3 Vector;
    }

    [Serializable]
    public struct SequenceSettings
    {
        public List<TweenSettings> Sequence;

        public int Count => Sequence.Count;
    }

    public static class TweenSettingsModifier
    {

        public static Tweener DOScale(this Transform tweenTransform, TweenSettings tweenSettings, bool fromZero = false, bool useStartScale = false)
        {
            Vector3 startScale = tweenTransform.localScale;
            Vector3 endScale = tweenSettings.Value * Vector3.one;

            if (fromZero)
                tweenTransform.localScale = Vector3.zero;

            if (useStartScale)
                endScale = startScale;


            return tweenTransform.DOScale(endScale, tweenSettings.Duration).SetDelayAndEase(tweenSettings);
        }

        public static Tweener DOAnchorPos(this RectTransform tweenTransform, TweenSettings tweenSettings)
        {
            return tweenTransform.DOAnchorPos(tweenSettings.Vector, tweenSettings.Duration).SetDelayAndEase(tweenSettings);
        }

        public static Tweener DOPunchScale(this Transform tweenTransform, TweenSettings tweenSettings)
        {
            return tweenTransform.DOPunchScale(tweenSettings.Vector, tweenSettings.Duration, 5, 0.5f).SetDelayAndEase(tweenSettings);
        }

        public static Tweener DORotate(this Transform tweenTransform, TweenSettings tweenSettings)
        {
            return tweenTransform.DORotate(tweenSettings.Vector, tweenSettings.Duration,RotateMode.FastBeyond360).SetDelayAndEase(tweenSettings);
        }

        public static Tweener SetDelayAndEase(this Tweener tweener, TweenSettings tweenSettings)
        {
            return tweener.SetDelay(tweenSettings.Delay).SetEase(tweenSettings.Ease);
        }

    }

}
