using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public abstract class PresenterBase : MonoBehaviour, IInitializable
    {
        public virtual void Initialize()
        {
        }
    }
}
