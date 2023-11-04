using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class AchievementManager : MonoBehaviour
    {

        [Serializable]
        public class Settings
        {
            public LevelEndSettings levelEndSettings;
        }

        [Serializable]
        public class LevelEndSettings
        {
            public int WinnerCoinMultiplier = 25;
            public int LoserCoinMultiplier = 10;
        }

    }
}
