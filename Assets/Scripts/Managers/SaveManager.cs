using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class SaveManager : MonoBehaviour
    {

        private CoinManager.SaveSettings coinManagerSave;

        [Inject]
        public void Construct(CoinManager.SaveSettings saveSettings)
        {
            coinManagerSave = saveSettings;
        }


        public static T GetValue<T>(string key,object defaultValue = null)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
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


        public static void SetValue<T>(string key,object value)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
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
    }
}
