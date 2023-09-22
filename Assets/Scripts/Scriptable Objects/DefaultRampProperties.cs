using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "DefaultRampProperties", menuName = "CannonFight/DefaultRampProperties", order = 1)]
    public class DefaultRampProperties : ScriptableObject
    {
        [Header("Ramp")]

        public float BoostMultiplier = 25;


    }
}
