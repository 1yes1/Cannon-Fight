#if UNITY_ANDROID 
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CannonFightBase
{
    public class CloudSaveManager : IInitializable,IDisposable
    {
        private static CloudSaveManager _instance;

        private CannonLevelSettings.DamageLevelSettings _damageLevelSettings;

        private CannonLevelSettings.FireRateLevelSettings _fireRateLevelSettings;

        private CannonLevelSettings.HealthLevelSettings _healthLevelSettings;

        private CannonLevelSettings.SpeedLevelSettings _speedLevelSettings;

        private JsonSaver _jsonSaver;

        private SignalBus _signalBus;

        private bool _isAuthanticated;

        public static CloudSaveManager Instance => _instance;

        public static bool IsFirstOpening => !HasValue(UserManager.NICKNAME_PREFS);

        [Inject]
        public void Construct(CoinManager.SaveSettings saveSettings,JsonSaver jsonSaver,
            CannonLevelSettings.DamageLevelSettings damageLevelSettings,
            CannonLevelSettings.FireRateLevelSettings fireRateLevelSettings,
            CannonLevelSettings.HealthLevelSettings healthLevelSettings,
            CannonLevelSettings.SpeedLevelSettings speedLevelSettings,
            SignalBus signalBus)
        {
            _jsonSaver = jsonSaver;

            _damageLevelSettings = damageLevelSettings;
            _fireRateLevelSettings = fireRateLevelSettings;
            _healthLevelSettings = healthLevelSettings;
            _speedLevelSettings = speedLevelSettings;
            _signalBus = signalBus;
        }


        public void Initialize()
        {
            if (_instance == null)
                _instance = this;

            _signalBus.Subscribe<OnLevelUpgradedSignal>(OnLevelUpgraded);
            SceneManager.sceneLoaded += OnSceneLoaded;
            _signalBus.Subscribe<OnNicknameEnteredSignal>(OnNicknameEntered);

            Debug.Log("IsFirstOpeningInThisPhone: " + IsFirstOpening);
            StartCloudConnection();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<OnLevelUpgradedSignal>(OnLevelUpgraded);
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _signalBus.Unsubscribe<OnNicknameEnteredSignal>(OnNicknameEntered);
        }

        private void OnNicknameEntered()
        {
            SetDefaultSettings();
            StartCloudConnection();
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            //Ana menüye her girdiðinde
            if (arg0.buildIndex == 0)
            {
                if (!IsFirstOpening)//Eðer ilk açýlýþsa beklesin þimdi deðil
                    StartCloudConnection();
            }
        }

        public void StartCloudConnection()
        {
            //Play Games için domain ekleninceye kadar bu þekilde kalacak.
//#if !UNITY_EDITOR
                //PlayGamesPlatform.Activate();
                //PlayGamesPlatform.Instance.Authenticate(OnAuthanticated);
//#else
            OnAuthanticated(false);
            //Social.localUser.Authenticate(OnAuthanticated);
//#endif
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
            {
                _jsonSaver.LoadData(OnPlayerCloudSavesLoaded);
                _signalBus.Fire(new OnPlayGamesAuthanticated() { IsAuthanticated = true});
            }
            else
            {
                //Belkide bir cloud kaydý var fakat error alýndý.
                //O yüzden sýfýrdan baþlar gibi yapýyoruz
                if (IsFirstOpening)
                {
                    _signalBus.Fire<OnFirstOpeningSignal>();
                    Debug.Log("Belkide bir cloud kaydý var fakat baðlanýlamadý");
                    return;
                }

                _signalBus.Fire(new OnPlayGamesAuthanticated() { IsAuthanticated = false });
                _signalBus.Fire(new OnFailedToLoadCloudSavesSignal());
            }
        }

        private void OnAuthanticated(SignInStatus signInStatus)
        {
            if (signInStatus == SignInStatus.Success)
                OnAuthanticated(true);
            else
                OnAuthanticated(false);
        }

        private void OnPlayerCloudSavesLoaded(PlayerSaveData data)
        {

            if (data == null)
            {
                Debug.Log("IsFirstOpening: " + IsFirstOpening);

                //Hem telefonda kayýt yok hem de cloud da yok o zaman first opening
                if(IsFirstOpening)
                    _signalBus.Fire<OnFirstOpeningSignal>();

                //Bir hata çýkmýþ ve yüklenememiþ veriler.
                _signalBus.Fire(new OnFailedToLoadCloudSavesSignal());
                Debug.Log("OnFailedToLoadCloudSavesSignal");
                return;
            }
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

            Debug.Log("CLOUD SAVE LOADED");
            _signalBus.Fire(new OnCloudSavesLoadedSignal() { PlayerSaveData = data });
        }

        private void SetDefaultSettings()
        {
            Debug.Log("No Saves");
            _damageLevelSettings.SetDefaultValue();
            _fireRateLevelSettings.SetDefaultValue();
            _healthLevelSettings.SetDefaultValue();
            _speedLevelSettings.SetDefaultValue();
            CoinManager.Instance.SetDefaultValue();
        }

        private void SaveDatasToMobile(PlayerSaveData playerSaveData)
        {
            Debug.Log("Datas Saving To Mobile");

            _damageLevelSettings.SetLevel(playerSaveData.CannonSaveData.DamageLevel);
            _fireRateLevelSettings.SetLevel(playerSaveData.CannonSaveData.FireRateLevel);
            _healthLevelSettings.SetLevel(playerSaveData.CannonSaveData.HealthLevel);
            _speedLevelSettings.SetLevel(playerSaveData.CannonSaveData.SpeedLevel);
            CoinManager.CurrentCoin = playerSaveData.CurrentCoin;

            string nickname = FixNickname(playerSaveData.Nickname);

            UserManager.Instance.SetNickname(nickname);
        }

        public void SaveDatasToCloud()
        {
            string nickname = FixNickname(GetValue<string>(UserManager.NICKNAME_PREFS));

            PlayerSaveData data = new PlayerSaveData()
            {
                Nickname = nickname,
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
            if(HasValue(UserManager.NICKNAME_PREFS))
            {
                if ((_damageLevelSettings.Level < data.CannonSaveData.DamageLevel ||
                        _fireRateLevelSettings.Level < data.CannonSaveData.FireRateLevel ||
                        _healthLevelSettings.Level < data.CannonSaveData.HealthLevel ||
                        _speedLevelSettings.Level < data.CannonSaveData.SpeedLevel))
                    return true;
                else
                    return false;

            }
            else
                return true;
        }

        private void OnLevelUpgraded(OnLevelUpgradedSignal obj)
        {
            SaveDatasToCloud();
        }

        private string FixNickname(string nick)
        {
            string nickname = nick;
            if (nickname[nickname.Length - 1] == '?')
                nickname = nickname.Remove(nickname.Length - 1, 1);

            return nickname;
        }

    }
}
