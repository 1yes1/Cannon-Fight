using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public struct OnLevelUpgradedSignal
    {
        public CannonLevelSettings cannonLevelSettings;
    }
    public struct OnPlayGamesAuthanticated { public bool IsAuthanticated; }

    public struct OnCloudSavesLoadedSignal
    {
        public PlayerSaveData PlayerSaveData;
    }

    public struct OnFailedToLoadCloudSavesSignal { }
    public struct OnMainMenuOpenedSignal { }
    public struct OnGameMatchingStartedSignal { }
    public struct OnFirstOpeningSignal { }
    public struct OnButtonClickSignal { }
    public struct OnNicknameEnteredSignal { }

}
