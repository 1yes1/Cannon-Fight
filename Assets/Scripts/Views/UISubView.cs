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

        public virtual void Show(float subViewDelay) => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);

        public abstract void Initialize();
    }
}
