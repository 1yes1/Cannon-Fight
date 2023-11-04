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
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private GameObject _cannonRenderTexture;

        public override void Initialize()
        {
            _btnPlay.onClick.AddListener(Launcher.Instance.JoinRoom);
        }

        private void Start()
        {
            _coinText.text = CoinManager.CurrentCoin.ToString();
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
