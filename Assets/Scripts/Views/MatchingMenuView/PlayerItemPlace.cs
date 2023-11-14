using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class PlayerItemPlace : MonoBehaviour
    {
        private PlayerItem _playerItem;

        private bool _isEmpty = true;

        public bool IsEmpty => _isEmpty;


        public void PlacePlayerItem(PlayerItem playerItem,bool isInTheRight)
        {
            _playerItem = playerItem;
            _isEmpty = false;

            playerItem.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            playerItem.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            //playerItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            playerItem.GetComponent<RectTransform>().anchorMax = Vector2.one;
            playerItem.GetComponent<RectTransform>().anchorMin = Vector2.zero;

            if(isInTheRight)
                playerItem.GetComponent<RectTransform>().localScale = new Vector2(-1,1);
            else
                playerItem.GetComponent<RectTransform>().localScale = Vector2.one;

        }


    }
}
