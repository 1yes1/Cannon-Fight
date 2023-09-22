using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance;

    [SerializeField] bool _usePlayerPrefs = false;
    [SerializeField] int _setMoney = 0;
    [SerializeField] int _cowPlaceCount = 0;
    [SerializeField] int _milkinManCount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (!PlayerPrefs.HasKey("TotalMoney"))
            SetDefaultPrefs();

#if UNITY_EDITOR
        if (_setMoney > -1) TotalMoney = _setMoney;
        if (_cowPlaceCount > -1) CowPlaceCount = _cowPlaceCount;
        if (_milkinManCount > -1) MilkingManCount = _milkinManCount;

#endif


    }
    private void Start()
    {

    }

    private void SetDefaultPrefs()
    {
        //Hiçbir prefs yok en baþtan atama yapýlýyor
        PlayerPrefs.SetInt("TotalMoney", 0);
        PlayerPrefs.SetInt("CowPlaceCount", 1);
        PlayerPrefs.SetInt("MilkingManCount", 0);
    }


    [Button("Reset Inspector Values", Spacing = ButtonSpacing.Before | ButtonSpacing.After)]
    private void ResetInspectorValues()
    {
        _setMoney = _cowPlaceCount = _milkinManCount = -1;
    }


    public static int TotalMoney
    {
        get
        {
            return PlayerPrefs.GetInt("TotalMoney", 0);
        }
        set
        {
            PlayerPrefs.SetInt("TotalMoney", value);
            //UIManager.instance.SetMoney();
        }
    }


    public static int CowPlaceCount
    {
        get
        {
            return PlayerPrefs.GetInt("CowPlaceCount", 0);
        }
        set
        {
            PlayerPrefs.SetInt("CowPlaceCount", value);
        }
    }

    public static int MilkingManCount
    {
        get
        {
            return PlayerPrefs.GetInt("MilkingManCount", 0);
        }
        set
        {
            PlayerPrefs.SetInt("MilkingManCount", value);
        }
    }

}
