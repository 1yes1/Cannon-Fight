using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using CannonFightUI;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

namespace CannonFightBase
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        private static RoomManager _instance;

        private bool _isGameSceneLoaded = false;

        public static RoomManager Instance => _instance;

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

            DontDestroyOnLoad(gameObject);
            _instance = this;
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(scene.name == "Game")
            {
                Debug.Log("NICKNAME: " + PhotonNetwork.LocalPlayer.NickName);
                GameEventCaller.Instance.OnGameSceneLoaded();
                _isGameSceneLoaded = true;
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            print("Left ROOOOOMMM " + otherPlayer.NickName);
            if(_isGameSceneLoaded)
                GameEventCaller.Instance?.OnPlayerLeftRoom(otherPlayer);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            print("Master Client Switched");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            print("Giriþ Ypatý");
            if(_isGameSceneLoaded)
                GameEventCaller.Instance?.OnPlayerEnteredRoom(newPlayer);
        }

    }
}