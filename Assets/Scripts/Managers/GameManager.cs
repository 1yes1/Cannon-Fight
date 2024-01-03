using CannonFightUI;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;

namespace CannonFightBase
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public bool useAndroidControllers = false;

        [SerializeField] private int _randomSeed = 999;

        private IEventSubscriber[] _eventSubscribers;

        private Cannon _currentCannon;

        private RoomManager.Settings _launcherSettings;

        private int _leftCannonsCount = 0;

        private bool _playWithBots = false;

        public static GameManager Instance => _instance;

        public static Cannon CurrentCannon => _instance._currentCannon;

        public GameEventCaller GameEventCaller { get; private set; }

        public GameEventReceiver GameEventReceiver { get; private set; }


        public int LeftCannonsCount
        {
            get { return _leftCannonsCount;}
            set { _leftCannonsCount = value; GameEventCaller.OnLeftCannonsCountChanged(_leftCannonsCount); }
        }

        public static bool PlayWithBots => _instance._playWithBots;

        [Inject]
        public void Construct(RoomManager.Settings settings, IEventSubscriber[] eventSubscribers)
        {
            _launcherSettings = settings;
            _eventSubscribers = eventSubscribers;
        }

        private void OnEnable()
        {
            GameEventReceiver.OnBeforeOurPlayerSpawnedEvent += OnBeforeOurPlayerSpawned;//Bu eventte GameManager ile Cannon a ulaşılmıyor çünkü atama yapılmadı
            RoomManager.OnPlayerLeftRoomEvent += OnPlayerLeftRoom;
            GameEventReceiver.OnOurPlayerDiedEvent += OnOurPlayerDied;
            GameEventReceiver.OnPlayerDiedEvent += OnPlayerDied;
            GameEventReceiver.OnAgentDiedEvent += OnAgentDied; ;
            GameEventReceiver.OnGameStartedEvent += OnGameStarted;
            //GameEventReceiver.OnOurPlayerSpawnedEvent += OnOurPlayerSpawned;//Bu eventte GameManager ile Cannon a ulaşılıyor
        }

        private void OnDisable()
        {
            GameEventReceiver.OnBeforeOurPlayerSpawnedEvent -= OnBeforeOurPlayerSpawned;
            RoomManager.OnPlayerLeftRoomEvent -= OnPlayerLeftRoom;
            GameEventReceiver.OnOurPlayerDiedEvent -= OnOurPlayerDied;
            GameEventReceiver.OnPlayerDiedEvent -= OnPlayerDied;
            GameEventReceiver.OnAgentDiedEvent -= OnAgentDied; ;
            GameEventReceiver.OnGameStartedEvent -= OnGameStarted;
            //GameEventReceiver.OnOurPlayerSpawnedEvent -= OnOurPlayerSpawned;
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            Initialize();
            LeftCannonsCount = 0;
            Application.targetFrameRate = 60;
            _playWithBots = (SaveManager.GetValue<int>("playWithBots") == 1) ? true : false;
            //UnityEngine.Random.InitState(_randomSeed);

        }


        private void Initialize()
        {
            this.GameEventReceiver = new GameEventReceiver();
            this.GameEventCaller = new GameEventCaller(GameEventReceiver);

            for (int i = 0; i < _eventSubscribers.Length; i++)
            {
                _eventSubscribers[i].SubscribeEvent();
            }
        }

        private void OnBeforeOurPlayerSpawned()
        {
            //if(PhotonNetwork.CurrentRoom.Players.Count >= _launcherSettings.MinPlayersCountToLoadSettings)

            //print("_launcherSettings.MinPlayersCountToStart: " + _launcherSettings.MinPlayersCountToStart);
            //print("Anlık Oyuncu: "+PhotonNetwork.CurrentRoom.Players.Count);

            SetCannon();
        }

        private void OnGameStarted()
        {
            LeftCannonsCount = 0;

            if (PlayWithBots)
            {
                LeftCannonsCount += AIManager.Instance.AgentCount;
                print("AIManager.Instance.AgentCount: " + AIManager.Instance.AgentCount);
            }
            //print("LeftCannonsCount: " + PhotonNetwork.CurrentRoom.Players.Values.Count);

            if(PhotonNetwork.IsConnected)
                LeftCannonsCount += PhotonNetwork.CurrentRoom.Players.Values.Count;
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

        public void DecreaseLeftCannonsCount()
        {
            LeftCannonsCount--;
        }

        private void OnPlayerLeftRoom(Player player)
        {
            if (!player.CustomProperties.Keys.Contains("isDead"))
            {
                DecreaseLeftCannonsCount();
            }
        }

        private void OnOurPlayerDied(Player player)
        {
            DecreaseLeftCannonsCount();
            UIManager.ShowWithDelay<DefeatedPanelView>(1.25f);
            //PhotonNetwork.LeaveRoom();
            print("YOU DIED");
        }

        private void OnPlayerDied(Player player)
        {
            DecreaseLeftCannonsCount();

            Hashtable hashtable = new Hashtable
            {
                { "isDead", true }
            };
            player.SetCustomProperties(hashtable);

            //Sadece biz kalmışız
            //print("-----------Kimler Kalmış-------" + LeftCannonsCount);

            if(LeftCannonsCount == 1)
            {
                UIManager.ShowWithDelay<VictoryPanelView>(1.25f);
            }
        }

        private void OnAgentDied(Agent agent)
        {
            DecreaseLeftCannonsCount();

            if (LeftCannonsCount == 1 && !Cannon.Current.IsDead)
            {
                UIManager.ShowWithDelay<VictoryPanelView>(1.25f);
            }
        }


    }
}

