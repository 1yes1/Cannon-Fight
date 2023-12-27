using CannonFightUI;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using static CannonFightBase.GameLoadingManager;
using Random = UnityEngine.Random;

namespace CannonFightBase
{
    public class GameLoadingView : UIView
    {
        [SerializeField] private Transform _itemsParent;

        private BotPlayerItemSettings _itemSettings;

        private List<PlayerItem> _items;

        private PlayerItem _itemPrefab;

        private string[] _playerNames = new string[3];

        private Sprite[] _playerPictures = new Sprite[3];

        private int _botCount;

        [Inject]
        public void Construct(PlayerItem playerItem, BotPlayerItemSettings botPlayerItemSettings)
        {
            _itemPrefab = playerItem;
            _itemSettings = botPlayerItemSettings;
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

        public void AddBotPlayerItem(int count = 3)
        {
            for (int i = 0; i < count; i++)
            {
                PlayerItem playerItem = Instantiate(_itemPrefab, _itemsParent) as PlayerItem;
                string name;
                Sprite sprite;
                GetRandomPlayerInfo(out name, out sprite);
                playerItem.Initialize(name, sprite);
            }
        }

        private void GetRandomPlayerInfo(out string name,out Sprite sprite)
        {
            int rndName = Random.Range(0, _itemSettings.Names.Count);

            while (_playerNames.Contains(_itemSettings.Names[rndName]))
                rndName = Random.Range(0, _itemSettings.Names.Count);

            int rndSprite = Random.Range(0,_itemSettings.Pictures.Count);

            while (_playerPictures.Contains(_itemSettings.Pictures[rndSprite]))
                rndSprite = Random.Range(0, _itemSettings.Pictures.Count);

            name = _itemSettings.Names[rndName];
            sprite = _itemSettings.Pictures[rndSprite];

            _playerNames[_botCount] = name;
            _playerPictures[_botCount] = sprite;
            _botCount++;
            //_itemSettings.Names.RemoveAt(rndName);
            //_itemSettings.Pictures.RemoveAt(rndSprite);
        }

        public void RemovePlayerItem(Player player)
        {
            PlayerItem playerItem = _items.Find(x => x.Player == player);
            _items.Remove(playerItem);
            Destroy(playerItem.gameObject);
        }


        [Serializable]
        public struct BotPlayerItemSettings
        {
            public List<string> Names;
            public List<Sprite> Pictures;
        }


    }
}
