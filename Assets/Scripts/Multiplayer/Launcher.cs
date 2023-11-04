using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CannonFightBase
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private static Launcher _instance;

        private Settings _settings;

        public static event Action OnJoinedRoomEvent;

        public static event Action OnLeftRoomEvent;

        public static event Action<Player> OnPlayerJoinedRoomEvent;

        public static event Action<Player> OnPlayerLeftRoomEvent;

        [SerializeField] private TMP_InputField _nameText;
        [SerializeField] private SceneContext _sceneContext;


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
        public void Construct(Settings settings)
        {
            _settings = settings;
        }


        void Start()
        {
            Connect();

            Invoke(nameof(Run), 2);
        }

        void Run()
        {
        }

        public void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }

        public void JoinRoom()
        {
            PhotonNetwork.LocalPlayer.NickName = _nameText.text;

            PhotonNetwork.JoinRandomRoom();
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

        private void CheckStartingGame()
        {
            print("PhotonNetwork.CurrentRoom.PlayerCount: " + PhotonNetwork.CurrentRoom.PlayerCount);
            if (PhotonNetwork.CurrentRoom.PlayerCount == _settings.MinPlayersCountToStart)
            {
                //PhotonNetwork.LocalPlayer.SetCustomProperties();
                UIManager.GetView<MatchingMenuView>().StartCountdown(_settings.GameStartCountdown,OnCountdownFinished);
            }
        }

        public override void OnConnectedToMaster()
        {
            //print("ConnectedMaster");
        }

        public override void OnConnected()
        {
            print("Connected Lobby");
        }

        //Local Player joined
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LocalPlayer.NickName = _nameText.text;

            UIManager.Show(UIManager.GetView<MatchingMenuView>());

            OnJoinedRoomEvent?.Invoke();

            //LoadingMenuView loadingMenuView = UIManager.Show<LoadingMenuView>();
            CheckStartingGame();
        }

        //Local Player left room
        public override void OnLeftRoom()
        {
            OnLeftRoomEvent?.Invoke();
            UIManager.ShowLast();
            //print("Left Room");
        }

        //Another Player joined room
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPlayerJoinedRoomEvent?.Invoke(newPlayer);

            CheckStartingGame();
        }

        //Another Player left room
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
        }


        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoom();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {

        }

        private void OnCountdownFinished()
        {
            if(PhotonNetwork.LocalPlayer.IsMasterClient)
                PhotonNetwork.LoadLevel(1);

        }

        [Serializable]
        public class Settings
        {
            public int MinPlayersCountToStart = 1;

            public int MinPlayersCountToLoadSettings = 2;

            public float GameStartCountdown = 1;
        }

    }
}
