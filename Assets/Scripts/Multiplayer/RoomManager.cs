using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using Zenject;

namespace CannonFightBase
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        private static RoomManager _instance;

        public static event Action<Player> OnPlayerEnteredRoomEvent;
        public static event Action<Player> OnPlayerLeftRoomEvent;

        private RoomServerSettings _settings;

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
                Debug.Log("NICKNAME: " + PhotonNetwork.LocalPlayer.NickName);
                GameEventCaller.Instance.OnGameSceneLoaded();
            }
        }

        public override void OnJoinedRoom()
        {

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

            public float WaitForPlayersUntilPlayWithBots;

            public float WaitAfterAllPlayersEnteredToRoom;

            public float WaitAfterSecondPlayerEnteredToRoom;

        }

    }
}