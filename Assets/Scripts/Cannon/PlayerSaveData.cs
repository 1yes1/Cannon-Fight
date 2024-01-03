using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    [Serializable]
    public class PlayerSaveData
    {
        public int CurrentCoin = 0;
        public CannonSaveData CannonSaveData;
    }

    [Serializable]
    public class CannonSaveData
    {
        public int DamageLevel = 1;
        public int SpeedLevel = 1;
        public int FireRateLevel = 1;
        public int HealthLevel = 1;
    }

    public interface ICannonDataLoader
    {
        public void SetCannonData();
    }

}