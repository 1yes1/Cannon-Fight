using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

namespace CannonFightBase
{
    public class MainMenuView : UIView
    {
        [SerializeField] private ProfileImageSubView _profileImageSubView;
        [SerializeField] private Button _btnPlay;
        [SerializeField] private GameObject _cannonRenderTexture;
        [SerializeField] private Button _upgradeButton;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public override void Initialize()
        {
            _btnPlay.onClick.AddListener(() =>
            {
                Launcher.Instance.StartFight();
                _signalBus.Fire<OnButtonClickSignal>();
            });
            _upgradeButton.onClick.AddListener(() =>
            {
                UIManager.Show<UpgradeMenuView>(this);
                _signalBus.Fire<OnButtonClickSignal>();
            });
        }

        public override void AddSubViews()
        {
            UIManager.AddSubView(this, _profileImageSubView);
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
