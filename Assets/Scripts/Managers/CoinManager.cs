using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using UnityEngine.SceneManagement;

namespace CannonFightBase
{
    public class CoinManager : IInitializable,IDisposable
    {
        private static CoinManager _instance;

        private AchievementManager.GameEndSettings _levelEndSettings;

        private SaveSettings _saveSettings;

        private CoinView _coinView;

        public static CoinManager Instance => _instance;

        public static int CurrentCoin
        {
            get
            {
                return CloudSaveManager.GetValue<int>(_instance._saveSettings.CurrentCoin,0);
            }
            set
            {
                CloudSaveManager.SetValue<int>(_instance._saveSettings.CurrentCoin, value);
                _instance._coinView.UpdateCoin(CurrentCoin);
            }
        }

        [Inject]
        public void Construct(SaveSettings saveSettings, AchievementManager.GameEndSettings levelEndSettings)
        {
            _saveSettings = saveSettings;
            _levelEndSettings = levelEndSettings;
        }

        public void Initialize()
        {
            if (_instance == null)
                _instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
            UpdateCoinView();
        }

        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _instance = null;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if(arg0.buildIndex == 0)
                UpdateCoinView();
        }

        public void UpdateCoinView()
        {
            _coinView = UIManager.GetView<CoinView>();
            if(_coinView != null)
                _coinView.UpdateCoin(CurrentCoin);
        }

        public void OnLoseTheGame()
        {
            CurrentCoin += Cannon.Current.KillCount * _levelEndSettings.LoserCoinMultiplier + _levelEndSettings.LoserCoinPrize;
        }

        public void OnWinTheGame()
        {
            CurrentCoin += Cannon.Current.KillCount * _levelEndSettings.WinnerCoinMultiplier + _levelEndSettings.WinnerCoinPrize;
        }

        public void SetDefaultValue()
        {
            CurrentCoin = 0;
        }


        [Serializable]
        public class SaveSettings
        {
            public string CurrentCoin;
        }

    }
}
