using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class PlayerItemController : MonoBehaviour
    {
        private PlayerItemPlace[] _playerItemPlaces;
        [SerializeField] protected PlayerItem _playerItemRightPrefab;
        [SerializeField] protected PlayerItem _playerItemLeftPrefab;
        private List<PlayerItem> _playerItems;

        private void OnEnable()
        {
            Launcher.OnJoinedRoomEvent += OnJoinedRoom;
            Launcher.OnPlayerJoinedRoomEvent += OnPlayerJoinedRoom;
            Launcher.OnPlayerLeftRoomEvent += OnPlayerLeftRoom;
        }

        private void OnDisable()
        {
            Launcher.OnJoinedRoomEvent -= OnJoinedRoom;
            Launcher.OnPlayerJoinedRoomEvent -= OnPlayerJoinedRoom;
            Launcher.OnPlayerLeftRoomEvent -= OnPlayerLeftRoom;
        }

        private void Awake()
        {
            _playerItemPlaces = GetComponentsInChildren<PlayerItemPlace>();
            _playerItems = new List<PlayerItem>();
        }

        //Our player joined to the room
        private void OnJoinedRoom()
        {

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                CreatePlayerItem(PhotonNetwork.LocalPlayer);
            }
            else
            {
                //CreatePlayerItem(PhotonNetwork.LocalPlayer);
                CreateAllPlayerItems();
            }
        }

        private void OnPlayerJoinedRoom(Player player)
        {
            CreatePlayerItem(player);
        }

        private void OnPlayerLeftRoom(Player player)
        {
            RemovePlayerItem(player);
        }

        private void CreatePlayerItem(Player player)
        {
            PlayerItem playerItem;
            bool isInTheRight = false;

            if(_playerItems.Count % 2 == 0)
                playerItem = Instantiate(_playerItemLeftPrefab);
            else
            {
                playerItem = Instantiate(_playerItemRightPrefab);
                isInTheRight = true;
            }

            playerItem.Initialize(player);
            _playerItems.Add(playerItem);
            PlaceItem(playerItem , _playerItems.Count - 1, isInTheRight);
        }

        private void RemovePlayerItem(Player player)
        {
            for (int i = 0; i < _playerItems.Count; i++)
            {
                if (_playerItems[i].Player == player)
                { 
                    PlayerItem item = _playerItems[i];
                    _playerItems.Remove(item);
                    Destroy(item.gameObject);
                    break; 
                }
            }
            PlaceAllPlayerItems();
        }

        private void PlaceAllPlayerItems()
        {
            for (int i = 0; i < _playerItems.Count; i++)
            {
                if(_playerItems.Count % 2 == 0)
                    PlaceItem(_playerItems[i], i);
                else
                    PlaceItem(_playerItems[i], i,true);
            }
        }

        private void CreateAllPlayerItems()
        {
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                CreatePlayerItem(player);
            }

            //PlaceAllPlayerItems();
        }

        private void PlaceItem(PlayerItem playerItem,int placeIndex = -1,bool isInTheRight = false)
        {
            if(placeIndex == -1)
                placeIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;

            playerItem.transform.SetParent(_playerItemPlaces[placeIndex].transform);
            _playerItemPlaces[placeIndex].PlacePlayerItem(playerItem, isInTheRight);

        }

    }

}
