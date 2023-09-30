using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

namespace CannonFightBase
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public bool useAndroidControllers = false;

        private DefaultSkillProperties _skillProperties;

        private DefaultChestProperties _defaultChestProperties;

        private DefaultRampProperties _defaultRampProperties;

        private Cannon _currentCannon;

        private int _leftCannonsCount = 0;

        public static GameManager Instance => _instance;

        public static Cannon CurrentCannon => _instance._currentCannon;

        public GameEventCaller GameEventCaller { get; private set; }

        public GameEventReceiver GameEventReceiver { get; private set; }

        public static DefaultSkillProperties DefaultSkillProperties => Instance._skillProperties;

        public static DefaultChestProperties DefaultChestProperties => Instance._defaultChestProperties;

        public static DefaultRampProperties DefaultRampProperties => Instance._defaultRampProperties;


        [Inject]
        public void Construct(DefaultChestProperties defaultChestProperties,DefaultSkillProperties defaultSkillProperties,DefaultRampProperties defaultRampProperties)
        {
            _skillProperties = defaultSkillProperties;
            _defaultChestProperties = defaultChestProperties;
            _defaultRampProperties = defaultRampProperties;
        }

        public int LeftCannonsCount
        {
            get { return _leftCannonsCount;}
            set { _leftCannonsCount = value; GameEventCaller.OnLeftCannonsCountChanged(_leftCannonsCount); }
        }

        private void OnEnable()
        {
            GameEventReceiver.BeforeOurPlayerSpawnedEvent += SetCannon;//Bu eventte GameManager ile Cannon a ulaşılmıyor çünkü atama yapılmadı
            GameEventReceiver.OnOurPlayerSpawnedEvent += OnOurPlayerSpawned;
            GameEventReceiver.OnPlayerEnteredRoomEvent += OnPlayerEnteredRoom;
            GameEventReceiver.OnPlayerLeftRoomEvent += OnPlayerLeftRoom;
            GameEventReceiver.OnPlayerDieEvent += OnPlayerDie;
            //GameEventReceiver.OnOurPlayerSpawnedEvent += OnOurPlayerSpawned;//Bu eventte GameManager ile Cannon a ulaşılıyor
        }

        private void OnDisable()
        {
            GameEventReceiver.BeforeOurPlayerSpawnedEvent -= SetCannon;
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

