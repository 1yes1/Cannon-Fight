using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

        private CannonManager.Factory _cannonManagerFactory;

        private AgentManager.Factory _agentManagerFactory;

        private SpawnPoint[] _spawnPoints;

        private CannonManager _cannonManager;

        private int _spawnedPlayersCount = -1;

        public static SpawnManager Instance => _instance;

        public static int SpawnedPlayersCount => _instance._spawnedPlayersCount;

        [Inject]
        public void Construct(CannonManager.Factory factory,AgentManager.Factory agentManagerFactory, BotPlayerItemSettings botPlayerItemSettings, GameAgentSettings gameAgentSettings)
        {
            _cannonManagerFactory = factory;
            _agentManagerFactory = agentManagerFactory;
            _botPlayerItemSettings = botPlayerItemSettings;
            _gameAgentSettings = gameAgentSettings;
        }


        private void OnEnable()
        {
            GameEventReceiver.OnGameReadyToStartEvent += OnGameReadyToStart;
            PhotonNetwork.NetworkingClient.EventReceived += EVENT_SpawnPlayerManager;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnGameReadyToStartEvent -= OnGameReadyToStart;
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
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom && !GameManager.PlayWithBots)
                index = (PhotonNetwork.LocalPlayer.ActorNumber > PhotonNetwork.CurrentRoom.PlayerCount) ? PhotonNetwork.LocalPlayer.ActorNumber - PhotonNetwork.CurrentRoom.PlayerCount : PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if (GameManager.PlayWithBots)
            {
                _instance._spawnedPlayersCount++;
                index = _instance._spawnedPlayersCount;
            }

            if (GameManager.Instance != null && GameManager.IsTutorialScene)
                index = 0;

            spawnPoint = _instance._spawnPoints[index];
            spawnPoint.IsUsing = true;
            
            return spawnPoint;
        }

        private void OnGameReadyToStart()
        {
            SpawnCannonManager();
            SpawnBotsCheck();
        }

        public CannonManager SpawnCannonManager()
        {
            //print("----------------------Spawn Player Managers---------------------");
            if (_cannonManager != null)
                return null;

            _cannonManager = _cannonManagerFactory.Create();
            PhotonView photonView = _cannonManager.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(photonView))
            {
                object[] data = new object[]
                {
                    _cannonManager.transform.position, _cannonManager.transform.rotation, photonView.ViewID
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
                Destroy(_cannonManager);
            }

            _cannonManager.SetNameAndPicture(UserManager.Instance.Nickname,null);

            _cannonManager.Initialize();
            return _cannonManager;
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
                CannonManager playerManager = _cannonManagerFactory.Create();
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


        public AgentManager[] SpawnBots(int count)
        {
            AgentManager[] result = new AgentManager[count];
            for (int i = 0; i < count; i++)
            {
                AgentManager agentManager = _agentManagerFactory.Create();
                agentManager.Initialize();
                agentManager.SetNameAndPicture(_botPlayerItemSettings.Names[i], _botPlayerItemSettings.Pictures[i]);
                result[i] = agentManager;
            }
            return result;
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
