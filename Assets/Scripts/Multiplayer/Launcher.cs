using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
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

        private RoomManager.RoomServerSettings _settings;

        [SerializeField] private TMP_InputField _nameText;

        [SerializeField] private SceneContext _sceneContext;

        private bool _canPlayWithBots = true;

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
        public void Construct(RoomManager.RoomServerSettings settings)
        {
            _settings = settings;
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

        public override void OnConnected()
        {
            print("Connected Lobby");
        }

        //Local Player joined
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LocalPlayer.NickName = _nameText.text;

            UIManager.Show(UIManager.GetView<MatchingMenuView>());

            SaveManager.SetValue<int>("playWithBots", 0);//Ýlk baþta eðer playerprefs varsa diye sýfýrlayalým duruma göre 1 yapýalcak

            OnJoinedRoomEvent?.Invoke();

            UpdatePlayersCount();

            CheckPlayersToStart();

            CheckPlayWithBots();

            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.CurrentRoom.MaxPlayers = _settings.PlayersInGame;
        }

        private void PlayWithBots()
        {
            if (PhotonNetwork.IsMasterClient && _canPlayWithBots)
            {
                SaveManager.SetValue<int>("playWithBots",1);
                print("PlayWithBots");

                PhotonNetwork.LoadLevel(1);
            }
        }


        private void PlayWithPlayers()
        {
            if (PhotonNetwork.IsMasterClient && !_canPlayWithBots)
            {
                SaveManager.SetValue<int>("playWithBots", 0);
                print("PlayWithPlayers");

                PhotonNetwork.LoadLevel(1);
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

        public override void OnMasterClientSwitched(Player newMasterClient)
        {

        }

        private void CheckPlayWithBots()
        {
            if (PhotonNetwork.IsMasterClient)
                Invoke(nameof(PlayWithBots), _settings.WaitForPlayersUntilPlayWithBots);
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
                Debug.Log("Play with players but not all");
            }
        }

        private void UpdatePlayersCount()
        {
            UIManager.GetView<MatchingMenuView>().UpdatePlayersCount(PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
}
