using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CannonFightBase
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private static Launcher _instance;

        public static event Action OnJoinedRoomEvent;

        public static event Action OnLeftRoomEvent;

        public static event Action<Player> OnPlayerJoinedRoomEvent;

        public static event Action<Player> OnPlayerLeftRoomEvent;

        [SerializeField] private TMP_InputField _nameText;

        private string _gameVersion = "1";

        public static Launcher Instance => _instance;

        void Awake()
        {
            if(_instance == null)
                _instance = this;

            if(!PhotonNetwork.LocalPlayer.IsMasterClient)
                PhotonNetwork.AutomaticallySyncScene = true;

        }


        void Start()
        {
            Connect();
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
            if (PhotonNetwork.CurrentRoom.PlayerCount == RoomManager.DefaultRoomProperties.MinPlayersCountToStart)
            {
                //PhotonNetwork.LocalPlayer.SetCustomProperties();
                UIManager.GetView<MatchingMenuView>().StartCountdown(OnCountdownFinished);
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

    }
}
