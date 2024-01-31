using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private static Launcher _instance;

        public static event Action OnJoinedRoomEvent;

        public static event Action OnLeftRoomEvent;

        public static event Action<Player> OnPlayerJoinedRoomEvent;

        public static event Action<Player> OnPlayerLeftRoomEvent;

        public static event Action<bool> OnPhotonConnectResultEvent;

        private RoomManager.RoomServerSettings _settings;

        private SignalBus _signalBus;

        private bool _canPlayWithBots = true;

        private bool _isFirstFight = false;

        private string _gameVersion = "1";


        public static Launcher Instance => _instance;


        void Awake()
        {
            if(_instance == null)
                _instance = this;

            if(!PhotonNetwork.LocalPlayer.IsMasterClient)
                PhotonNetwork.AutomaticallySyncScene = true;

        }

        [Inject]
        public void Construct(RoomManager.RoomServerSettings settings,SignalBus signalBus)
        {
            _settings = settings;
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            base.OnEnable();
            _signalBus.Subscribe<OnCloudSavesLoadedSignal>(OnCloudSavesResult);
            _signalBus.Subscribe<OnFailedToLoadCloudSavesSignal>(OnCloudSavesResult);

        }

        private void OnDisable()
        {
            base.OnDisable();

            _signalBus.Unsubscribe<OnCloudSavesLoadedSignal>(OnCloudSavesResult);
            _signalBus.Unsubscribe<OnFailedToLoadCloudSavesSignal>(OnCloudSavesResult);

        }


        private void OnCloudSavesResult()
        {
            print("Try Connect: " + PhotonNetwork.IsConnectedAndReady); ;

            if (PhotonNetwork.IsConnectedAndReady)
            {
                OnConnectedToMaster();
                return;
            }

            Connect();
            Invoke(nameof(NoConnection),_settings.PhotonConnectTimeout);
        }


        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }

        private void NoConnection()
        {
            if (PhotonNetwork.IsConnected)
                return;

            print("No Connection");
            PhotonNetwork.Disconnect();
            PhotonNetwork.OfflineMode = true;
            OnPhotonConnectResultEvent?.Invoke(false);
        }

        public void StartFight()
        {
            print("PhotonNetwork.OfflineMode: " + PhotonNetwork.OfflineMode);
            if(!PhotonNetwork.OfflineMode)
            {
                JoinRoom();
                print("Join Room");
            }
            else
            {
                print("Play With Bots");

                PlayWithBots();
            }
        }

        private void JoinRoom()
        {
            PhotonNetwork.LocalPlayer.NickName = UserManager.Instance.Nickname;

            bool isSuccess = PhotonNetwork.JoinRandomRoom();
            if (!isSuccess && PhotonNetwork.IsConnectedAndReady)
                OnJoinRoomFailed();
        }

        public void LeftRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void CreateRoom()
        {
            string roomName = "Room " + UnityEngine.Random.Range(1, 1000);
            RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 10, IsOpen = true, IsVisible = true };
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        public override void OnConnectedToMaster()
        {
            OnPhotonConnectResultEvent?.Invoke(true);
        }


        //Local Player joined
        public override void OnJoinedRoom()
        {
            _signalBus.Fire<OnGameMatchingStartedSignal>();

            CloudSaveManager.SetValue<int>("playWithBots", 0);//Ýlk baþta eðer playerprefs varsa diye sýfýrlayalým duruma göre 1 yapýalcak

            OnJoinedRoomEvent?.Invoke();

            UpdatePlayersCount();

            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.MaxPlayers = _settings.PlayersInGame;

            if (RoomManager.IsFirstFight)
            {
                PlayWithBotsFirstFight();
                return;
            }

            CheckPlayersToStart();

            CheckPlayWithBots();
        }

        private void PlayWithBots()
        {
            if ((PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnectedAndReady) && _canPlayWithBots)
            {
                CloudSaveManager.SetValue<int>("playWithBots",1);
                LoadSceneManager.PhotonLoadScene(GameScene.Game);
            }
        }

        public void PlayWithBotsFirstFight()
        {
            CloudSaveManager.SetValue<int>("playWithBots", 1);
            LoadSceneManager.PhotonLoadScene(GameScene.Game);
        }


        private void PlayWithPlayers()
        {
            if (PhotonNetwork.IsMasterClient && !_canPlayWithBots)
            {
                CloudSaveManager.SetValue<int>("playWithBots", 0);
                LoadSceneManager.PhotonLoadScene(GameScene.Game);
            }
        }

        //Local Player left room
        public override void OnLeftRoom()
        {
            OnLeftRoomEvent?.Invoke();
            UIManager.ShowLast();
        }

        //Another Player joined room
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPlayerJoinedRoomEvent?.Invoke(newPlayer);
            UpdatePlayersCount();
            CheckPlayersToStart();
        }

        //Another Player left room
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
            UpdatePlayersCount();

            CancelInvoke();

            CheckPlayWithBots();
            CheckPlayersToStart();
        }


        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoom();
        }

        public void OnJoinRoomFailed()
        {
            print("Offline: " + PhotonNetwork.OfflineMode);
            PhotonNetwork.Disconnect();
            Invoke(nameof(OnCloudSavesResult), 2);
        }

        private void CheckPlayWithBots()
        {
            if (PhotonNetwork.IsMasterClient)
                Invoke(nameof(PlayWithBots), _settings.WaitForPlayersUntilPlayWithBots);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            PhotonNetwork.OfflineMode = true;
        }

        private void CheckPlayersToStart()
        {
            //Sadece master isek oyunun baþlayýp baþlamayacaðýný ayarlayabilriz
            if (!PhotonNetwork.IsMasterClient)
                return;

            if (PhotonNetwork.CurrentRoom.PlayerCount == _settings.PlayersInGame)
            {
                _canPlayWithBots = false;
                Invoke(nameof(PlayWithPlayers),_settings.WaitAfterAllPlayersEnteredToRoom);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && PhotonNetwork.CurrentRoom.PlayerCount < _settings.PlayersInGame)
            {
                //Herkesin girmesini beklemek yerine 2 kiþi veya fazla ise onun için de süre sayalým. Botlarla oynamaktan iyidir
                _canPlayWithBots = false;
                Invoke(nameof(PlayWithPlayers), _settings.WaitAfterSecondPlayerEnteredToRoom);
            }
        }

        private void UpdatePlayersCount()
        {
            UIManager.GetView<MatchingMenuView>().UpdatePlayersCount(PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
}
