using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightUI
{
    public abstract class UISubView : MonoBehaviour
    {
        protected float _delay;

        public abstract void Show(float subViewDelay);

        public abstract void Initialize();
    }

    [Serializable]
    public partial struct TweenSettings
    {
        public Ease Ease;
        public float Delay;
        public float Duration;
        public float Value;
        public Vector3 Vector;
    }

}
