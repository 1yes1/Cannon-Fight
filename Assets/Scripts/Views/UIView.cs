using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightUI
{
    public abstract class UIView : MonoBehaviour
    {
        public bool isPopup;

        public abstract void Initialize();

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}
