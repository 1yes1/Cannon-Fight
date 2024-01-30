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
using UnityEngine.SceneManagement;

namespace CannonFightBase
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        [SerializeField] private int _randomSeed = 999;

        private IEventSubscriber[] _eventSubscribers;

        private Cannon _currentCannon;

        private CannonManager _currentCannonManager;

        private int _leftCannonsCount = 0;

        private bool _playWithBots = false;

        private bool _isTutorialScene = false;

        private int _startPlayersCount;

        private bool _isGameFinished;

        public static GameManager Instance => _instance;

        public static Cannon CurrentCannon => _instance._currentCannon;

        public static CannonManager CurrentCannonManager => _instance._currentCannonManager;

        public static bool IsGameFinished => _instance._isGameFinished;

        public GameEventCaller GameEventCaller { get; private set; }

        public GameEventReceiver GameEventReceiver { get; private set; }


        public int LeftCannonsCount
        {
            get { return _leftCannonsCount;}
            set { _leftCannonsCount = value; GameEventCaller.OnLeftCannonsCountChanged(_leftCannonsCount); }
        }

        public static bool PlayWithBots => _instance._playWithBots;

        public static bool IsTutorialScene => _instance._isTutorialScene;

        [Inject]
        public void Construct(IEventSubscriber[] eventSubscribers)
        {
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
            _playWithBots = (CloudSaveManager.GetValue<int>("playWithBots") == 1) ? true : false;
            //UnityEngine.Random.InitState(_randomSeed);
            AudioManager.PlaySound(GameSound.GameMusic);

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
            }
            //print("LeftCannonsCount: " + PhotonNetwork.CurrentRoom.Players.Values.Count);

            if(!PhotonNetwork.OfflineMode)
                LeftCannonsCount += PhotonNetwork.CurrentRoom.Players.Values.Count;
            else//Eğer photon bağlanmadıysa ve first fight ise kendimizi de ekleyelim
            {
                LeftCannonsCount++;
            }

            _startPlayersCount = LeftCannonsCount;
        }


        private void SetCannon()
        {
            if (_currentCannon != null)
                return;

            GameObject[] cannons = GameObject.FindGameObjectsWithTag("Cannon");
            for (int i = 0; i < cannons.Length; i++)
                if (cannons[i].GetComponent<PhotonView>().IsMine || !PhotonNetwork.IsConnectedAndReady)
                    _currentCannon = cannons[i].GetComponent<Cannon>();

            GameObject[] cannonManagers = GameObject.FindGameObjectsWithTag("CannonManager");
            for (int i = 0; i < cannonManagers.Length; i++)
                if (cannonManagers[i].GetComponent<PhotonView>().IsMine || !PhotonNetwork.IsConnectedAndReady)
                    _currentCannonManager = cannonManagers[i].GetComponent<CannonManager>();

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

                if (LeftCannonsCount == 1 && _startPlayersCount == 2)
                {
                    RoomManager.Instance.InGameDisconnect("Other Player's Internet Connection Lost!");
                }
                else if(LeftCannonsCount == 1)
                {
                    UIManager.Show<WarningView>().CreateWarning("Other Player's Internet Connection Lost!");
                    Winner();
                }
            }
        }

        private void OnOurPlayerDied(Player player)
        {
            DecreaseLeftCannonsCount();
            Loser();

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

            if(LeftCannonsCount == 1 && !CurrentCannon.IsDead)
            {
                Winner();
            }
        }

        private void OnAgentDied(Agent agent)
        {
            if (IsTutorialScene)
                return;

            DecreaseLeftCannonsCount();

            if (LeftCannonsCount == 1 && !Cannon.Current.IsDead)
            {
                Winner();
            }
        }

        private void Winner()
        {
            CurrentCannon.CannonManager.SetAsWinner();
            UIManager.ShowWithDelay<VictoryPanelView>(1.25f);
            AudioManager.PlaySound(GameSound.Victory,default,1.25f);
            AudioManager.PlaySound(MenuSound.MenuMusic);
            GameEventCaller.OnWinTheGame();
            GameEventCaller.OnGameFinished();
            _isGameFinished = true;

        }

        private void Loser()
        {
            CurrentCannon.CannonManager.SetAsLoser();
            UIManager.ShowWithDelay<DefeatedPanelView>(1.25f);
            AudioManager.PlaySound(GameSound.Defeated,default,1.25f);
            AudioManager.PlaySound(MenuSound.MenuMusic);
            GameEventCaller.OnLoseTheGame();
            GameEventCaller.OnGameFinished();
            _isGameFinished = true;

        }

        public static void SetTutorialScene()
        {
            _instance._isTutorialScene = true;
            _instance.LeftCannonsCount = 1;
        }

    }
}

