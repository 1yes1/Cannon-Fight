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
using static CannonFightBase.AIManager;
using static CannonFightBase.GameLoadingView;

namespace CannonFightBase
{
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager _instance;

        private GameAgentSettings _gameAgentSettings;

        private BotPlayerItemSettings _botPlayerItemSettings;

        private CannonManager.Factory _playerManagerFactory;

        private AgentManager.Factory _agentManagerFactory;

        private SpawnPoint[] _spawnPoints;

        private CannonManager _playerManager;

        private int _spawnedPlayersCount = -1;

        [Inject]
        public void Construct(CannonManager.Factory factory,AgentManager.Factory agentManagerFactory, BotPlayerItemSettings botPlayerItemSettings, GameAgentSettings gameAgentSettings)
        {
            _playerManagerFactory = factory;
            _agentManagerFactory = agentManagerFactory;
            _botPlayerItemSettings = botPlayerItemSettings;
            _gameAgentSettings = gameAgentSettings;
        }


        private void OnEnable()
        {
            GameEventReceiver.OnGameReadyToStartEvent += SpawnPlayerManager;
            GameEventReceiver.OnGameReadyToStartEvent += SpawnBotsCheck;
            PhotonNetwork.NetworkingClient.EventReceived += EVENT_SpawnPlayerManager;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnGameReadyToStartEvent -= SpawnPlayerManager;
            GameEventReceiver.OnGameReadyToStartEvent -= SpawnBotsCheck;
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

        public static SpawnPoint GetSpawnPoint()
        {
            SpawnPoint spawnPoint;

            //print("PhotonNetwork.LocalPlayer.ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);
            //print("PhotonNetwork.CurrentRoom.PlayerCount: " + PhotonNetwork.CurrentRoom.PlayerCount);
            int index = 0;
            if (PhotonNetwork.IsConnected)
                index = (PhotonNetwork.LocalPlayer.ActorNumber > PhotonNetwork.CurrentRoom.PlayerCount) ? PhotonNetwork.LocalPlayer.ActorNumber - PhotonNetwork.CurrentRoom.PlayerCount : PhotonNetwork.LocalPlayer.ActorNumber - 1;


            if (GameManager.PlayWithBots)
            {
                _instance._spawnedPlayersCount++;
                index = _instance._spawnedPlayersCount;
            }
            spawnPoint = _instance._spawnPoints[index];
            spawnPoint.IsUsing = true;
            
            return spawnPoint;
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

            _playerManager.SetNameAndPicture(PhotonNetwork.LocalPlayer.NickName,null);

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
                CannonManager playerManager = _playerManagerFactory.Create();
                PhotonView photonView = playerManager.GetComponent<PhotonView>();
                photonView.ViewID = (int)data[2];
                //print("----------------------Spawn Player Managers Misafir---------------------");

                //print("------------- Girdi Event: " + photonView.ViewID + " --------------------");
            }
        }


        private void SpawnBotsCheck()
        {
            if (GameManager.PlayWithBots)
                SpawnBots(_gameAgentSettings.MaxAgentCount);
        }


        private void SpawnBots(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AgentManager agentManager = _agentManagerFactory.Create();
                agentManager.Initialize();
                agentManager.SetNameAndPicture(_botPlayerItemSettings.Names[i], _botPlayerItemSettings.Pictures[i]);
            }
        }

        public void TEST_SpawnBot(int spawnCount)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                AgentManager agentManager = _agentManagerFactory.Create();
                agentManager.Initialize();
                agentManager.SetNameAndPicture(_botPlayerItemSettings.Names[i], _botPlayerItemSettings.Pictures[i]);
            }

            GameEventCaller.Instance.OnGameStarted();
        }

    }
}
