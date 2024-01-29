using CannonFightUI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannonFightBase
{
    public class LoadingMenuView : UIView
    {
        [SerializeField] ScrollViewController _scrollViewController;

        [SerializeField] TextMeshProUGUI _txtPlayersCount;

        public override void Initialize()
        {
            Launcher.OnJoinedRoomEvent += OnJoinedRoom;
            Launcher.OnPlayerJoinedRoomEvent += OnPlayerJoinedRoom;
            Launcher.OnPlayerLeftRoomEvent += OnPlayerLeftRoom;
            Launcher.OnLeftRoomEvent += OnLeftRoom;
        }
        public override void AddSubViews()
        {
        }

        private void OnDisable()
        {
            Launcher.OnJoinedRoomEvent -= OnJoinedRoom;
            Launcher.OnPlayerJoinedRoomEvent -= OnPlayerJoinedRoom;
            Launcher.OnPlayerLeftRoomEvent-= OnPlayerLeftRoom;
            Launcher.OnLeftRoomEvent -= OnLeftRoom;
        }

        private void OnPlayerLeftRoom(Player player)
        {
            //_scrollViewController.RemovePlayerItem(player);
            UpdatePlayersCount();
        }

        private void OnJoinedRoom()
        {
            //_scrollViewController.UpdateItemList();
            UpdatePlayersCount();
        }

        public void OnPlayerJoinedRoom(Player player)
        {

            //_scrollViewController.AddPlayerItem(player);
            UpdatePlayersCount();
        }

        public void OnLeftRoom()
        {
            //_scrollViewController.ClearItemList();
            UpdatePlayersCount();
        }


        private void UpdatePlayersCount()
        {
            if(PhotonNetwork.CurrentRoom != null)
                _txtPlayersCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }


    }
}
