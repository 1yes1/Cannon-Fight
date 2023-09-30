using EasyButtons;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public class KillsManager : MonoBehaviour
    {

        [SerializeField] private KillItem _killItem;

        [SerializeField] private float _startPositionY;

        [SerializeField] private float _offsetY;

        [SerializeField] private float _waitTime = 3;

        [SerializeField] private float _hideTime = 1;

        [SerializeField] private float _moveUpSpeed = 4;


        private List<KillItem> _killItems;


        private void OnEnable()
        {
            GameEventReceiver.OnKillEvent += CreateKillItem;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnKillEvent -= CreateKillItem;
        }

        private void Awake()
        {
            _killItems = new List<KillItem>();
        }

        
        private void CreateKillItem(Player killer, Player dead)
        {
            if(_killItems == null)
                _killItems = new List<KillItem>();

            KillItem killItem = Instantiate(_killItem, transform);
            killItem.Initialize(killer.NickName, dead.NickName, _waitTime,_hideTime,PlaceKillItems);
            //killItem.Initialize("Öldüren", "Ölen", _waitTime, _hideTime,PlaceKillItems);

            RectTransform rectTransform = killItem.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, _startPositionY + _killItems.Count * _offsetY);
            _killItems.Add(killItem);
        }


        private void PlaceKillItems(KillItem killItem)
        {
            _killItems.Remove(killItem);

            for (int i = 0; i < _killItems.Count; i++)
            {
                Vector2 pos = new Vector2(0, _startPositionY + i * _offsetY);
                _killItems[i].MoveToPlace(pos, _moveUpSpeed);
            }

        }


    }
}
