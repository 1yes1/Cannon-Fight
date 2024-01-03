using CannonFightUI;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class SaveManager : MonoBehaviour
    {
        private CannonLevelSettings.DamageLevelSettings _damageLevelSettings;

        private CannonLevelSettings.FireRateLevelSettings _fireRateLevelSettings;

        private CannonLevelSettings.HealthLevelSettings _healthLevelSettings;

        private CannonLevelSettings.SpeedLevelSettings _speedLevelSettings;

        private CoinManager.SaveSettings coinManagerSave;

        private JsonSaver _jsonSaver;

        private SignalBus _signalBus;

        private bool _canLoadData;

        private bool _isAuthanticated;

        [Inject]
        public void Construct(CoinManager.SaveSettings saveSettings,JsonSaver jsonSaver,
            CannonLevelSettings.DamageLevelSettings damageLevelSettings,
            CannonLevelSettings.FireRateLevelSettings fireRateLevelSettings,
            CannonLevelSettings.HealthLevelSettings healthLevelSettings,
            CannonLevelSettings.SpeedLevelSettings speedLevelSettings,
            SignalBus signalBus)
        {
            coinManagerSave = saveSettings;
            _jsonSaver = jsonSaver;

            _damageLevelSettings = damageLevelSettings;
            _fireRateLevelSettings = fireRateLevelSettings;
            _healthLevelSettings = healthLevelSettings;
            _speedLevelSettings = speedLevelSettings;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<OnLevelUpgradedSignal>(OnLevelUpgraded);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<OnLevelUpgradedSignal>(OnLevelUpgraded);
        }

        private void Start()
        {
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(OnAuthanticated);
        }

        public static T GetValue<T>(string key, object defaultValue = null)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                    return (T)(object)PlayerPrefs.GetInt(key, (defaultValue == null) ? 0 : (int)defaultValue);
                    break;
                case TypeCode.Decimal:
                    return (T)(object)PlayerPrefs.GetFloat(key, (defaultValue == null) ? 0 : (int)defaultValue);
                    break;
                case TypeCode.String:
                    return (T)(object)PlayerPrefs.GetString(key, (defaultValue == null) ? "" : (string)defaultValue);
                    break;
                default:
                    return default(T);
                    break;
            }
        }


        public static void SetValue<T>(string key, object value)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                    PlayerPrefs.SetInt(key, (int)value);
                    break;
                case TypeCode.Decimal:
                    PlayerPrefs.SetFloat(key, (float)value);
                    break;
                case TypeCode.String:
                    PlayerPrefs.SetString(key, (string)value);
                    break;
                default:
                    break;
            }
        }


        public static bool HasValue(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        private void OnAuthanticated(bool obj)
        {
            _isAuthanticated = obj;
            Debug.Log("Authanticated: " + _isAuthanticated);

            if (_isAuthanticated)
                _jsonSaver.LoadData(OnPlayerCloudSavesLoaded);
            else
                print("Cant Load Datas. No Authantication");
        }

        private void OnPlayerCloudSavesLoaded(PlayerSaveData data)
        {
            if (!HasValue(UpgradeType.Speed.ToString()) && !HasValue(UpgradeType.Damage.ToString()))
            {
                Debug.Log("No Saves");
                _damageLevelSettings.SetDefaultValue();
                _fireRateLevelSettings.SetDefaultValue();
                _healthLevelSettings.SetDefaultValue();
                _speedLevelSettings.SetDefaultValue();
                CoinManager.Instance.SetDefaultValue();
            }
            else
                Debug.Log("Has Saves");

            if (data == null)
                SaveDatasToCloud();
            else
            {
                if(IsCloudSavesPreffered(data))
                {
                    Debug.Log("Cloud kayýtlarý daha yeni. O zaman mobile kaydedelim.");
                    SaveDatasToMobile(data);
                }
                else
                {
                    Debug.Log("Mobil kayýtlar daha yeni. O zaman cloud a kaydedelim");
                    SaveDatasToCloud();
                }
            }
            _signalBus.Fire(new OnCloudSavesLoadedSignal() { PlayerSaveData = data });
        }

        private void SaveDatasToMobile(PlayerSaveData playerSaveData)
        {
            _damageLevelSettings.SetLevel(playerSaveData.CannonSaveData.DamageLevel);
            _fireRateLevelSettings.SetLevel(playerSaveData.CannonSaveData.FireRateLevel);
            _healthLevelSettings.SetLevel(playerSaveData.CannonSaveData.HealthLevel);
            _speedLevelSettings.SetLevel(playerSaveData.CannonSaveData.SpeedLevel);
            CoinManager.CurrentCoin = playerSaveData.CurrentCoin;
        }

        private void SaveDatasToCloud()
        {
            print("Datas Saving To Cloud: ");
            
            PlayerSaveData data = new PlayerSaveData()
            {
                CurrentCoin = CoinManager.CurrentCoin,
                CannonSaveData = new CannonSaveData
                {
                    DamageLevel = _damageLevelSettings.Level,
                    FireRateLevel = _fireRateLevelSettings.Level,
                    HealthLevel = _healthLevelSettings.Level,
                    SpeedLevel = _speedLevelSettings.Level,
                }
            };

            _jsonSaver.SaveData(data);
        }


        private bool IsCloudSavesPreffered(PlayerSaveData data)
        {
            if (_damageLevelSettings.Level > data.CannonSaveData.DamageLevel ||
                    _fireRateLevelSettings.Level > data.CannonSaveData.FireRateLevel ||
                    _healthLevelSettings.Level > data.CannonSaveData.HealthLevel ||
                    _speedLevelSettings.Level > data.CannonSaveData.SpeedLevel)
                return false;
            else
                return true;
        }

        private void OnLevelUpgraded(OnLevelUpgradedSignal obj)
        {
            SaveDatasToCloud();
        }

    }
}
