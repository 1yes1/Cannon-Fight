using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightUI
{
    public abstract class UIView : MonoBehaviour
    {
        public abstract void Initialize();

        public abstract void AddSubViews();

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);

        public void HideImmediately() => gameObject.SetActive(false);
    }
}
