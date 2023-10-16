using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace CannonFightBase
{
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager _instance;

        private PlayerManager.Factory _playerManagerFactory;



        public event Action OnSpawnPlayersEvent;

        private SpawnPoint[] _spawnPoints;

        [Inject]
        public void Construct(PlayerManager.Factory factory)
        {
            _playerManagerFactory = factory;
        }

        private void OnEnable()
        {
            GameEventReceiver.OnGameSceneLoadedEvent += SpawnPlayerManager;
            PhotonNetwork.NetworkingClient.EventReceived += EVENT_SpawnPlayerManager;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnGameSceneLoadedEvent -= SpawnPlayerManager;
            PhotonNetwork.NetworkingClient.EventReceived -= EVENT_SpawnPlayerManager;
        }

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            _spawnPoints = GetComponentsInChildren<SpawnPoint>();

        }


        public void SpawnPlayerManager()
        {
            PlayerManager playerManager = _playerManagerFactory.Create();
            PhotonView photonView = playerManager.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(photonView))
            {
                object[] data = new object[]
                {
                    playerManager.transform.position, playerManager.transform.rotation, photonView.ViewID
                };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                PhotonNetwork.RaiseEvent(EventCodeManager.SPAWN_PLAYER_MANAGER_EVENT_CODE, data, raiseEventOptions, SendOptions.SendUnreliable);
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewId.");
                Destroy(playerManager);
            }

            playerManager.Initialize();
        }

        public void EVENT_SpawnPlayerManager(EventData photonEvent)
        {
            if (photonEvent.Code == EventCodeManager.SPAWN_PLAYER_MANAGER_EVENT_CODE)
            {
                object[] data = (object[])photonEvent.CustomData;
                PlayerManager playerManager = _playerManagerFactory.Create();
                PhotonView photonView = playerManager.GetComponent<PhotonView>();
                photonView.ViewID = (int)data[2];
                //print("------------- Girdi Event: "+ photonView.ViewID+" --------------------");
            }
        }

        public static SpawnPoint GetSpawnPoint()
        {
            //print("PhotonNetwork.LocalPlayer.ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);
            //print("PhotonNetwork.CurrentRoom.PlayerCount: " + PhotonNetwork.CurrentRoom.PlayerCount);
            int index = (PhotonNetwork.LocalPlayer.ActorNumber > PhotonNetwork.CurrentRoom.PlayerCount) ? PhotonNetwork.LocalPlayer.ActorNumber - PhotonNetwork.CurrentRoom.PlayerCount: PhotonNetwork.LocalPlayer.ActorNumber - 1;

            return _instance._spawnPoints[index];
        }

    }
}
