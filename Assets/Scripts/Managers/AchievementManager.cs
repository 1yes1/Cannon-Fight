using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class AchievementManager
    {
        [Serializable]
        public struct Settings
        {
            public GameEndSettings GameEndSettings;
        }

        [Serializable]
        public struct GameEndSettings
        {
            public int WinnerCoinPrize;
            public int WinnerCoinMultiplier;
            public int LoserCoinPrize;
            public int LoserCoinMultiplier;
        }

    }
}
