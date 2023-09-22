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
                //if (PhotonNetwork.IsMasterClient)
                //    PhotonNetwork.LocalPlayer.NickName = "MasterrYunusss";
                //else
                //    PhotonNetwork.LocalPlayer.NickName = "Client" + UnityEngine.Random.Range(0, 9999);

                Debug.Log("NICKNAME: " + PhotonNetwork.LocalPlayer.NickName);
                GameEventCaller.Instance.OnGameSceneLoaded();
                //print("PhotonNetwork.CurrentRoom.PlayerCount: " + PhotonNetwork.CurrentRoom.PlayerCount);
                //PlayerManager playerManager = PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity).GetComponent<PlayerManager>();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {

        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            print("Master Client Switched");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            print("Giriþ Ypatý");
            GameEventCaller.Instance.OnPlayerEntered(newPlayer);
        }

    }
}