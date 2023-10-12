using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public bool useAndroidControllers = false;

        private Cannon _currentCannon;

        [Inject]
        private Launcher.Settings _launcherSettings;

        private int _leftCannonsCount = 0;

        public static GameManager Instance => _instance;

        public static Cannon CurrentCannon => _instance._currentCannon;

        public GameEventCaller GameEventCaller { get; private set; }

        public GameEventReceiver GameEventReceiver { get; private set; }


        public int LeftCannonsCount
        {
            get { return _leftCannonsCount;}
            set { _leftCannonsCount = value; GameEventCaller.OnLeftCannonsCountChanged(_leftCannonsCount); }
        }

        public void Construct(Launcher.Settings settings)
        {
            _launcherSettings = settings;
            print("Girrrr");
        }

        private void OnEnable()
        {
            GameEventReceiver.OnBeforeOurPlayerSpawnedEvent += OnBeforeOurPlayerSpawned;//Bu eventte GameManager ile Cannon a ulaşılmıyor çünkü atama yapılmadı
            GameEventReceiver.OnOurPlayerSpawnedEvent += OnOurPlayerSpawned;
            GameEventReceiver.OnPlayerEnteredRoomEvent += OnPlayerEnteredRoom;
            GameEventReceiver.OnPlayerLeftRoomEvent += OnPlayerLeftRoom;
            GameEventReceiver.OnPlayerDieEvent += OnPlayerDie;
            //GameEventReceiver.OnOurPlayerSpawnedEvent += OnOurPlayerSpawned;//Bu eventte GameManager ile Cannon a ulaşılıyor
        }

        private void OnDisable()
        {
            GameEventReceiver.OnBeforeOurPlayerSpawnedEvent -= OnBeforeOurPlayerSpawned;
            GameEventReceiver.OnOurPlayerSpawnedEvent -= OnOurPlayerSpawned;
            GameEventReceiver.OnPlayerEnteredRoomEvent -= OnPlayerEnteredRoom;
            GameEventReceiver.OnPlayerLeftRoomEvent -= OnPlayerLeftRoom;
            GameEventReceiver.OnPlayerDieEvent -= OnPlayerDie;
            //GameEventReceiver.OnOurPlayerSpawnedEvent -= OnOurPlayerSpawned;
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            Initialize();
        }

        private void Initialize()
        {
            this.GameEventReceiver = new GameEventReceiver();
            this.GameEventCaller = new GameEventCaller(GameEventReceiver);

            foreach (var item in FindObjectsOfType<CFBehaviour>())
            {
                item.RegisterCallerEvents();
            }

        }

        private void OnBeforeOurPlayerSpawned()
        {
            //if(PhotonNetwork.CurrentRoom.Players.Count >= _launcherSettings.MinPlayersCountToLoadSettings)

            print("_launcherSettings.MinPlayersCountToStart: " + _launcherSettings.MinPlayersCountToStart);
            print("Anlık Oyuncu: "+PhotonNetwork.CurrentRoom.Players.Count);

            SetCannon();
        }

        private void SetCannon()
        {
            if (_currentCannon != null)
                return;

            GameObject[] cannons = GameObject.FindGameObjectsWithTag("Cannon");

            for (int i = 0; i < cannons.Length; i++)
                if (cannons[i].GetComponent<PhotonView>().IsMine)
                    _currentCannon = cannons[i].GetComponent<Cannon>();

            GameEventCaller.OnOurPlayerSpawned();
        }

        private void IncreaseLeftCannonsCount()
        {
            LeftCannonsCount++;
        }
        public void DecreaseLeftCannonsCount()
        {
            LeftCannonsCount--;
        }

        public void OnPlayerEnteredRoom(Player player)
        {
            IncreaseLeftCannonsCount();
        }

        private void OnPlayerLeftRoom(Player obj)
        {
            if (!obj.CustomProperties.ContainsKey("isDead"))
                DecreaseLeftCannonsCount();
        }

        private void OnPlayerDie(Player player)
        {
            DecreaseLeftCannonsCount();
        }


        private void OnOurPlayerSpawned()
        {
            CheckLeftCannonsCount();
        }


        private void CheckLeftCannonsCount()
        {
            foreach (Player item in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (!item.CustomProperties.ContainsKey("isDead"))
                    LeftCannonsCount++;
            }
        }

    }
}

