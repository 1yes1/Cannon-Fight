using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class MainMenuView : UIView
    {
        [SerializeField] private Button _btnPlay;
        [SerializeField] private GameObject _cannonRenderTexture;
        [SerializeField] private Button _upgradeButton;

        public override void Initialize()
        {
            _btnPlay.onClick.AddListener(Launcher.Instance.JoinRoom);
            _upgradeButton.onClick.AddListener(() => { UIManager.Show<UpgradeMenuView>(this); });
        }


        public override void Show()
        {
            base.Show();
            _cannonRenderTexture.SetActive(true);
        }

        public override void Hide()
        {
            base.Hide();
            _cannonRenderTexture.SetActive(false);
        }

    }
}
