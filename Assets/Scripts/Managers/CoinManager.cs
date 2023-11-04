using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class CoinManager : MonoBehaviour
    {
        private static CoinManager _instance;

        private SaveSettings _saveSettings;

        public static CoinManager Instance => _instance;

        public static int CurrentCoin
        {
            get 
            { 
                if (!PlayerPrefs.HasKey(Instance._saveSettings.CurrentCoin))
                    PlayerPrefs.SetInt(Instance._saveSettings.CurrentCoin, 0);

                return PlayerPrefs.GetInt(Instance._saveSettings.CurrentCoin);
            }
            set
            {
                PlayerPrefs.SetInt(Instance._saveSettings.CurrentCoin, value);
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

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            CurrentCoin += 58;
        }

        [Serializable]
        public class SaveSettings
        {
            public string CurrentCoin;
        }
    }
}
