using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannonFightBase
{
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager _instance;

        public event Action OnSpawnPlayersEvent;

        private SpawnPoint[] _spawnPoints;

        private void OnEnable()
        {
            GameEventReceiver.OnGameSceneLoadedEvent += SpawnPlayerManagers;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnGameSceneLoadedEvent -= SpawnPlayerManagers;
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

        public void SpawnPlayerManagers()
        {
            PlayerManager playerManager = PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity).GetComponent<PlayerManager>();

            SpawnPoint spawnPoint = GetSpawnPoint();

            playerManager.Initialize(spawnPoint);
        }

        private SpawnPoint GetSpawnPoint()
        {
            //print("PhotonNetwork.LocalPlayer.ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);
            //print("PhotonNetwork.CurrentRoom.PlayerCount: " + PhotonNetwork.CurrentRoom.PlayerCount);
            int index = (PhotonNetwork.LocalPlayer.ActorNumber > PhotonNetwork.CurrentRoom.PlayerCount) ? PhotonNetwork.LocalPlayer.ActorNumber - PhotonNetwork.CurrentRoom.PlayerCount: PhotonNetwork.LocalPlayer.ActorNumber - 1;

            return _spawnPoints[index];
        }

    }
}
