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

        private SpawnPoint[] _spawnPoints;

        private PlayerManager _playerManager;

        [Inject]
        public void Construct(PlayerManager.Factory factory)
        {
            _playerManagerFactory = factory;
        }

        private void OnEnable()
        {
            GameEventReceiver.OnGameReadyToStartEvent += SpawnPlayerManager;
            PhotonNetwork.NetworkingClient.EventReceived += EVENT_SpawnPlayerManager;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnGameReadyToStartEvent -= SpawnPlayerManager;
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
            //print("----------------------Spawn Player Managers---------------------");
            if (_playerManager != null)
                return;

            _playerManager = _playerManagerFactory.Create();
            PhotonView photonView = _playerManager.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(photonView))
            {
                object[] data = new object[]
                {
                    _playerManager.transform.position, _playerManager.transform.rotation, photonView.ViewID
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
                Destroy(_playerManager);
            }

            _playerManager.Initialize();
        }

        public void EVENT_SpawnPlayerManager(EventData photonEvent)
        {

            if (photonEvent.Code == EventCodeManager.SPAWN_PLAYER_MANAGER_EVENT_CODE)
            {
                //if(_playerManager == null)
                //{
                //    print("Bizimk daha oluþmamýþ önce onu oluþturalým");
                //    SpawnPlayerManager();
                //}
                object[] data = (object[])photonEvent.CustomData;
                PlayerManager playerManager = _playerManagerFactory.Create();
                PhotonView photonView = playerManager.GetComponent<PhotonView>();
                photonView.ViewID = (int)data[2];
                //print("----------------------Spawn Player Managers Misafir---------------------");

                //print("------------- Girdi Event: " + photonView.ViewID + " --------------------");
            }
        }

        public static SpawnPoint GetSpawnPoint()
        {
            //print("PhotonNetwork.LocalPlayer.ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);
            //print("PhotonNetwork.CurrentRoom.PlayerCount: " + PhotonNetwork.CurrentRoom.PlayerCount);
            int index = 0;
            if(PhotonNetwork.IsConnected)
                index = (PhotonNetwork.LocalPlayer.ActorNumber > PhotonNetwork.CurrentRoom.PlayerCount) ? PhotonNetwork.LocalPlayer.ActorNumber - PhotonNetwork.CurrentRoom.PlayerCount: PhotonNetwork.LocalPlayer.ActorNumber - 1;
            
            return _instance._spawnPoints[index];
        }

    }
}
