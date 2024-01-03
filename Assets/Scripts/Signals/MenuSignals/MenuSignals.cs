using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public struct OnLevelUpgradedSignal
    {
        public CannonLevelSettings cannonLevelSettings;
    }

    public struct OnCloudSavesLoadedSignal
    {
        public PlayerSaveData PlayerSaveData;
    }

}
