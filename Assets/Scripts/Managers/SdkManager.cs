//using Facebook.Unity;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SdkManager : MonoBehaviour
//{
//    public float time = 0;


//    private void Start()
//    {
//        SendEventOnStart();
//        time = 0;
//        FB.Init(InitCallback, OnHideUnity);

//    }

//    private void Update()
//    {
//        if (GameManager.instance.gameIsRunning)
//        {
//            time += Time.deltaTime;
//        }

//    }


//    #region FacebookSdk

//    private void InitCallback()
//    {
//        if (FB.IsInitialized)
//        {
//            FB.ActivateApp();
//            print("SDK Activated");
//            FB.Mobile.SetAutoLogAppEventsEnabled(true);
//            //GameManager.instance.LogStart_AppEvent();
//        }
//    }

//    private void OnHideUnity(bool isGameShown)
//    {
//        Debug.Log("Is game showing? " + isGameShown);
//    }

//    public void LevelEnded()
//    {
//        var tutParams = new Dictionary<string, object>();
//        tutParams[AppEventParameterName.ContentID] = "Level " + PlayerPrefs.GetInt("LevelNowIndex");
//        tutParams[AppEventParameterName.Description] = "Level Completed";

//        FB.LogAppEvent(
//            AppEventName.CompletedTutorial,
//            parameters: tutParams
//        );

//    }

//    //public void LogStart_AppEvent()
//    //{
//    //    FB.LogAppEvent(
//    //        "Start_App"
//    //    );
//    //}

//    //public void LogFinish_LevelEvent(string loseOrWin, double valToSum)
//    //{
//    //    var parameters = new Dictionary<string, object>();
//    //    parameters["loseOrWin"] = loseOrWin;
//    //    FB.LogAppEvent(
//    //        "Finish_Level",
//    //        (float)valToSum,
//    //        parameters
//    //    );
//    //}

//    #endregion



//    #region AppMetrica Sdk

//    void SendEventOnStart()
//    {
//        var metrica = AppMetrica.Instance;

//        Dictionary<string, object> startList = new Dictionary<string, object>();

//        startList.Add("Level", (PlayerPrefs.GetInt("LevelNowIndex") + 1));

//        metrica.ReportEvent("level_start", startList);
//        metrica.SendEventsBuffer();
//    }

//    //Oyun bittiğinde Finish_Type (lose veya win) e göre çağrılır
//    public void SendEventOnFinish(string loseOrWin)
//    {
//        var metrica = AppMetrica.Instance;

//        Dictionary<string, object> finishList = new Dictionary<string, object>();

//        finishList.Add("Level", (PlayerPrefs.GetInt("LevelNowIndex") + 1));
//        finishList.Add("Finish_Type", loseOrWin);
//        finishList.Add("Spent_Time", (int)time);
//        finishList.Add("Level_Progress", GetLevelProgress(loseOrWin));

//        //print("Level: " + (PlayerPrefs.GetInt("LevelNowIndex") + 1));
//        //print("FinishType: " + loseOrWin);
//        //print("Spent_Time: " + (int)time);
//        //print("Level_Progress: " + GetLevelProgress(loseOrWin));

//        metrica.ReportEvent("level_finish", finishList);
//        metrica.SendEventsBuffer();
//    }


//    #endregion



//    public float GetLevelProgress(string loseOrWin)
//    {
//        float progress = 0;
//        if (loseOrWin == "win") progress = 100;
//        else
//            progress = UIManager.instance.GetLevelProgress();

//        return progress;
//    }
//}
