using CannonFightUI;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class GamePanelView : UIView
    {
        [SerializeField] private TextMeshProUGUI _healthText;

        [SerializeField] private Slider _ballFrequencySlider;

        [SerializeField] private Image _fillImage;

        [SerializeField] private TextMeshProUGUI _cannonsLeftText;

        public override void Initialize()
        {
        }


        private void OnEnable()
        {
            GameEventReceiver.OnOurPlayerSpawnedEvent += OnOurPlayerSpawned;
            GameEventReceiver.OnOurPlayerHealthChangedEvent += UpdateHealthText;
            GameEventReceiver.OnPlayerCountInRoomChangedEvent += UpdateCannonsLeftText;
        }


        private void OnDisable()
        {
            GameEventReceiver.OnOurPlayerSpawnedEvent -= OnOurPlayerSpawned;
            GameEventReceiver.OnOurPlayerHealthChangedEvent -= UpdateHealthText;
            GameEventReceiver.OnPlayerCountInRoomChangedEvent -= UpdateCannonsLeftText;
        }

        private void OnOurPlayerSpawned()
        {
            UpdateHealthText();
            UpdateCannonsLeftText();
        }

        private void UpdateCannonsLeftText()
        {
            _cannonsLeftText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }

        private void UpdateHealthText()
        {
            _healthText.text = Cannon.Current.Health.ToString();
        }

        public void UpdateFrequencySlider(float val)
        {
            _ballFrequencySlider.value = val;
        }

        public void OnEnemyKilled()
        {
            //_fillImage.fillAmount = GameManager.CurrentCannon.KillCount * GameManager.Instance.MaxKillCountForPowers / 100;
        }

    }
}
