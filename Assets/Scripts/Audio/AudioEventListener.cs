using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class AudioEventListener : IInitializable,IDisposable
    {
        private SignalBus _signalBus;

        public AudioEventListener(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            AudioManager.PlaySound(MenuSound.MenuMusic);
            _signalBus.Subscribe<OnButtonClickSignal>(OnButtonClick);
            _signalBus.Subscribe<OnLevelUpgradedSignal>(OnLevelUpgraded);
        }

        private void OnLevelUpgraded()
        {
            AudioManager.PlaySound(MenuSound.Upgrade);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OnButtonClickSignal>(OnButtonClick);
            _signalBus.Unsubscribe<OnLevelUpgradedSignal>(OnLevelUpgraded);
        }

        private void OnButtonClick()
        {
            AudioManager.PlaySound(MenuSound.ButtonClick);
        }

    }
}
