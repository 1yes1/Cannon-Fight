using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class ScoreboardScrollMenu : MonoBehaviour
    {
        [SerializeField] private ScrollPlayerItem playerItemPrefab;

        [SerializeField] private float height = 0;

        private List<ScrollPlayerItem> playerItems;

        private ScrollRect scrollRect;

        private void OnEnable()
        {
            //RoomManager.OnNewPlayerJoinedEvent += RefreshItemList;
            //RoomManager.OnGameSceneLoadedEvent += RefreshItemList;
        }


        public void Initialize()
        {
            scrollRect = GetComponent<ScrollRect>();
            playerItems = new List<ScrollPlayerItem>();
        }

        private void UpdateItemPositions()
        {
            int activeCount = 0;

            for (int i = 0; i < playerItems.Count; i++)
            {
                ScrollPlayerItem item = playerItems[i];

                if (!item.gameObject.activeSelf || item == null)
                    continue;

                RectTransform rectTransform = item.GetComponent<RectTransform>();
                //rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
                //rect.rect.Set(1, 2, 3, height);
                rectTransform.anchorMax.Set(0, 1);
                rectTransform.anchorMin.Set(1, 1);
                //rectTransform.offsetMax.Set(0, 0);
                //rectTransform.offsetMin.Set(0, 0);
                rectTransform.offsetMin = new Vector2(0,0);
                rectTransform.offsetMax = new Vector2(0, height);
                rectTransform.anchoredPosition = new Vector2(0, (-height / 2f) - activeCount * height);

                activeCount++;
            }

            //Vector2 size = scrollRect.content.sizeDelta;
            //size.y = height * activeCount;
            //scrollRect.content.sizeDelta = size;
        }

        public void RefreshItemList()
        {
            Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;

            ClearItemList();
            foreach (Player player in players.Values)
            {
                AddPlayerItem(player);
            }
        }

        public void UpdateItemList()
        {
            for (int i = 0; i < playerItems.Count; i++)
                playerItems[i].UpdateKillCount();
        }

        public ScrollPlayerItem AddPlayerItem(Player player)
        {
            ScrollPlayerItem playerItem = Instantiate(playerItemPrefab).GetComponent<ScrollPlayerItem>();
            playerItem.Initialize(player);

            if (player.IsLocal)
                playerItem.GetComponent<Image>().color = Color.yellow;

            playerItem.transform.SetParent(scrollRect.content);
            playerItem.GetComponent<RectTransform>().localScale = Vector2.one;
            playerItems.Add(playerItem);

            UpdateItemPositions();

            return playerItem;
        }

        public void RemovePlayerItem(Player player)
        {
            ScrollPlayerItem scrollPlayerItem = null;
            for (int i = 0; i < playerItems.Count; i++)
            {
                if (playerItems[i].Player == player)
                {
                    scrollPlayerItem = playerItems[i];
                    playerItems.Remove(scrollPlayerItem);
                    Destroy(scrollPlayerItem.gameObject);
                    break;
                }
            }

            UpdateItemPositions();
        }

        public void ClearItemList()
        {
            if (playerItems == null)
                return;

            for (int i = 0; i < playerItems.Count; i++)
            {
                if (playerItems[i] != null)
                    Destroy(playerItems[i].gameObject);
            }

            playerItems.Clear();
        }


    }
}
