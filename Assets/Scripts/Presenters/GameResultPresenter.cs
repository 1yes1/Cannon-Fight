using CannonFightUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static CannonFightBase.AchievementManager;

namespace CannonFightBase
{
    public class GameResultPresenter : PresenterBase
    {
        private GameEndSettings _levelEndSettings;

        private CoinGainSubView _coinGainSubView;

        private KillCountSubView _killCountSubView;

        [Inject]
        public void Construct(AchievementManager.GameEndSettings levelEndSettings)
        {
            _levelEndSettings = levelEndSettings;
        }

        public override void Initialize()
        {
            GameEventReceiver.OnWinTheGameEvent += OnWinTheGame;
            GameEventReceiver.OnLoseTheGameEvent += OnLoseTheGame;
        }

        public void OnDisable()
        {
            GameEventReceiver.OnWinTheGameEvent -= OnWinTheGame;
            GameEventReceiver.OnLoseTheGameEvent -= OnLoseTheGame;
        }

        private void OnLoseTheGame()
        {
            _coinGainSubView = UIManager.GetSubView<DefeatedPanelView, CoinGainSubView>();
            _killCountSubView = UIManager.GetSubView<DefeatedPanelView, KillCountSubView>();

            int coinCount = CannonManager.Current.KillCount * _levelEndSettings.LoserCoinMultiplier + _levelEndSettings.LoserCoinPrize;
            CoinManager.Instance.OnLoseTheGame();

            SetSubViewParameters(coinCount, CannonManager.Current.KillCount);
        }

        private void OnWinTheGame()
        {
            _coinGainSubView = UIManager.GetSubView<VictoryPanelView, CoinGainSubView>();
            _killCountSubView = UIManager.GetSubView<VictoryPanelView, KillCountSubView>();

            int coinCount = CannonManager.Current.KillCount * _levelEndSettings.WinnerCoinMultiplier + _levelEndSettings.WinnerCoinPrize;

            CoinManager.Instance.OnWinTheGame();

            SetSubViewParameters(coinCount, CannonManager.Current.KillCount);
        }

        private void SetSubViewParameters(int coinCount,int killCount)
        {
            _killCountSubView.SetParameters(killCount);
            _coinGainSubView.SetParameters(coinCount);
        }

    }
}
