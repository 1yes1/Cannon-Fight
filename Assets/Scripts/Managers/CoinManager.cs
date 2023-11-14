using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace CannonFightBase
{
    public class CoinManager : MonoBehaviour
    {
        public static CoinManager _instance;

        private SaveSettings _saveSettings;
        private CoinView _coinView;

        public static int CurrentCoin
        {
            get
            {
                return SaveManager.GetValue<int>(_instance._saveSettings.CurrentCoin);
            }
            set
            {
                SaveManager.SetValue<int>(_instance._saveSettings.CurrentCoin, value);
                _instance._coinView.UpdateCoin(CurrentCoin);
            }
        }

        [Inject]
        public void Construct(SaveSettings saveSettings)
        {
            _saveSettings = saveSettings;
        }

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
        }

        private void Start()
        {
            _coinView = UIManager.GetView<CoinView>();

            _coinView.UpdateCoin(CurrentCoin);

            CurrentCoin += 888;

        }


        [Serializable]
        public class SaveSettings
        {
            public string CurrentCoin;
        }

    }
}
