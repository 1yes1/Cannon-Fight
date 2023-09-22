using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "DefaultChestProperties", menuName = "CannonFight/DefaultChestProperties", order = 1)]
    public class DefaultChestProperties: ScriptableObject
    {
        [Header("Chest")]

        public float Health = 50;

        [Header("Chest Potion")]

        public float PotionFlightTime = 0.8f;

        [Header("Fill Chests")]

        public float StartFillTime = 3;

        public float StartFillFrequency = 2;

        public float RefillTime = 5;

    }
}
