using CannonFightUI;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class ScoreboardView : UIView
    {
        [SerializeField] private ScoreboardScrollMenu scrollMenu;

        [SerializeField] private CanvasGroup _canvasGroup;

        private bool _showScoreboard = false;

        private bool _isScoreboardOpen = false;


        public override void Initialize()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            HideScoreboard();
            scrollMenu.Initialize();
            scrollMenu.RefreshItemList();
            //_canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void AddSubViews()
        {
        }

        public void ShowScoreboard()
        {
            scrollMenu.UpdateItemList();

            _canvasGroup.alpha = 1;
            _isScoreboardOpen = true;
        }
        private void HideScoreboard()
        {
            _canvasGroup.alpha = 0;
            _isScoreboardOpen = false;
        }

        private void Update()
        {
            if (InputManager.ShowScoreboard && !_isScoreboardOpen)
                ShowScoreboard();
            else if (!InputManager.ShowScoreboard && _isScoreboardOpen)
                HideScoreboard();
        }

    }
}
