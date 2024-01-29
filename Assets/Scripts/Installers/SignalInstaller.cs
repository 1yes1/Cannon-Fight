using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class SignalInstaller : Installer<SignalInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<OnLevelUpgradedSignal>();
            Container.DeclareSignal<OnCloudSavesLoadedSignal>();
            Container.DeclareSignal<OnPlayGamesAuthanticated>();
            Container.DeclareSignal<OnFailedToLoadCloudSavesSignal>();
            Container.DeclareSignal<OnMainMenuOpenedSignal>();
            Container.DeclareSignal<OnGameMatchingStartedSignal>();
            Container.DeclareSignal<OnFirstOpeningSignal>();
            Container.DeclareSignal<OnButtonClickSignal>();
            Container.DeclareSignal<OnNicknameEnteredSignal>();
        }
    }
}
