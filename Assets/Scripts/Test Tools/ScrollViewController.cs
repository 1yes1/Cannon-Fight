using EasyButtons;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    [ExecuteInEditMode]
    public class ScrollViewController : MonoBehaviour
    {
        [SerializeField] private ScrollPlayerItem playerItemPrefab;

        [SerializeField] private float height = 0;

        private List<ScrollPlayerItem> playerItems;

        private ScrollRect scrollRect;


        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            playerItems = new List<ScrollPlayerItem>();
        }

        public void UpdateItemPositions()
        {
            int activeCount = 0;

            for (int i = 0; i < playerItems.Count; i++)
            {
                ScrollPlayerItem item = playerItems[i];

                if (!item.gameObject.activeSelf || item == null)
                    continue;

                RectTransform rect = item.GetComponent<RectTransform>();
                rect.anchorMax.Set(0.5f, 1);
                rect.anchorMin.Set(0.5f, 1);

                rect.anchoredPosition = new Vector2(0, (-height / 2f) - activeCount * height);

                activeCount++;
            }

            Vector2 size = scrollRect.content.sizeDelta;
            size.y = height * activeCount;
            scrollRect.content.sizeDelta = size;
        }

        public void UpdateItemList()
        {
            Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;

            ClearItemList();
            foreach (Player player in players.Values)
            {
                AddPlayerItem(player);
            }
        }

        public ScrollPlayerItem AddPlayerItem(Player player)
        {
            ScrollPlayerItem playerItem = Instantiate(playerItemPrefab).GetComponent<ScrollPlayerItem>();
            playerItem.Initialize(player);

            if (player.IsLocal)
                playerItem.GetComponent<Image>().color = Color.yellow;

            playerItem.GetComponentInChildren<TextMeshProUGUI>().text = playerItem.NickName;
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
