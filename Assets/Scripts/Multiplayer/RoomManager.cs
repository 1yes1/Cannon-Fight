using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using Zenject;
using CannonFightUI;

namespace CannonFightBase
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        private static RoomManager _instance;

        public static event Action<Player> OnPlayerEnteredRoomEvent;
        public static event Action<Player> OnPlayerLeftRoomEvent;

        private RoomServerSettings _settings;

        public static bool IsFirstFight
        {
            get => CloudSaveManager.GetValue<int>("goFirstFight") == 1;
            set => CloudSaveManager.SetValue<int>("goFirstFight",(value) ? 1 : 0);
        }

        public static RoomManager Instance => _instance;

        [Inject]
        public void Construct(RoomServerSettings settings)
        {
            _settings = settings;
        }

        private void OnEnable()
        {
            base.OnEnable();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void Awake()
        {
            if(_instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }
            PhotonNetwork.EnableCloseConnection = true;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(scene.name == "Game")
            {
                GameEventCaller.Instance.OnGameSceneLoaded();
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        public override void OnConnected()
        {
            PhotonNetwork.MaxResendsBeforeDisconnect = 4;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            //if(targetPlayer.CustomProperties.ContainsKey("isDead") && (bool)changedProps["isDead"])
            //{
            //    GameEventCaller.Instance.OnPlayerDie(targetPlayer);
            //}
            //else if ((string)changedProps["killerPlayer"] != "")
            //{
            //    print("Girdi Kill Target: "+targetPlayer);
            //    Player killerPlayer = null;
            //    foreach (var item in PhotonNetwork.CurrentRoom.Players.Values)
            //    {
            //        if (item.UserId == (string)changedProps["killerPlayer"])
            //            killerPlayer = item;
            //    }

            //    print("Girdi Kill Killer: " + killerPlayer);

            //    GameEventCaller.Instance.OnKill(killerPlayer, targetPlayer);
            //}
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            print("DISCONNECTED: "+cause.ToString());

            PhotonNetwork.OfflineMode = true;
            if(cause != DisconnectCause.DisconnectByClientLogic)
                InGameDisconnect("Internet Connection Lost!");
        }

        public void InGameDisconnect(string warningText)
        {

            if (SceneManager.GetActiveScene().buildIndex == (int)GameScene.Game)
            {
                UIManager.Show<WarningView>().CreateWarning(warningText);
                LoadSceneManager.LoadScene(GameScene.Menu,2);
            }
        }


        [Serializable]
        public struct Settings
        {
            public GameServerSettings GameServerSettings;

            public RoomServerSettings RoomServerSettings;
        }

        [Serializable]
        public struct GameServerSettings
        {
            public float GameStartCountdown;

            public float WaitAfterAllPlayersEnteredToGame;

        }

        [Serializable]
        public struct RoomServerSettings
        {
            public int PlayersInGame;

            public float PhotonConnectTimeout;

            public float WaitForPlayersUntilPlayWithBots;

            public float WaitAfterAllPlayersEnteredToRoom;

            public float WaitAfterSecondPlayerEnteredToRoom;

        }

    }
}