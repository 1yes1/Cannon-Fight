using CannonFightUI;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class GameLoadingView : UIView
    {
        [SerializeField] private Transform _itemsParent;

        private List<PlayerItem> _items;

        private PlayerItem _itemPrefab;

        [Inject]
        public void Construct(PlayerItem playerItem)
        {
            _itemPrefab = playerItem;
        }

        public override void Initialize()
        {
            _items = new List<PlayerItem>();
        }

        public override void Show()
        {
            base.Show();
        }

        public void AddPlayerItem(Player player)
        {
            PlayerItem playerItem = Instantiate(_itemPrefab,_itemsParent) as PlayerItem;
            playerItem.Initialize(player);
            _items.Add(playerItem);
        }

        public void RemovePlayerItem(Player player)
        {
            PlayerItem playerItem = _items.Find(x => x.Player == player);
            _items.Remove(playerItem);
            Destroy(playerItem.gameObject);
        }

    }
}
